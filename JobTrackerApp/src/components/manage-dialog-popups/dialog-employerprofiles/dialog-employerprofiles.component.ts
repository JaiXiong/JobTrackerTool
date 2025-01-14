import { Component, Inject, OnInit } from '@angular/core';
import { JobTrackerService } from '../../../services/jobtracker.service-old';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FormGroup, FormBuilder, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EmployerProfile, ActionResult, Details } from '../../../models/employer-profile.model';
import { CommonModule} from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule, MatTabGroup, MatTab } from '@angular/material/tabs';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Router, RouterModule} from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { CustomEmployersnackbarComponent } from '../../custom-components/custom-employer-snackbar/custom-employersnackbar/custom-employersnackbar.component';
import { NotificationService } from '../../../services/notifications/notification.service';

@Component({
  selector: 'app-dialog-employerprofiles',
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
    MatSnackBarModule
  ],
  templateUrl: './dialog-employerprofiles.component.html',
  styleUrl: './dialog-employerprofiles.component.scss'
})
export class DialogEmployerprofilesComponent implements OnInit {
  employerProfileForm!: FormGroup;
  actionForm!: FormGroup;
  detailsForm!: FormGroup;
  selectedTabIndex: number = 0;
  employerTab: boolean = false;
  actionTab: boolean = false;
  detailsTab: boolean = false;

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<DialogEmployerprofilesComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EmployerProfile,
    private jobTrackerService: JobTrackerService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.actionForm = this.formBuilder.group({
      //id: [''],
      //employerprofileid: [''],
      //date: [''],
      //latestUpdate: [''],
      action: ['', Validators.required],
      method: [''],
      actionresult: ['',],
    });
    this.detailsForm = this.formBuilder.group({
      //id: [''],
      //employerprofileid: [''],
      //date: [''],
      //latestUpdate: [''],
      comments: ['', Validators.required],
      updates: [''],
    });

    this.employerProfileForm = this.formBuilder.group({
      //id: [''],
      //date: [{ value: this.data.date, disabled: true }],
      //latestUpdate: [''],
      jobProfileId: [this.data.jobProfileId],
      name: ['', Validators.required],
      title: [''],
      address: [''],
      city: [''],
      state: [''],
      zip: [''],
      phone: [''],
      email: [''],
      website: [''],
      result: this.actionForm,
      detail: this.detailsForm
    });
  }

  public onSubmit(): void {
    if (this.employerProfileForm.valid) {
      const employerProfile = this.employerProfileForm.value;
      //this.dialogRef.close(employerProfile); // Close the dialog and return the data

      this.jobTrackerService.CreateEmployerProfile(employerProfile).subscribe({
        next: (response) => {

          console.log('Employer Profile created successfully', response);
          
          this.notificationService.showNotification('Employer Profile created successfully', 5000);
          this.dialogRef.close();
        },
        error: (error) => {
          console.error('Failed to create Employer Profile', error);
          this.notificationService.showNotification('Failed to create Employer Profile', 5000);
        }
      });
    }
  }

  public onActionSubmit(): void {
    if (this.actionForm.valid) {
      const action = this.actionForm.value;
      //this.dialogRef.close(action); // Close the dialog and return the data

      this.jobTrackerService.CreateEmployerAction(action).subscribe({
        next: (response) => {
          console.log('Employer Action created successfully', response);
          this.notificationService.showNotification('Employer Action created successfully', 5000);
        },
        error: (error) => {
          console.error('Failed to create Employer Action', error);
          this.notificationService.showNotification('Failed to create Employer Action', 5000);
        }
      });
    }
  }

  public onDetailsSubmit(): void {
    if (this.detailsForm.valid) {
      const details = this.detailsForm.value;
      //this.dialogRef.close(details); // Close the dialog and return the data

      this.jobTrackerService.CreateEmployerDetails(details).subscribe({
        next: (response) => {
          console.log('Employer Details created successfully', response);
          this.notificationService.showNotification('Employer Details created successfully', 5000);
        },
        error: (error) => {
          console.error('Failed to create Employer Details', error);
          this.notificationService.showNotification('Failed to create Employer Details', 5000);
        }
      });
    }
  }

  onClose(): void {
    this.openSnackBar(this.selectedTabIndex);
  }

  public onCancel(): void {
    this.openSnackBar(0);
    this.dialogRef.close();
  }

  public isFormValid(): boolean {
    if (this.employerProfileForm.value.name != '' && this.actionForm.value.action != '' && this.detailsForm.value.comments != '') {
      return true;
    }
    return false;
  }

  public openSnackBar(index: number): void {
    const snackBarRef = this.snackBar.openFromComponent(CustomEmployersnackbarComponent, {
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });

    snackBarRef.onAction().subscribe(() => {
      this.handleSaveClick(index);
    });
  }

  public handleSaveClick(Tabindex: number): void {
    if (Tabindex == 0) {
      this.disableAllTabs();
      //his.saveEmployerProfile();
      this.enableAllTabs();
      // this.employerTab = false;
      // this.actionTab = true;
      // this.detailsTab = true;
    }
    if (Tabindex == 1) {
      this.disableAllTabs();
      //this.saveEmployerAction();
      this.enableAllTabs();
      // this.employerTab = true;
      // this.actionTab = false;
      // this.detailsTab = true;
    }
    else {
      this.disableAllTabs();
      //this.saveEmployerDetail();
      this.enableAllTabs();
      // this.employerTab = true;
      // this.actionTab = true;
      // this.detailsTab = false;
    }
  }

  public onTabChange(event: any) {
    this.selectedTabIndex = event.index;
    if (event.index == 0) {
      this.selectedTabIndex = 0;
    } else if (event.index == 1) {
      this.selectedTabIndex = 1;
    } else {
      this.selectedTabIndex = 2;
    }
  }

  // private saveEmployerProfile(): void {
  //   this.jobTrackerService.UpdateEmployerProfile(this.employerProfileForm.value).subscribe(
  //     (response) => {
  //       console.log('Employer Profile Updated');
  //     },
  //     (error) => {
  //       console.log('Error updating Employer Profile');
  //     }
  //   );
  // }

  // public saveEmployerAction(): void {
  //   this.jobTrackerService.UpdateEmployerProfile(this.actionForm.value).subscribe(
  //     (response) => {
  //       console.log('Employer Action Updated');
  //     },
  //     (error) => {
  //       console.log('Error updating Employer Action');
  //     }
  //   );
  // }

  // public saveEmployerDetail(): void {
  //   this.jobTrackerService.UpdateEmployerProfile(this.detailsForm.value).subscribe(
  //     (response) => {
  //       console.log('Employer Detail Updated');
  //     },
  //     (error) => {
  //       console.log('Error updating Employer Detail');
  //     }
  //   );
  // }

  private disableAllTabs(): void {
    this.employerTab = true;
    this.actionTab = true;
    this.detailsTab = true;
  }

  private enableAllTabs(): void {
    this.employerTab = false;
    this.actionTab = false;
    this.detailsTab = false;
  }

}
