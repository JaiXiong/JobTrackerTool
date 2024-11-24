import { Component, Inject, OnInit } from '@angular/core';
import { JobTrackerService } from '../../../../services/jobtracker.service';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
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

@Component({
  selector: 'app-create-jobprofiles',
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
  ],
  templateUrl: './dialog-jobprofiles.component.html',
  styleUrl: './dialog-jobprofiles.component.scss',
})
export class DialogJobprofilesComponent implements OnInit {
  jobProfileForm!: FormGroup;
  
  constructor(
    private jobTrackerService: JobTrackerService,
    public dialog: MatDialog,
    private formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: JobProfile
  ) {}

  ngOnInit(): void {
    this.jobProfileForm = this.formBuilder.group({
      id: [this.data.id],
      date: [{ value: this.data.date, disabled: true }],
      userProfileId: [this.data.userProfileId],
      profileName: [this.data.profileName],
    });
  }

  onSubmit(): void {
    this.jobTrackerService
      .CreateJobProfile(this.jobProfileForm.value)
      .subscribe(
        (response) => {
          console.log('Job Profile created successfully', response);
          // Handle success response
        },
        (error) => {
          console.error('Failed to create job profile', error);
          // Handle error response
        }
      );
  }

  onClose(): void {
    this.dialog.closeAll();
  }
}
