import { Component, ViewChild } from '@angular/core';
import { MatFormFieldModule, MatLabel } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {
  ActivatedRoute,
  Router,
  RouterLink,
  RouterLinkActive,
  RouterModule,
  RouterOutlet,
} from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';
import { MatTabLabel, MatTabsModule } from '@angular/material/tabs';
import { MatIcon } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { JobTrackerService } from '../../../services/jobtracker.service';
import { MatSelectModule } from '@angular/material/select';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { JobProfile } from '../../../models/job-profile.model';
import { EmployerProfile } from '../../../models/employer-profile.model';
import { EmployerprofileComponent } from '../../manage-employerprofiles/employerprofile/employerprofile.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { DialogJobprofilesComponent } from '../../manage-dialog-popups/dialog-jobprofiles/dialog-jobprofiles/dialog-jobprofiles.component';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

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
    MatDialogModule,
    MatTooltipModule,
    MatSnackBarModule
  ],
  providers: [
    //provideAnimations(),
    DatePipe,
  ],
  templateUrl: './jobprofile.component.html',
  styleUrl: './jobprofile.component.scss',
})
export class JobprofileComponent {
  title = 'JobTrackerApp';
  @ViewChild('paginator', { static: true }) paginator!: MatPaginator;
  totalRecords: number = 0;
  pageSize: number = 10;
  pageIndex: number = 0;
  _userNameId: string = '';
  _userName: string = '';
  _jobProfiles: JobProfile[] = [];
  _jobProfile!: JobProfile;
  _jobProfileSelected: JobProfile = {
    id: '',
    userProfileId: '',
    date: new Date(),
    latestUpdate: new Date(),
    profileName: '',
    employerProfiles: [],
  };
  _employerProfile: EmployerProfile = {
    id: '',
    date: new Date(),
    latestUpdate: new Date(),
    jobProfileId: '',
    name: '',
    title: '',
    address: '',
    city: '',
    state: '',
    zip: '',
    phone: '',
    email: '',
    website: '',
    jobAction: {
      id: '',
      employerProfileId: '',
      date: '',
      latestUpdate: '',
      action: '',
      method: '',
      actionResult: '',
    },
    details: {
      id: '',
      employerProfileId: '',
      date: '',
      latestUpdate: '',
      comments: '',
      updates: '',
    },
  };
  _employerProfiles: EmployerProfile[] = [];
  displayedColumns: string[] = [
    'date',
    'name',
    'city',
    'state',
    'phone',
    'email',
    'website',
    'latestupdate',
  ];
  dataSource: EmployerProfile[] = [];
  _isSelected: boolean = false;
  _showJobOptions: boolean = true;
  _showEmployerOptions: boolean = false;

  ngOnInit(): void {
    // Retrieve the username from the query parameters
    this.route.queryParams.subscribe((params) => {
      this._userNameId = params['usernameid'];
      this._userName = params['name'];
    });
    this.getJobProfiles();
  }

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private jobTrackerService: JobTrackerService,
    private dialog: MatDialog,
    private datePipe: DatePipe,
    private snackBar: MatSnackBar
  ) {}

  public getEmployerProfiles(): void {
    this.jobTrackerService
      .GetEmployerProfiles(this._jobProfileSelected.id)
      .subscribe(
        (response) => {
          //do something with the response
        },
        (error) => {
          console.error('Failed to get employer profiles', error);
        }
      );
  }

  public getJobProfile(): void {
    this.jobTrackerService
      .GetJobProfile(this._userNameId, this._jobProfile)
      .subscribe(
        (response) => {
          console.log(response);
          this._jobProfile = response;
        },
        (error) => {
          console.error('Failed to get job profile', error);
        }
      );
  }

  public getJobProfiles(): void {
    this.jobTrackerService.GetJobProfiles(this._userNameId).subscribe(
      (response) => {
        this._jobProfiles = this.convertJobProfiles(response);
      },
      (error) => {
        console.error('Failed to get job profiles', error);
      }
    );
  }

  public convertJobProfiles(response: any): JobProfile[] {
    return response.map((element: JobProfile) => {
      return {
        ...element,
        date: this.datePipe.transform(element.date, 'M/dd/yyyy hh:mm a') as any,
        latestUpdate: this.datePipe.transform(
          element.latestUpdate,
          'M/dd/yyyy hh:mm a'
        ) as any,
      };
    });
  }

  public convertEmployerProfiles(response: any): EmployerProfile[] {
    return response.map((element: EmployerProfile) => {
      return {
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
    event.preventDefault(); // Prevent the default anchor behavior

    const dialogRef = this.dialog.open(EmployerprofileComponent, {
      width: '500px',
      height: '800px',
      data: element,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.jobTrackerService.UpdateEmployerProfile(result).subscribe(
          (response) => {
            console.log('Employer profile updated successfully', response);
            // Handle success response
          },
          (error) => {
            console.error('Failed to update employer profile', error);
            // Handle error response
          }
        );
      }
    });
  }

  public dialogPopup(): void {}

  public onJobProfileChange(event: any) {
    this._jobProfileSelected = event.value;
    this.getPageData();
    this._isSelected = true;
    this._showJobOptions = false;
    this._showEmployerOptions = true;
  }

  public onPageChange(event: any): void {
    this.pageSize = event.pageSize;
    this.pageIndex = event.pageIndex;
    this.getPageData();
  }

  public getPageData(): void {
    if (!this._jobProfileSelected) {
      console.error('No job profile selected');
      return;
    }

    this.jobTrackerService
      .GetEmployerPagingData(
        this._jobProfileSelected.id,
        this.pageIndex,
        this.pageSize
      )
      .subscribe(
        (response) => {
          this.totalRecords = response.length ? response.length : 0;
          this.dataSource = this.convertEmployerProfiles(response);
        },
        (error) => {
          console.error('Failed to get paging data', error);
        }
      );
  }

  public download(): void {
    console.log('Download button clicked!');
  }

  public onCreateJobProfile(): void {
    console.log('Create button clicked!');
    const dialogRef = this.dialog.open(DialogJobprofilesComponent, {
      width: '500px',
      //height: '800px',
      data: { userProfileId: this._userNameId },
    });
    

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.jobTrackerService.CreateJobProfile(result).subscribe(
          (response) => {

            console.log('Job profile created successfully', response);
            this.snackBar.open('Job profile created successfully', 'Close', {
              duration: 2000,
              horizontalPosition: 'right', // Set horizontal position
              verticalPosition: 'top', // Set vertical position
            });

          },
          (error) => {

            console.error('Failed to create job profile', error);
            this.snackBar.open('Failed to create job profile', 'Close', {
              duration: 2000,
              horizontalPosition: 'right', // Set horizontal position
              verticalPosition: 'top', // Set vertical position
            });

          }
        );
      }
    });
  }

  public onEditJobProfile(): void {
    console.log('Update button clicked!');
    const dialogRef = this.dialog.open(DialogJobprofilesComponent, {
      width: '500px',
      height: '800px',
      data: this._employerProfile,
    });
    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.jobTrackerService.UpdateJobProfile(result).subscribe(
          (response) => {
            console.log('Employer profile updated successfully', response);
            // Handle success response
          },
          (error) => {
            console.error('Failed to update employer profile', error);
            // Handle error response
          }
        );
      }
    });
  }

  public onDeleteJobProfile(): void { 
    console.log('Delete button clicked!');
  }

  public onCreateEmployerProfile(): void {
    console.log('Create Employer Profile button clicked!');
  }

  public onEditEmployerProfile(): void {
    console.log('Edit Employer Profile button clicked!');
  }

  public onDeleteEmployerProfile(): void {
    console.log('Delete Employer Profile button clicked!');
  }

  public onJobDefaultClick(): void {
    console.log('Job Default button clicked!');
    this._showJobOptions = true;
    this._showEmployerOptions = false;
    this.dataSource = [];
  }
}
