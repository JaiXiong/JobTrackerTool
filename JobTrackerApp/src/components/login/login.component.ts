import { ChangeDetectionStrategy, Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS, MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule, provideAnimations } from '@angular/platform-browser/animations';
import { Router, RouterLink, RouterLinkActive, RouterModule, RouterOutlet } from '@angular/router';
import { error } from 'console';
import { response } from 'express';
import { LoginService } from './../../services/login.service';
import { CommonModule } from '@angular/common';
import { UserProfile } from '../../models/user-profile.model';
import { MatDialog } from '@angular/material/dialog';
import { DialogUserprofilesComponent } from '../manage-dialog-popups/dialog-userprofiles/dialog-userprofiles/dialog-userprofiles.component';
import { PopupformModularComponent } from "../modular/popupform/popupform-modular/popupform-modular.component";
import { RegisteruserComponent } from "../manage-users/registeruser/registeruser.component";

@Component({
  selector: 'app-login',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    RouterModule,
    CommonModule,
    ReactiveFormsModule,
    MatIconModule,
    FormsModule,
    RegisteruserComponent
],
  providers:
    [
      //provideAnimations(),
      //{ provide: MAT_FORM_FIELD_DEFAULT_OPTIONS, useValue: { appearance: 'outline' } }
    ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  _username: string = '';
  _password: string = '';
  _usernameid: string = '';
  _isRegister: boolean = false;

  constructor(private router: Router, private loginService: LoginService, private dialog: MatDialog) { }

  public login(): void {
    console.log('Login button clicked!');

    this.loginService.Login(this._username, this._password).subscribe(
      response => {
        this.router.navigate(['/jobprofile'], { queryParams: { usernameid: response.id, name: response.name } });
      },
      error => {
        console.error('Login failed', error);
      });
  }

  public registerPopup(event: Event): void {
    event.preventDefault();
    this._isRegister = !this._isRegister;
  }

  
  public register(element: UserProfile): void {
    const dialogRef = this.dialog.open(DialogUserprofilesComponent, {
      width: '250px',
      data: element
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  handleClose() {
    this._isRegister = false;
  }
}
