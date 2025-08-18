import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SsoAuthService } from '../../../services/auth/sso-auth.service';
import { CommonModule } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-auth-callback',
  standalone: true,
  imports: [
    CommonModule,
    MatProgressSpinnerModule
  ],
  template: `
    <div class="auth-callback-container">
      <h2>Processing authentication...</h2>
      <mat-spinner></mat-spinner>
    </div>
  `,
  styles: [`
    .auth-callback-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      height: 100vh;
      text-align: center;
    }
    
    h2 {
      margin-bottom: 20px;
      color: #333;
    }
  `]
})
export class AuthCallbackComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private ssoAuthService: SsoAuthService
  ) {}

  ngOnInit(): void {
    // Check query parameters for OAuth code
    this.route.queryParams.subscribe(params => {
      if (params['code']) {
        // This is a Google OAuth callback with a code
        this.ssoAuthService.processGoogleCallback(params['code']);
      } else {
        // For Microsoft, MSAL.js handles the redirect automatically
        // Just redirect to home or dashboard
        this.router.navigate(['/']);
      }
    });
  }
}
