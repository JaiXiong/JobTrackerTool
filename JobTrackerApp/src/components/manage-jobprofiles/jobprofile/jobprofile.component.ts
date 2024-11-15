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
import { EmployerprofileComponent } from '../../manage-employerprofiles/employerprofile/employerprofile.component';

export interface EmployerProfile {
  id: string;
  date: Date;
  latestUpdate: Date;
  jobProfileId: string;
  name: string;
  title: string;
  address: string;
  city: string;
  state: string;
  zip: string;
  phone: string;
  email: string;
  website: string;
}

export interface JobProfile {
  id: string;
  userProfileId: string;
  date: Date;
  latestUpdate: Date;
  profileName: string;
  //name: string;
}

@Component({
  selector: 'app-jobprofile',
  standalone: true,
  imports: [
    RouterModule,
    RouterOutlet,
    CommonModule,
    RouterLink,
    RouterLinkActive,
    MatLabel,
    MatInputModule,
    FormsModule,
    MatTabsModule,
    MatTabLabel,
    MatIcon,
    MatTableModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatPaginator,
    MatDialogModule,
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
  _jobProfiles: JobProfile[] = [];
  _jobProfile!: JobProfile;
  _jobProfileSelected: JobProfile = {
    id: '',
    userProfileId: '',
    date: new Date(),
    latestUpdate: new Date(),
    profileName: '',
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
  };
  _employerProfiles: EmployerProfile[] = [];
  displayedColumns: string[] = [
    'name',
    'city',
    'state',
    'phone',
    'email',
    'website',
    'date',
    'latestupdate',
  ];
  dataSource: EmployerProfile[] = [];
  _isSelected: boolean = false;

  ngOnInit(): void {
    // Retrieve the username from the query parameters
    this.route.queryParams.subscribe((params) => {
      this._userNameId = params['usernameid'];
    });
    this.getJobProfiles();
  }

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private jobTrackerService: JobTrackerService,
    private dialog: MatDialog,
    private datePipe: DatePipe
  ) {}

  onCreateJobProfile(): void {
    const jobProfile = {
      // Define the job profile data structure
      // Add other fields as needed
    };

    this.jobTrackerService.CreateJobProfile(jobProfile).subscribe(
      (response) => {
        console.log('Job profile created successfully', response);
        // Handle success response
      },
      (error) => {
        console.error('Failed to create job profile', error);
        // Handle error response
      }
    );
  }

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
    console.log('Name clicked:', element);
    for (let i = 0; i < this.dataSource.length; i++) {}
    const dialogRef = this.dialog.open(EmployerprofileComponent, {
      width: '500px',
      data: {
        name: element.name,
        city: element.city,
        state: element.state,
        phone: element.phone,
        email: element.email,
        website: element.website,
      },
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
}
