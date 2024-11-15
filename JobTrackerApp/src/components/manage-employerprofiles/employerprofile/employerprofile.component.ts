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

export interface EmployerProfile {
  id: string;

  name: string;
  title: string;
  address: string;
  city: string;
  state: string;
  zip: string;
  phone: string;
  email: string;
  website: string;
}

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

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<EmployerprofileComponent>,
    @Inject(MAT_DIALOG_DATA) public data: MatDialog,
    private location: Location,
    private jobTrackerService: JobTrackerService
  ) {}

  ngOnInit(): void {
    this.employerProfileForm = this.formBuilder.group({
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
  }

  onSubmit(): void {
    // Process checkout data here
    console.warn(
      'Your order has been submitted',
      this.employerProfileForm.value
    );

    this.jobTrackerService.UpdateEmployerProfile(this.employerProfileForm.value).subscribe(
      (response) => {
        console.log('Employer Profile Updated');
        //update the employer profile?
      },
      (error) => {
        console.log('Error updating Employer Profile');
      }
    );
  }

  onClose(): void {
    this.dialogRef.close();
    //this.location.back();
    //this.router.navigate(['/jobprofile']);
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
