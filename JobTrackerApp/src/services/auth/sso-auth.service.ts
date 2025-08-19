import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, catchError, Observable, of, Subject } from 'rxjs';
import { AuthService } from './auth.service';
import { googleAuthConfig } from './auth.config';
import { MSAL_GUARD_CONFIG, MsalBroadcastService, MsalGuardConfiguration, MsalService } from '@azure/msal-angular';
import { PopupRequest, RedirectRequest } from '@azure/msal-browser';

@Injectable({
  providedIn: 'root'
})

export class SsoAuthService {
  private readonly _destroying$ = new Subject<void>();
  private ssoUserSubject = new BehaviorSubject<any>(null);
  public ssoUser$ = this.ssoUserSubject.asObservable();
  private isFirefox = window.navigator.userAgent.indexOf('Firefox') > -1;
  private msalServiceInit = false;

  constructor(
    @Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
    private http: HttpClient,
    private router: Router,
    private authService: AuthService,
    private msalService: MsalService,
    private msalBroadcastService: MsalBroadcastService
  ) {
    //this.checkAccount();
  }

  private async ensureMsalInitialized(): Promise<void> {
    if (!this.msalServiceInit) {
      await this.msalService.instance.initialize();
      this.msalServiceInit = true;
    }
  }

  /**
   * Initiates Microsoft login flow
   */
  public async loginWithMicrosoft(): Promise<void> {
    await this.ensureMsalInitialized();

    if (this.isFirefox) {
      if (this.msalGuardConfig.authRequest) {
          this.msalService.loginPopup({...this.msalGuardConfig.authRequest } as PopupRequest)
          .pipe(
            catchError((error: any) => {
              console.error('Error during Firefox popup login', error);
              this.msalService.loginRedirect({...this.msalGuardConfig.authRequest } as RedirectRequest);
              return of(null);
            })
          ).subscribe();
        } else {
          this.msalService.loginPopup()
          .pipe(
            catchError((error: any) => {
              console.error('Error during Firefox popup login', error);
              this.msalService.loginRedirect();
              return of(null);
            })
          ).subscribe();
        }
      } else {
        if (this.msalGuardConfig.authRequest) {
          this.msalService.loginRedirect({ ...this.msalGuardConfig.authRequest } as RedirectRequest);
        } else {
          this.msalService.loginRedirect();
        }
      }
    }

  /**
   * Initiates Google login flow
   */
  public loginWithGoogle(): void {
    // Implement Google OAuth flow
    // This is a simplified example - typically you'd use a library like gapi
    const googleAuthEndpoint = 'https://accounts.google.com/o/oauth2/v2/auth';
    const redirectUri = `${window.location.origin}/auth-callback`;
    
    const googleAuthUrl = `${googleAuthEndpoint}?` +
      `client_id=${googleAuthConfig.clientId}` +
      `&redirect_uri=${encodeURIComponent(redirectUri)}` +
      `&response_type=code` +
      `&scope=${encodeURIComponent('email profile')}` +
      `&prompt=select_account`;
    
    // Redirect to Google's OAuth page
    window.location.href = googleAuthUrl;
  }

  /**
   * Handles the authentication response from SSO providers
   * @param token Access token from SSO provider
   * @param provider The SSO provider ('microsoft' or 'google')
   */
  private handleSsoAuthentication(token: string | undefined, provider: string): void {
    if (!token) {
      console.error('No token received from SSO provider');
      return;
    }
    
    // Call your backend API to validate the token and get your app's JWT token
    this.http.post('/api/auth/sso-login', { token, provider })
      .subscribe({
        next: (response: any) => {
          // Use your existing auth service to store tokens and set user state
          this.authService.doLoginUser(
            response.email, 
            response.access_token, 
            response.refresh_token
          );
          
          // Navigate to home or dashboard
          this.router.navigate(['/dashboard']);
        },
        error: (error) => {
          console.error('Error authenticating with backend:', error);
        }
      });
  }
  
  /**
   * Processes Google auth callback with code
   * @param code Authorization code from Google
   */
  public processGoogleCallback(code: string): void {
    // Exchange code for token with your backend
    this.http.post('/api/auth/google-callback', { code })
      .subscribe({
        next: (response: any) => {
          this.handleSsoAuthentication(response.token, 'google');
        },
        error: (error) => {
          console.error('Error processing Google callback:', error);
        }
      });
  }
  
  /**
   * Logs the user out of SSO services
   */
  public logout(): void {
    // Log out from Microsoft
    //this.msalInstance.logout();
    
    // Clear local user state
    this.ssoUserSubject.next(null);
    
    // Use existing auth service to clear tokens
    this.authService.logout();
  }
}
