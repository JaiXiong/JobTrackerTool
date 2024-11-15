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
import { CommonModule, isPlatformBrowser } from '@angular/common';
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
  _jobProfiles: any;
  _jobProfile: any;
  _jobProfileSelected: any;
  _employerProfiles: any;
  displayedColumns: string[] = [
    'name',
    'city',
    'state',
    'phone',
    'email',
    'website',
  ];
  dataSource: EmployerProfile[] = [];
  _isSelected: boolean = false;

  ngOnInit(): void {
    // Retrieve the username from the query parameters
    this.route.queryParams.subscribe((params) => {
      this._userNameId = params['usernameid'];
    });
    this._jobProfiles = this.getJobProfiles();
  }

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private jobTrackerService: JobTrackerService,
    private dialog: MatDialog
  ) {}

  onCreateJobProfile() {
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
          const employerList = response.map(
            (element: {
              name: any;
              city: any;
              state: any;
              phone: any;
              email: any;
              website: any;
            }) => {
              return {
                //id: element.id,
                //date: new Date(element.date),
                name: element.name,
                //title: element.title,
                //address: element.address,
                city: element.city,
                state: element.state,
                //zip: element.zip,
                phone: element.phone,
                email: element.email,
                website: element.website,
              };
            }
          );
          this.dataSource = employerList;
        },
        (error) => {
          console.error('Failed to get employer profiles', error);
        }
      );
  }

  public getJobProfile() {
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

  public getJobProfiles() {
    this.jobTrackerService.GetJobProfiles(this._userNameId).subscribe(
      (response) => {
        const dropDownList = response.map(
          (element: { name: any; profileName: any }) => {
            element.name = element.profileName;
            return element;
          }
        );
        this._jobProfiles = dropDownList;
      },
      (error) => {
        console.error('Failed to get job profiles', error);
      }
    );
  }

  public convertJobProfiles(response: any) {
    const dropDownList = response.map(
      (element: { name: any; profileName: any }) => {
        element.name = element.profileName;
        return element;
      }
    );

    return dropDownList;
  }

  public convertEmployerProfiles(response: any) {
    const employerList = response.map(
      (element: {
        name: any;
        city: any;
        state: any;
        phone: any;
        email: any;
        website: any;
      }) => {
        return {
          //id: element.id,
          //date: element.date,
          name: element.name,
          //title: element.title,
          //address: element.address,
          city: element.city,
          state: element.state,
          //zip: element.zip,
          phone: element.phone,
          email: element.email,
          website: element.website,
        };
      }
    );

    return employerList;
  }
  public onNameClick(event: Event, element: EmployerProfile) {
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
      console.log('The dialog was closed');
      //this.jobTrackerService.UpdateEmployerProfile(result).subscribe(
        
    });
    // this.router.navigate(['/employerprofile'], {
    //   queryParams: { employerid: element.id },
    // });
  }

  public dialogPopup() {}


  public onJobProfileChange(event: any) {
    this._jobProfileSelected = event.value;
    this.getPageData();
    this._isSelected = true;
  }

  public onPageChange(event: any) {
    this.pageSize = event.pageSize;
    this.pageIndex = event.pageIndex;
    this.getPageData();
  }

  public getPageData() {
    // Call the service to get the data for the current page
    // const searchCriteria = {
    //   //sort/search criteria
    // };

    this.jobTrackerService
      .GetEmployerPagingData(
        this._jobProfileSelected.id,
        this.pageIndex,
        this.pageSize
      )
      .subscribe(
        (response) => {
          // Handle success response
          console.log('Paging data:', response);
          this.totalRecords = response.length ? response.length : 0;
          this.dataSource = this.convertEmployerProfiles(response);
        },
        (error) => {
          console.error('Failed to get paging data', error);
        }
      );
  }

  public download() {
    console.log('Download button clicked!');
  }
}
