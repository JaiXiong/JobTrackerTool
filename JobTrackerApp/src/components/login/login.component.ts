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

@Component({
  selector: 'app-login',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports:
    [
      MatFormFieldModule,
      MatInputModule,
      RouterModule,
      RouterOutlet, 
      CommonModule,
      RouterLink,
      RouterLinkActive,
      ReactiveFormsModule,
      MatIconModule,
      FormsModule
    ],
  providers:
    [
      provideAnimations(),
      //{ provide: MAT_FORM_FIELD_DEFAULT_OPTIONS, useValue: { appearance: 'outline' } }
    ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  username: string = '';
  password: string = '';

  constructor(private router: Router, private loginService: LoginService) { }

  public login(): void {
    console.log('Login button clicked!');
      //call the backend and authenticate the user and get the userid
      //var userId = this.loginService.Login(this.username, this.password);
      //this.router.navigate(['/jobprofile'], { queryParams: { username: this.username } });

    this.loginService.Login(this.username, this.password).subscribe(
      response => {
        console.log('Login successful', response);
        var userId = response.userId;
        this.router.navigate(['/jobprofile'], { queryParams: { username: this.username } });
      },
      error => {
        console.error('Login failed', error);
      });
  }
}
