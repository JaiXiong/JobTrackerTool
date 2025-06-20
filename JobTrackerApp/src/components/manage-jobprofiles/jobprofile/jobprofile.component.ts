import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MatFormFieldModule, MatLabel } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';
import { MatTabsModule } from '@angular/material/tabs';
import { MatIcon } from '@angular/material/icon';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { JobProfile } from '../../../models/job-profile.model';
import { EmployerProfile } from '../../../models/employer-profile.model';
import { EmployerprofileComponent } from '../../manage-employerprofiles/employerprofile/employerprofile.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { DialogJobprofilesComponent } from '../../manage-dialog-popups/dialog-jobprofiles/dialog-jobprofiles/dialog-jobprofiles.component';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { DialogEmployerprofilesComponent } from '../../manage-dialog-popups/dialog-employerprofiles/dialog-employerprofiles.component';
import { DialogEditJobprofilesComponent } from '../../manage-dialog-popups/dialog-jobprofiles/dialog-edit-jobprofiles/dialog-edit-jobprofiles.component';
import { BehaviorSubject, Observable, of, Subject, Subscription } from 'rxjs';
import { tap, catchError, switchMap, map, takeUntil } from 'rxjs/operators';
import { JobTrackerService } from '../../../services/jobtracker/jobtracker.service';
import { MatSort, MatSortModule, Sort } from '@angular/material/sort';
import {LiveAnnouncer} from '@angular/cdk/a11y';
import { NotificationService } from '../../../services/notifications/notification.service';
import { UploadModularComponent } from "../../modular/upload-modular/upload-modular.component";
import { DownloadModularComponent } from '../../modular/download-modular/download-modular.component';
import { MatMenuModule } from '@angular/material/menu';
import { UploadService } from '../../../services/upload/upload.service';

@Component({
  selector: 'app-jobprofile',
  standalone: true,
  imports: [
    RouterModule,
    CommonModule,
    MatLabel,
    MatInputModule,
    FormsModule,
    MatTabsModule,
    MatIcon,
    MatTableModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatPaginator,
    MatPaginatorModule,
    MatDialogModule,
    MatTooltipModule,
    MatSnackBarModule,
    MatSortModule,
    UploadModularComponent,
    DownloadModularComponent,
    MatMenuModule,
],
  providers: [
    DatePipe,
  ],
  //TODO
  //Eventually to use this for performance optimization we need to try to create components that are pure
  //This means that the component will only update when the input changes
  //But right now this falls under the umbrella of lost time, we've already implemented the way it works and going back to change it would take time to refractor
  //changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './jobprofile.component.html',
  styleUrl: './jobprofile.component.scss',
})
export class JobprofileComponent implements OnInit, OnDestroy {
  title = 'JobTrackerApp';
  private uploadSubscription!: Subscription;
  private jobProfilesSubject = new BehaviorSubject<JobProfile[]>([]);
  @ViewChild('paginator', { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  totalRecords: number = 0;
  pageSize: number = 15;
  pageIndex: number = 0;
  userNameId: string = '';
  userName: string = '';
  jobProfiles: JobProfile[] = [];
  //jobProfile!: JobProfile;
  jobProfileSelected!: JobProfile | null;
  employerProfileSelected: string = '';
  displayedColumns: string[] = [
    'date',
    'name',
    'city',
    'state',
    'phone',
    'email',
    'website',
    'latestupdate',
    'actions'
  ];
  dataSource = new MatTableDataSource<EmployerProfile>();
  isSelected: boolean = false;
  showJobOptions: boolean = true;
  showEmployerOptions: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private jobTrackerService: JobTrackerService,
    private dialog: MatDialog,
    private datePipe: DatePipe,
    private cdr: ChangeDetectorRef,
    private _liveAnnouncer: LiveAnnouncer,
    private notificationService: NotificationService, 
    private uploadService: UploadService
  ) {}

  ngOnInit(): void {
    this.uploadSubscription = this.uploadService.uploadComplete$.subscribe(() => {
      this.getPageData();
    });
    // Retrieve the username from the query parameters
    this.route.queryParams.subscribe((params) => {
      this.userNameId = params['usernameid'];
      this.userName = params['name'];
    });

    if (localStorage.getItem('jobProfileId')) {
      this.isSelected = true;
      this.showEmployerOptions = true;
      this.jobProfileSelected = this.jobProfiles.find(
        (profile) => profile.id === localStorage.getItem('jobProfileId')
      ) as JobProfile;
    }

    this.getJobProfiles().subscribe({
      next: (profiles: JobProfile[]) => {
        this.jobProfiles = profiles;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error in subscription:', error);
      },
    });

    this.getPageData();
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }

  private destroy$ = new Subject<void>();

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    localStorage.removeItem('jobProfile');
  }

  

  // public getJobProfile(): void {
  //   if (this.jobProfileSelected) {
  //     console.error('No job profile selected');
  //     this.jobTrackerService
  //       .GetJobProfile(this.jobProfileSelected.id)
  //       .subscribe({
  //         next: (response: JobProfile) => {
  //           console.log(response);
  //           this.jobProfile = response;
  //         },
  //         error: (error) => {
  //           console.error('Failed to get job profile', error);
  //         },
  //       });
  //   }
  // }

  public getJobProfiles(): Observable<JobProfile[]> {
    if (!this.userNameId) {
      console.error('userNameId is undefined');
      return of([]);
    }

    return this.jobTrackerService.GetJobProfiles(this.userNameId).pipe(
      tap((response: JobProfile[]) => {
        if (!response) {
          console.warn('No profiles returned');
          this.jobProfiles = [];
          return;
        }
        this.jobProfiles = this.convertJobProfiles(response);
      }),
      catchError((error) => {
        console.error('Failed to get job profiles', error);
        this.jobProfiles = [];
        return of([]);
      }),
      takeUntil(this.destroy$)
    );
  }

  public convertJobProfiles(response: JobProfile[]): JobProfile[] {
    return response.map((element: JobProfile) => {
      return {
        //short for all properties in jobprofile
        ...element,
        date: this.datePipe.transform(element.date, 'M/dd/yyyy hh:mm a') as any,
        latestUpdate: this.datePipe.transform(
          element.latestUpdate,
          'M/dd/yyyy hh:mm a'
        ) as any,
      };
    });
  }

  public convertEmployerProfiles(
    response: EmployerProfile[]
  ): EmployerProfile[] {
    return response.map((element: EmployerProfile) => {
      return {
        //short for all properties in employerprofile
        ...element,
        date: this.datePipe.transform(element.date, 'M/dd/yyyy hh:mm a') as any,
        latestUpdate: this.datePipe.transform(
          element.latestUpdate,
          'M/dd/yyyy hh:mm a'
        ) as any,
      };
    });
  }

  public onNameClick(event: Event, element: EmployerProfile): void {
    event.preventDefault();
    this.employerProfileSelected = element.id;

    const dialogRef = this.dialog.open(EmployerprofileComponent, {
      width: '500px',
      height: '800px',
      data: element,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.jobTrackerService.UpdateEmployerProfile(result).subscribe({
          next: (response) => {
            //console.log('Employer profile updated successfully');
          },
          error: (error) => {
            console.error('Failed to update employer profile', error);
          },
        });
      }
    });
  }

  public onJobProfileChange(event: any) {
    this.jobProfileSelected = event.value;

    if (this.jobProfileSelected === null) {
      this.jobProfileSelected = null;
      localStorage.removeItem('jobProfileId');
      this.cdr.detectChanges();
    }

    if (this.jobProfileSelected) {
      localStorage.setItem('jobProfileId', this.jobProfileSelected.id);
    }

    if (!this.jobProfileSelected) {
      //console.error('No job profile selected during job profile change');
      return;
    }

    this.getPageData();
    this.isSelected = true;
    this.showJobOptions = false;
    this.showEmployerOptions = true;
  }

  public onPageChange(event: any): void {
    this.pageSize = event.pageSize;
    this.pageIndex = event.pageIndex;
    this.getPageData();
  }

  public getPageData(): void {
    const jobProfileId =
      this.jobProfileSelected?.id || localStorage.getItem('jobProfileId');

    if (!jobProfileId) {
      console.error('No job profile selected during paging');
      return;
    }

    this.getJobProfiles()
      .pipe(
        tap((profiles) => {
          this.jobProfiles = profiles;
          this.jobProfileSelected = profiles.find(
            (profile) => profile.id === jobProfileId
          ) as JobProfile;
          this.cdr.markForCheck();
        }),
        switchMap(() => {
          if (!this.jobProfileSelected) {
            console.error('No job profile found with the given ID');
            return of([]);
          }

          return this.jobTrackerService
            .GetEmployerPagingData(jobProfileId, this.pageIndex, this.pageSize)
            .pipe(
              tap((response) => {
                this.totalRecords = response.totalCount;
                this.dataSource.data = this.convertEmployerProfiles(response.data);
                this.dataSource.sort = this.sort;
                this.cdr.detectChanges();
              })
            );
        }),
        catchError((error) => {
          console.error('Failed to get paging data', error);
          return of([]);
        }),
        takeUntil(this.destroy$)
      )
      .subscribe({
        next: () => {
          this.cdr.detectChanges();
        },
        error: (error) => {
          console.error('Error in subscription:', error);
        },
      });
  }

  public download(): void {
    //console.log('Download button clicked!');
  }

  public onCreateJobProfile(): void {
    const dialogRef = this.dialog.open(DialogJobprofilesComponent, {
      width: '500px',
      disableClose: true,
      data: { userProfileId: this.userNameId },
    });

    //use switchMap to chain the observable
    //use behavior subject to emit the new job profiles because we're tracking the job profiles already
    dialogRef
      .afterClosed()
      .pipe(switchMap(() => this.getJobProfiles()))
      .subscribe((profiles) => {
        this.jobProfilesSubject.next(profiles);
      });
  }

  public onEditJobProfile(): void {
    const dialogRef = this.dialog.open(DialogEditJobprofilesComponent, {
      width: '500px',
      disableClose: true,
      data: this.jobProfileSelected,
    });

    dialogRef.afterClosed().subscribe(() => {
      const selectedJobProfileId = this.jobProfileSelected
        ? this.jobProfileSelected.id
        : null;

      //because the job profile was updated, we need to refresh the job profiles list
      //and select the updated job profile
      this.getJobProfiles().subscribe(() => {
        this.jobProfileSelected = this.jobProfiles.find(
          (profile) => profile.id === selectedJobProfileId
        ) as JobProfile;
        this.cdr.detectChanges();
      });
    });
  }

  public onDeleteJobProfile(): void {
    if (!this.jobProfileSelected) {
      console.error('No job profile selected');
      return;
    }
    this.jobTrackerService
      .DeleteJobProfile(this.jobProfileSelected.id)
      .subscribe({
        next: (response) => {
          this.notificationService.showNotification("Job profile deleted successfully", 5000);
          this.getJobProfiles();
          this.cdr.detectChanges();
        },
        error: (error) => {
          console.error('Failed to delete job profile', error);
          this.notificationService.showNotification("Failed to delete job profile", 5000);
        },
      });
  }

  public onCreateEmployerProfile(): void {
    const dialogRef = this.dialog.open(DialogEmployerprofilesComponent, {
      width: '500px',
      height: '675px',
      disableClose: true,
      data: {
        jobProfileId: this.jobProfileSelected
          ? this.jobProfileSelected.id
          : null,
      },
    });

    dialogRef.afterClosed().subscribe(() => {
      this.getPageData();
      this.cdr.detectChanges();
    });
  }

  public onEditEmployerProfile(event: Event, element: EmployerProfile): void {
    this.onNameClick(event, element);
  }

  public onDeleteEmployerProfile(): void {
    //console.log('Delete Employer Profile button clicked!');
    this.jobTrackerService
      .DeleteJobProfile(this.employerProfileSelected)
      .subscribe({
        next: (response) => {
          this.notificationService.showNotification("Employer profile deleted successfully", 5000);
          this.getPageData();
          this.cdr.detectChanges();
        },
        complete: () => {
          //console.log('Delete Employer Profile completed');
        },
        error: (error) => {
          console.error('Failed to delete employer profile', error);
          this.notificationService.showNotification("Failed to delete employer profile", 5000);
        },
      });
  }

  public sortData(sort: Sort) {
    if (sort.direction) {
      this._liveAnnouncer.announce(`Sorted ${sort.direction}ending`);
    } else {
      this._liveAnnouncer.announce('Sorting cleared');
    }
  }

  public onJobDefaultClick(): void {
    this.showJobOptions = true;
    this.isSelected = false;
    this.showEmployerOptions = false;
    this.dataSource = new MatTableDataSource<EmployerProfile>();
  }
}
