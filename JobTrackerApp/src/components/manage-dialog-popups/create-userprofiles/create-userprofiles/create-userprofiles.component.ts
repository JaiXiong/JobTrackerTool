import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { JobTrackerService } from '../../../../services/jobtracker.service';
import { UserprofileComponent } from '../../../manage-users/userprofile/userprofile.component';

@Component({
  selector: 'app-create-userprofiles',
  standalone: true,
  imports: 
  [

  ],
  templateUrl: './create-userprofiles.component.html',
  styleUrl: './create-userprofiles.component.scss'
})
export class CreateUserprofilesComponent {

  constructor(private jobTrackerService: JobTrackerService, private dialog: MatDialog) {}

  public UserCreationDialog(): void {
    // Open the dialog
    const dialogRef = this.dialog.open(CreateUserprofilesComponent, {
      width: '500px',
      height: '800px',
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.jobTrackerService.CreateUser(result).subscribe(
          (response) => {
            console.log('User created successfully', response);
            // Handle success response
          },
          (error) => {
            console.error('Failed to create user', error);
            // Handle error response
          }
        );
      }
    });
  }
}
