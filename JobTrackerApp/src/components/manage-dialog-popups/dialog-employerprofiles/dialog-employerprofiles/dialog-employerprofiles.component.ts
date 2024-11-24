import { Component, Inject } from '@angular/core';
import { JobTrackerService } from '../../../../services/jobtracker.service';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FormGroup, FormBuilder, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from 'express';
import { EmployerProfile, ActionResult, Details } from '../../../../models/employer-profile.model';
import { EmployerprofileComponent } from '../../../manage-employerprofiles/employerprofile/employerprofile.component';
import { CommonModule, NgFor, JsonPipe } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule, MatTabGroup, MatTab } from '@angular/material/tabs';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule, RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-create-employerprofiles',
  standalone: true,
  imports: 
  [
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
  templateUrl: './dialog-employerprofiles.component.html',
  styleUrl: './dialog-employerprofiles.component.scss'
})
export class DialogEmployerprofilesComponent {

  //constructor(private jobTrackerService: JobTrackerService, private dialog: MatDialog) {}

  employerProfileForm!: FormGroup;
  actionForm!: FormGroup;
  detailsForm!: FormGroup;

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<EmployerprofileComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EmployerProfile,
    private jobTrackerService: JobTrackerService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.employerProfileForm = this.formBuilder.group({
      id: [''],
      //date: [{ value: this.data.date, disabled: true }],
      jobProfileId: [''],
      name: ['', Validators.required],
      title: [''],
      address: [''],
      city: [''],
      state: [''],
      zip: [''],
      phone: [''],
      email: [''],
      website: [''],
    });
    this.actionForm = this.formBuilder.group({
      id: [''],
      employerprofileid: [''],
      date: [''],
      latestUpdate: [''],
      action: [''],
      method: [''],
      actionresult: [''],
    });
    this.detailsForm = this.formBuilder.group({
      id: [''],
      employerprofileid: [''],
      date: [''],
      latestUpdate: [''],
      comments: [''],
      updates: [''],
    });
    // this.setEmployerProfileForm();
    // this.setActionForm();
    // this.setDetailsForm();
  }

  public onSubmit(): void {
    // Process checkout data here
    // console.warn(
    //   'Your order has been submitted',
    //   this.employerProfileForm.value
    // );
    if (this.employerProfileForm.valid) {
      const employerProfile = this.employerProfileForm.value;
      this.dialogRef.close(employerProfile); // Close the dialog and return the data
    }

    // this.jobTrackerService.UpdateEmployerProfile(this.employerProfileForm.value).subscribe(
    //   (response) => {
    //     console.log('Employer Profile Updated');
    //     //update the employer profile?
    //   },
    //   (error) => {
    //     console.log('Error updating Employer Profile');
    //   }
    // );
  }

  public onClose(): void {
    
    this.dialogRef.close();
    //this.location.back();
    //this.router.navigate(['/jobprofile']);
  }

  public setActionForm(): void {
    //var actionData = this.jobTrackerService.GetJobAction(this.employerProfileForm.value.id);
    this.jobTrackerService.GetJobAction(this.employerProfileForm.value.id).subscribe((data: ActionResult) => {
      console.log('Action Data', data.actionResult);
      
      this.actionForm = this.formBuilder.group({
        id: [data.id],
        employerprofileid: [data.employerProfileId],
        date: [data.date],
        latestUpdate: [data.latestUpdate],
        action: [data.action],
        method: [data.method],
        actionresult: [data.actionResult],

      });
    },
    (error) => {
      console.error('Error getting job action', error);
    });
  }

  public setDetailsForm(): void {
    this.jobTrackerService.GetDetail(this.employerProfileForm.value.id).subscribe((data: Details) => {

      this.detailsForm = this.formBuilder.group({
        id: [data.id],
        employerprofileid: [data.employerProfileId],
        date: [data.date],
        latestUpdate: [data.latestUpdate],
        comments: [data.comments],
        updates: [data.updates],

      });
    },
    (error) => {
      console.error('Error getting details', error);
    });
  }
  
  public setEmployerProfileForm(): void {
    this.employerProfileForm = this.formBuilder.group({
      id: [this.data.id],
      date: [{ value: this.data.date, disabled: true }],
      jobProfileId: [this.data.jobProfileId],
      name: [this.data.name, Validators.required],
      title: [this.data.title],
      address: [this.data.address],
      city: [this.data.city],
      state: [this.data.state],
      zip: [this.data.zip],
      phone: [this.data.phone],
      email: [this.data.email],
      website: [this.data.website],
      jobaction: [this.data.jobAction],
      details: [this.data.details],
    });
  }

  public onTabChange(event: any) {
    if (event.index == 0) {
      this.setEmployerProfileForm();
      this.saveEmployerProfile();
    } else if (event.index == 1) {
      this.setActionForm();
      this.saveEmployerAction();
    } else {
      this.setDetailsForm();
      this.saveEmployerDetail();
    }
  }

  private saveEmployerProfile(): void {
    this.jobTrackerService.UpdateEmployerProfile(this.employerProfileForm.value).subscribe(
      (response) => {
        console.log('Employer Profile Updated');
      },
      (error) => {
        console.log('Error updating Employer Profile');
      }
    );
  }

  public saveEmployerAction(): void {
    this.jobTrackerService.UpdateEmployerProfile(this.actionForm.value).subscribe(
      (response) => {
        console.log('Employer Action Updated');
      },
      (error) => {
        console.log('Error updating Employer Action');
      }
    );
  }

  public saveEmployerDetail(): void {
    this.jobTrackerService.UpdateEmployerProfile(this.detailsForm.value).subscribe(
      (response) => {
        console.log('Employer Detail Updated');
      },
      (error) => {
        console.log('Error updating Employer Detail');
      }
    );
  }

  public EmployerProfileDialog(element: any): void {
    // Open the dialog
    const dialogRef = this.dialog.open(DialogEmployerprofilesComponent, {
      width: '500px',
      height: '800px',
      data: element,
    });

    dialogRef.afterClosed().subscribe((result: DialogEmployerprofilesComponent) => {
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
}
