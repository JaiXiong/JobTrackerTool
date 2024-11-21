import { Location, CommonModule, JsonPipe, NgFor } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  InjectionToken,
  OnInit,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule, MatTabGroup, MatTab } from '@angular/material/tabs';
import { MatTooltipModule } from '@angular/material/tooltip';
import {
  RouterModule,
  RouterOutlet,
  RouterLink,
  RouterLinkActive,
  Router
} from '@angular/router';
import { JobTrackerService } from '../../../services/jobtracker.service';
import { EmployerProfile } from '../../../models/employer-profile.model';

@Component({
  selector: 'app-employerprofile',
  standalone: true,
  imports: [
    RouterModule,
    RouterOutlet,
    CommonModule,
    RouterLink,
    RouterLinkActive,
    FormsModule,
    MatTabsModule,
    MatInputModule,
    MatTabGroup,
    MatTab,
    MatIconModule,
    MatTooltipModule,
    ReactiveFormsModule,
    NgFor,
    JsonPipe,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './employerprofile.component.html',
  styleUrl: './employerprofile.component.scss',
})
export class EmployerprofileComponent implements OnInit {
  employerProfileForm!: FormGroup;
  actionForm!: FormGroup;
  detailsForm!: FormGroup;

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<EmployerprofileComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EmployerProfile,
    private location: Location,
    private jobTrackerService: JobTrackerService
  ) {}

  ngOnInit(): void {
    // this.employerProfileForm = this.formBuilder.group({
    //   id: [this.data.id],
    //   date: [{ value: this.data.date, disabled: true }],
    //   jobProfileId: [this.data.jobProfileId],
    //   name: [this.data.name, Validators.required],
    //   title: [this.data.title],
    //   address: [this.data.address],
    //   city: [this.data.city],
    //   state: [this.data.state],
    //   zip: [this.data.zip],
    //   phone: [this.data.phone],
    //   email: [this.data.email],
    //   website: [this.data.website],
    // });
    this.employerProfileForm = this.setEmployerProfileForm();
    this.actionForm = this.setActionForm();
    this.detailsForm = this.setDetailsForm();
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

  public setActionForm(): FormGroup {
    var actionData = this.jobTrackerService.GetJobAction(this.employerProfileForm.value.id);
    return this.formBuilder.group({
      id: [''],
      employerprofileid: [''],
      date: [''],
      latestUpdate: [''],
      action: [''],
      method: [''],
      actionresult: [''],
    });
  }

  public setDetailsForm(): FormGroup {
    var detailsData = this.jobTrackerService.GetDetail(this.employerProfileForm.value.id);
    return this.formBuilder.group({
      id: [''],
      employerprofileid: [''],
      date: [''],
      latestUpdate: [''],
      comments: [''],
      updates: [''],
    });
  }
  
  public setEmployerProfileForm(): FormGroup {
    return this.formBuilder.group({
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
      jobaction: [this.data.jobaction],
      details: [this.data.details],
    });
  }

  public onTabChange(event: any) {
    if (event.index == 1) {
      this.employerProfileForm = this.setEmployerProfileForm();
      
    } else if (event.index == 2) {
      this.actionForm = this.setActionForm();
    } else {
      this.detailsForm = this.setDetailsForm();
    }

  }
}
// function inject(
//   MAT_DIALOG_DATA: InjectionToken<any>
// ): (
//   target: typeof EmployerprofileComponent,
//   propertyKey: undefined,
//   parameterIndex: 2
// ) => void {
//   throw new Error('Function not implemented.');
// }
