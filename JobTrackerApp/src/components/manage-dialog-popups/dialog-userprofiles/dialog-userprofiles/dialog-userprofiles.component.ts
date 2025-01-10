import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { JobTrackerService } from '../../../../services/jobtracker.service-old';
import { UserprofileComponent } from '../../../manage-users/userprofile/userprofile.component';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule, MatTabGroup, MatTab } from '@angular/material/tabs';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule } from '@angular/router';
import { UserProfile } from '../../../../models/user-profile.model';

@Component({
  selector: 'app-dialog-userprofiles',
  standalone: true,
  imports: [
    RouterModule,
    CommonModule,
    FormsModule,
    MatTabsModule,
    MatInputModule,
    MatTabGroup,
    MatTab,
    MatIconModule,
    MatTooltipModule,
    ReactiveFormsModule,
  ],
  templateUrl: './dialog-userprofiles.component.html',
  styleUrl: './dialog-userprofiles.component.scss',
})
export class DialogUserprofilesComponent {
  userProfileForm!: FormGroup;
  constructor(
    private jobTrackerService: JobTrackerService,
    private dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: UserProfile,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    this.userProfileForm = this.formBuilder.group({
      // id: [this.data.id],
      // date: [{ value: this.data.date, disabled: true }],
      // jobProfile: [this.data.jobProfiles],
      // name: [this.data.name],
      // email: [this.data.email],
      // phone: [this.data.phone],
      // address: [this.data.address],
      // city: [this.data.city],
      // state: [this.data.state],
      id: [''],
      //date: [{ value: this.data.date, disabled: true }],
      jobProfile: [''],
      name: [''],
      email: [''],
      phone: [''],
      address: [''],
      city: [''],
      state: [''],
    });
  }

  onSubmit(): void {
    this.jobTrackerService.CreateUserProfile(this.userProfileForm.value).subscribe(
      (result) => {
        this.dialog.closeAll();
      },
      (error) => {
        console.error(error);
      }
    );
  }

  onClose(): void {
    this.dialog.closeAll();
  }

  public onTabChange(event: any) {
    if (event.index == 0) {
    } else if (event.index == 1) {
    } else {
    }
  }
}
