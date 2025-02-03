import { Component } from '@angular/core';
import { JobTrackerService } from '../../../services/jobtracker.service-old';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { EmployerprofileComponent } from '../../manage-employerprofiles/employerprofile/employerprofile.component';
import { EmployerProfile } from '../../../models/employer-profile.model';
import { UserprofileComponent } from '../../manage-users/userprofile/userprofile.component';

@Component({
  selector: 'app-dialogpopups',
  standalone: true,
  imports: 
  [
    MatDialogModule,
  ],
  templateUrl: './dialogpopups.component.html',
  styleUrl: './dialogpopups.component.scss'
})
export class DialogpopupsComponent {
uploadProgress: boolean = false;

  constructor(private jobTrackerService: JobTrackerService, private dialog: MatDialog) {}

  public CreateJobProfileDialog(): void {
    // Open the dialog
    const dialogRef = this.dialog.open(EmployerprofileComponent, {
      width: '500px',
      height: '800px',
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.jobTrackerService.CreateJobProfile(result).subscribe(
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
    });
  }

  public EmployerProfileDialog(element: any): void {
    // Open the dialog
    const dialogRef = this.dialog.open(EmployerprofileComponent, {
      width: '500px',
      height: '800px',
      data: element,
    });

    dialogRef.afterClosed().subscribe((result: EmployerProfile) => {
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

  public UserCreationDialog(): void {
    // Open the dialog
    const dialogRef = this.dialog.open(UserprofileComponent, {
      width: '500px',
      height: '800px',
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        // this.jobTrackerService.CreateUser(result).subscribe(
        //   (response) => {
        //     console.log('User created successfully', response);
        //     // Handle success response
        //   },
        //   (error) => {
        //     console.error('Failed to create user', error);
        //     // Handle error response
        //   }
        // );
      }
    });
  }
}
