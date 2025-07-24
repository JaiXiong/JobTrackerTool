import { CommonModule } from '@angular/common';
import { Component, EventEmitter, input, Output } from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  FormGroup,
  FormBuilder,
  Validators,
  Form,
} from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule, MatTabGroup, MatTab } from '@angular/material/tabs';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule } from '@angular/router';
import { ClosebuttonModularComponent } from '../../modular/closebutton-modular/closebutton-modular.component';
import { SubmitbuttonModularComponent } from '../../modular/submitbutton-modular/submitbutton-modular.component';
import { BackbuttonModularComponent } from '../../modular/backbutton-modular/backbutton-modular.component';
import { VerificationComponent } from '../verification/verification.component';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-registeruser',
  standalone: true,
  imports: [
    RouterModule,
    CommonModule,
    FormsModule,
    MatTabsModule,
    MatInputModule,
    MatIconModule,
    MatTooltipModule,
    ReactiveFormsModule,
    SubmitbuttonModularComponent,
    BackbuttonModularComponent,
    MatSnackBarModule,
    //VerificationComponent
],
  templateUrl: './registeruser.component.html',
  styleUrl: './registeruser.component.scss',
})
export class RegisteruserComponent {
  //userProfileForm!: FormGroup;
  _registerUser: FormGroup;
  _email: string = '';
  _confirmEmail: string = '';
  _password: string = '';
  _confirmPassword: string = '';
  @Output() registerComplete = new EventEmitter<void>();
  @Output() backClicked = new EventEmitter<void>();

  constructor(private formBuilder: FormBuilder, private snackBar: MatSnackBar) {
    this._registerUser = this.formBuilder.group({
      email: ['', Validators.required],
      confirmEmail: ['', Validators.required],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required],
    });
  }

  ngOnInit(): void {
  }

  public onSubmitRegisterUser(): void {
    
    this._registerUser.value.email = this._email;
    this._registerUser.value.confirmEmail = this._confirmEmail;
    this._registerUser.value.password = this._password;
    this._registerUser.value.confirmPassword = this._confirmPassword;

    this.showMessage('Check your Email!');
  }

  private showMessage(message: string, action: string = 'Close', duration: number = 10000): void {
    this.snackBar.open(message, action, {
      duration: duration,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
    });
  }

  public onBackClicked(): void {
    this.backClicked.emit();
  }

  handleCloseRegister() {
    this.registerComplete.emit();
  }
}
