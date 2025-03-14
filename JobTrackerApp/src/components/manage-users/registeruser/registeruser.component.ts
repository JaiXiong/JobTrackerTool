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
  ],
  templateUrl: './registeruser.component.html',
  styleUrl: './registeruser.component.scss',
})
export class RegisteruserComponent {
  //userProfileForm!: FormGroup;
  _registerUser: FormGroup;
  _email: string = '';
  _password: string = '';
  _confirmPassword: string = '';
  @Output() registerComplete = new EventEmitter<void>();

  constructor(private formBuilder: FormBuilder) {
    this._registerUser = this.formBuilder.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required],
    });
  }

  ngOnInit(): void {
  }

  public onSubmitRegisterUser(): void {
    
    this._registerUser.value.email = this._email;
    this._registerUser.value.password = this._password;
    this._registerUser.value.confirmPassword = this._confirmPassword;
  }

  handleCloseRegister() {
    this.registerComplete.emit();
  }
}
