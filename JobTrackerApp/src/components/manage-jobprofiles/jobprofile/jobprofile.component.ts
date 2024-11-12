import { Component, Inject, PLATFORM_ID } from '@angular/core';
import { MatFormFieldModule, MatLabel } from '@angular/material/form-field';
import { MatInput, MatInputModule } from '@angular/material/input';
import { routes } from '../../../app/app.routes';
import { ActivatedRoute, Router, RouterLink, RouterLinkActive, RouterModule, RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { MatTabLabel, MatTabsModule } from '@angular/material/tabs';
import { MatIcon } from '@angular/material/icon';
import { provideAnimations } from '@angular/platform-browser/animations';
import { MatTable, MatTableModule } from '@angular/material/table';
import { JobTrackerService } from '../../../services/jobtracker.service';
import { response } from 'express';
import { Observable } from 'rxjs';
import { MatSelectModule } from '@angular/material/select';

export interface EmployerProfile {
  id: string;
  date: Date;
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

// const ELEMENT_DATA: EmployerProfile[] = [
//   { id: '1', date: new Date('2024-11-01'), name: 'Acme Corp', title: 'Software Engineer', address: '123 Main St', city: 'Metropolis', state: 'NY', zip: '10001', phone: '555-1234', email: 'contact@acme.com', website: 'https://www.acme.com' },
//   { id: '2', date: new Date('2024-02-01'), name: 'Globex Corporation', title: 'Project Manager', address: '456 Elm St', city: 'Springfield', state: 'IL', zip: '62701', phone: '555-5678', email: 'info@globex.com', website: 'https://www.globex.com' },
//   { id: '3', date: new Date('2024-03-01'), name: 'Initech', title: 'Systems Analyst', address: '789 Oak St', city: 'Capital City', state: 'CA', zip: '90001', phone: '555-8765', email: 'support@initech.com', website: 'https://www.initech.com' },
//   { id: '4', date: new Date('2024-04-01'), name: 'Umbrella Corporation', title: 'Biochemist', address: '101 Pine St', city: 'Raccoon City', state: 'MI', zip: '48001', phone: '555-4321', email: 'research@umbrella.com', website: 'https://www.umbrella.com' },
//   { id: '5', date: new Date('2024-05-01'), name: 'Stark Industries', title: 'Mechanical Engineer', address: '202 Maple St', city: 'New York', state: 'NY', zip: '10002', phone: '555-6789', email: 'engineering@stark.com', website: 'https://www.starkindustries.com' },
//   { id: '6', date: new Date('2024-06-01'), name: 'Wayne Enterprises', title: 'CEO', address: '303 Birch St', city: 'Gotham', state: 'NJ', zip: '07001', phone: '555-9876', email: 'ceo@wayne.com', website: 'https://www.wayneenterprises.com' },
//   { id: '7', date: new Date('2024-07-01'), name: 'Oscorp', title: 'Research Scientist', address: '404 Cedar St', city: 'New York', state: 'NY', zip: '10003', phone: '555-3456', email: 'research@oscorp.com', website: 'https://www.oscorp.com' },
//   { id: '8', date: new Date('2024-08-01'), name: 'LexCorp', title: 'Chief Financial Officer', address: '505 Walnut St', city: 'Metropolis', state: 'NY', zip: '10004', phone: '555-6543', email: 'cfo@lexcorp.com', website: 'https://www.lexcorp.com' }
// ];

@Component({
  selector: 'app-jobprofile',
  standalone: true,
  imports: 
  [
    RouterModule,
    RouterOutlet, 
    CommonModule,
    RouterLink,
    RouterLinkActive,
    MatLabel,
    MatInputModule,
    FormsModule,
    CommonModule,
    MatTabsModule,
    MatTabLabel,
    MatIcon, 
    MatTableModule,
    MatFormFieldModule, 
    MatSelectModule, 
    MatInputModule,
  ],
  providers: 
  [
    //provideAnimations(),
  ],
  templateUrl: './jobprofile.component.html',
  styleUrl: './jobprofile.component.scss'
})

export class JobprofileComponent {
  title = 'JobTrackerApp';
  _username: string = '';
  _userNameId: any;
  _jobProfiles: any;
  _jobProfile: any;
  _jobProfileSelected: any;
  _employerProfile: any;
  displayedColumns: string[] = ['name', 'city', 'state', 'phone', 'email', 'website', 'date'];
  ELEMENT_DATA: EmployerProfile[] = [];
  dataSource: any;
  _isSelected: boolean = false;
  //dataSource = ELEMENT_DATA;
  
  ngOnInit(): void {
    // Retrieve the username from the query parameters
    this.route.queryParams.subscribe(params => {
      this._username = params['username'];
    });
    //_userNameId = this.jobTrackerService.userProfile(username);
    this.dataSource = this.getEmployerProfiles();
    this._jobProfile = this.getJobProfile(this._username);
    this._jobProfiles = this.getJobProfiles(this._username);
  }

  constructor(private router: Router, private route: ActivatedRoute, private jobTrackerService: JobTrackerService) { }

  onCreateJobProfile() {
    const jobProfile = {
      // Define the job profile data structure
      // Add other fields as needed
    };

    this.jobTrackerService.CreateJobProfile(jobProfile).subscribe(response => {
      console.log('Job profile created successfully', response);
      // Handle success response
    },
      error => {
        console.error('Failed to create job profile', error);
        // Handle error response
      }
    );
  }

  public getEmployerProfiles(): void {
    this.jobTrackerService.GetEmployerProfiles(this._jobProfile).subscribe(response => {
      console.log(response);
      this.dataSource = response;
    },
      error => {
        console.error('Failed to get employer profiles', error);
      });
  }

  public getJobProfile(username: string): any {
    this.jobTrackerService.GetJobProfile(this._userNameId, this._jobProfile).subscribe(response => {
      console.log(response);
      this._jobProfile = response;
    },
      error => {
        console.error('Failed to get job profile', error);
      });
  }

  public getJobProfiles(username: string): any {
    this.jobTrackerService.GetJobProfiles().subscribe(response => {
      console.log(response);
      this._jobProfiles = response;
    },
      error => {
        console.error('Failed to get job profiles', error);
      });
    
  }

  public onJobProfileChange(event: any) { 
    this._jobProfileSelected = event.value;
  }

  public download() {
    console.log('Download button clicked!');
  }
}
