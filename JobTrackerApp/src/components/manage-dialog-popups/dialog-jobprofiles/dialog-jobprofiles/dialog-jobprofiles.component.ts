import { Component, EventEmitter, Inject, OnInit, Output } from '@angular/core';
import { JobTrackerService } from '../../../../services/jobtracker.service-old';
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
import { NotificationService } from '../../../../services/notifications/notification.service';

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
  isEditMode: boolean = false;
  currentProfileName: string = '';
  
  constructor(
    private jobTrackerService: JobTrackerService,
    public dialog: MatDialog,
    private dialogRef: MatDialogRef<DialogJobprofilesComponent>,
    private formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: JobProfile,
    private snackBar: MatSnackBar,
    private notificationService: NotificationService
  ) { }

  ngOnInit(): void {
    this.notificationService.dialogClose$.subscribe(() => {
      this.dialogRef.close();
    });

    this.jobProfileForm = this.formBuilder.group({
      userProfileId: [this.data.userProfileId],
      profileName: ['', Validators.required],
    });
  }

  public onSubmit(): void {
    if (this.jobProfileForm.valid) {
      const jobProfile = this.jobProfileForm.value;

      this.jobTrackerService.CreateJobProfile(jobProfile).subscribe(
        (response) => {

          console.log('Job profile created successfully', response);
          this.notificationService.CreateJobProfile('Job profile created successfully', 5000);
          this.dialogRef.close();
        },
        (error) => {

          console.error('Failed to create job profile', error);
          this.notificationService.CreateJobProfile('Failed to create job profile', 5000);
        }
      );

      //this.dialogRef.close(jobProfile);
    }
  }

  public onClose(): void {
    this.notificationService.ChangesWillBeLost('Changes will be lost. Close without saving?', 5000);
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
