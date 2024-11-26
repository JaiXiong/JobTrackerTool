import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { MatTabsModule, MatTab } from '@angular/material/tabs';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule } from '@angular/router';
import { JobProfile } from '../../../../../models/job-profile.model';
import { JobTrackerService } from '../../../../../services/jobtracker.service';
import { DialogJobprofilesComponent } from '../../dialog-jobprofiles/dialog-jobprofiles.component';

@Component({
  selector: 'app-dialog-edit-jobprofiles',
  standalone: true,
  imports: 
  [
    RouterModule,
    CommonModule,
    FormsModule,
    MatTabsModule,
    MatInputModule,
    MatTab,
    MatIconModule,
    MatTooltipModule,
    ReactiveFormsModule,
    MatSnackBarModule
  ],
  templateUrl: './dialog-edit-jobprofiles.component.html',
  styleUrl: './dialog-edit-jobprofiles.component.scss'
})
export class DialogEditJobprofilesComponent {
  jobProfileForm!: FormGroup;
  currentProfileName: string = '';
  
  constructor(
    private jobTrackerService: JobTrackerService,
    public dialog: MatDialog,
    private dialogRef: MatDialogRef<DialogEditJobprofilesComponent>,
    private formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: JobProfile,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.jobProfileForm = this.formBuilder.group({
      // id: [this.data.id],
      // date: [{ value: this.data.date, disabled: true }],
      // userProfileId: [this.data.userProfileId],
      // profileName: [this.data.profileName],
      //id: [''],
      //date: [{ value: this.data.date, disabled: true }],
      userProfileId: [this.data.userProfileId],
      profileName: [this.data.profileName, Validators.required],
    });
  }

  public onSubmit(): void {
    if (this.jobProfileForm.valid) {
      const jobProfile = this.jobProfileForm.value;

      this.jobTrackerService.CreateJobProfile(jobProfile).subscribe(
        (response) => {

          console.log('Job profile created successfully', response);
          this.snackBar.open('Job profile created successfully', 'Close', {
            duration: 5000,
            horizontalPosition: 'right', // Set horizontal position
            verticalPosition: 'top', // Set vertical position
          });
          this.dialogRef.close();
        },
        (error) => {

          console.error('Failed to create job profile', error);
          this.snackBar.open('Failed to create job profile', 'Close', {
            duration: 5000,
            horizontalPosition: 'right', // Set horizontal position
            verticalPosition: 'top', // Set vertical position
          });
        }
      );

      //this.dialogRef.close(jobProfile);
    }
  }

  public onClose(): void {
    const snackBarRef = this.snackBar.open('Changes will be lost. Close without saving?', 'Cancel', {
      duration: 5000,
      horizontalPosition: 'right', // Set horizontal position
      verticalPosition: 'top', // Set vertical position
    });

    snackBarRef.onAction().subscribe(() => {
      console.log('Close action canceled');
      // Do nothing, just close the snackbar
    });

    snackBarRef.afterDismissed().subscribe((info) => {
      if (!info.dismissedByAction) {
        this.dialogRef.close();
      }
    });
  }

  public onCancel(): void {
    this.dialogRef.close();
  }

  public onTabChange(event: any) {
    if (event.index == 0) {
    } else if (event.index == 1) {
    } else {
    }
  }
}
