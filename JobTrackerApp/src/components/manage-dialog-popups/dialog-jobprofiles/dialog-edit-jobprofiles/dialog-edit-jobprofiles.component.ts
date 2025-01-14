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
import { JobProfile } from '../../../../models/job-profile.model';
import { JobTrackerService } from '../../../../services/jobtracker.service-old';
import { DialogJobprofilesComponent } from '../dialog-jobprofiles/dialog-jobprofiles.component';
import { NotificationService } from '../../../../services/notifications/notification.service';

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
    private snackBar: MatSnackBar,
    private notificationService: NotificationService
  ) { }

  ngOnInit(): void {
    this.notificationService.dialogClose$.subscribe(() => {
      this.dialogRef.close();
    });

    this.jobProfileForm = this.formBuilder.group({
       id: [this.data.id],
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

      this.jobTrackerService.UpdateJobProfile(jobProfile).subscribe(
        (response) => {
          this.notificationService.showNotification('Job profile updated successfully', 5000);
          this.dialogRef.close();
        },
        (error) => {

          console.error('Failed to create job profile', error);
          this.notificationService.showNotification('Failed to updated job profile', 5000);
        }
      );

      //this.dialogRef.close(jobProfile);
    }
  }

  public onClose(): void {
    this.notificationService.showNotification('Changes will be lost. Close without saving?', 5000);
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
