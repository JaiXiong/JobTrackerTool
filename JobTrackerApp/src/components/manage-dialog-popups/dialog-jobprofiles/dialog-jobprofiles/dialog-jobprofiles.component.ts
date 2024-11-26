import { Component, Inject, OnInit } from '@angular/core';
import { JobTrackerService } from '../../../../services/jobtracker.service';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { JobProfile } from '../../../../models/job-profile.model';
import {
  RouterModule,
} from '@angular/router';
import { CommonModule  } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule, MatTabGroup, MatTab } from '@angular/material/tabs';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-dialog-jobprofiles',
  standalone: true,
  imports: [
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
  templateUrl: './dialog-jobprofiles.component.html',
  styleUrl: './dialog-jobprofiles.component.scss',
})
export class DialogJobprofilesComponent implements OnInit {
  jobProfileForm!: FormGroup;
  
  constructor(
    private jobTrackerService: JobTrackerService,
    public dialog: MatDialog,
    private dialogRef: MatDialogRef<DialogJobprofilesComponent>,
    private formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: JobProfile,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.jobProfileForm = this.formBuilder.group({
      // id: [this.data.id],
      // date: [{ value: this.data.date, disabled: true }],
      // userProfileId: [this.data.userProfileId],
      // profileName: [this.data.profileName],
      //id: [''],
      //date: [{ value: this.data.date, disabled: true }],
      userProfileId: [this.data.userProfileId],
      profileName: ['', Validators.required],
    });
  }

  onSubmit(): void {
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

  onClose(): void {
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

  public onTabChange(event: any) {
    if (event.index == 0) {
    } else if (event.index == 1) {
    } else {
    }
  }
}
