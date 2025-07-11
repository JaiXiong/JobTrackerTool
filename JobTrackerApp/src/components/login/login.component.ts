import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { UserProfile } from '../../models/user-profile.model';
import { MatDialog } from '@angular/material/dialog';
import { DialogUserprofilesComponent } from '../manage-dialog-popups/dialog-userprofiles/dialog-userprofiles/dialog-userprofiles.component';
import { RegisteruserComponent } from "../manage-users/registeruser/registeruser.component";
import { LoginService } from '../../services/login/login.service';
import { AuthService } from '../../services/auth/auth.service';
import { tap } from 'rxjs';

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
    RegisteruserComponent,
  ],
  providers: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  _isRegister: boolean = false;
  _currenUser: any;
  //private isUpdatingValidity = false;

  constructor(
    private router: Router,
    private loginService: LoginService,
    private dialog: MatDialog,
    private authService: AuthService,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(2)]],
    });

    //this.checkFormValidity();
  }

  ngAfterViewInit() {
    if (this.username?.value && this.password?.value) {
      this.loginForm.updateValueAndValidity();
      this.loginForm.markAsTouched();
    }
  }

  get username() {
    return this.loginForm.get('username');
  }

  get password() {
    return this.loginForm.get('password');
  }

  // checkFormValidity() {
  //   if (this.isUpdatingValidity) {
  //     return;
  //   }

  //   this.isUpdatingValidity = true;
    
  //   if (this.username?.value && this.password?.value) {
  //     this.loginForm.updateValueAndValidity();
  //   }
  //   this.isUpdatingValidity = false;
  // }

  public login(): void {
    // this.loginService.Login(this._username, this._password).subscribe({
    //   next: (response) => {
    //    this.router.navigate(['/jobprofile'], { queryParams: { usernameid: response.id, name: response.name } });
    // },
    //   error: (error) => {
    //     console.error('Login failed', error);
    //   },
    // });

    this.loginService
      .Login(
        this.loginForm.get('username')?.value,
        this.loginForm.get('password')?.value
      )
      .pipe(
        tap({
          next: (tokens) => {
            //console.log('Response received:', tokens);
            this.authService.doLoginUser(
              this.loginForm.get('username')?.value,
              tokens.access_token,
              tokens.refresh_token
            );
            this.router.navigate(['/jobprofile'], {
              queryParams: { usernameid: tokens.id, name: tokens.name },
            });
          },
          complete: () => {
            //console.log('Request completed');
          },
          error: (error) => {
            console.log('Error received:', error);
          },
        })
      )
      .subscribe();
  }

  public registerPopup(event: Event): void {
    event.preventDefault();
    this._isRegister = !this._isRegister;
  }

  public register(element: UserProfile): void {
    const dialogRef = this.dialog.open(DialogUserprofilesComponent, {
      width: '250px',
      data: element,
    });

    dialogRef.afterClosed().subscribe((result) => {
      console.log('The dialog was closed');
    });
  }

  handleClose() {
    this._isRegister = false;
  }

  onRegisterComplete() {
    this._isRegister = false;
  }
}
