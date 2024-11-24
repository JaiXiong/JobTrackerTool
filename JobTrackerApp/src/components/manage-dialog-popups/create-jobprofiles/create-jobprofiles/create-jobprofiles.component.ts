import { Component } from '@angular/core';
import { JobTrackerService } from '../../../../services/jobtracker.service';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-create-jobprofiles',
  standalone: true,
  imports: 
  [

  ],
  templateUrl: './create-jobprofiles.component.html',
  styleUrl: './create-jobprofiles.component.scss'
})
export class CreateJobprofilesComponent {

  constructor(private jobTrackerService: JobTrackerService, private dialog: MatDialog) {}

  public CreateJobProfileDialog(): void {
    // Open the dialog
    const dialogRef = this.dialog.open(CreateJobprofilesComponent, {
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
}
