import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { msalConfig, loginRequest, googleAuthConfig } from './auth.config';

// For Microsoft Authentication
import * as msal from '@azure/msal-browser';

@Injectable({
  providedIn: 'root'
})
export class SsoAuthService {
  private msalInstance: msal.PublicClientApplication;
  private ssoUserSubject = new BehaviorSubject<any>(null);
  public ssoUser$ = this.ssoUserSubject.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router,
    private authService: AuthService
  ) {
    // Initialize MSAL instance for Microsoft authentication
    this.msalInstance = new msal.PublicClientApplication(msalConfig);
    
    // Check if user is already signed in
    this.checkAccount();
  }

  /**
   * Checks if there's an active account in the MSAL instance
   */
  private checkAccount(): void {
    const accounts = this.msalInstance.getAllAccounts();
    if (accounts.length > 0) {
      // User is already logged in
      const user = accounts[0];
      this.ssoUserSubject.next(user);
    }
  }

  /**
   * Initiates Microsoft login flow
   */
  public loginWithMicrosoft(): void {
    this.msalInstance.loginPopup(loginRequest)
      .then(response => {
        // Handle successful login
        console.log('Login successful', response);
        this.ssoUserSubject.next(response.account);
        
        // Get access token for API access
        return this.getTokenSilent();
      })
      .then(tokenResponse => {
        // Use token for API access or integrate with the existing auth service
        this.handleSsoAuthentication(tokenResponse?.accessToken, 'microsoft');
      })
      .catch(error => {
        console.error('Microsoft login error:', error);
      });
  }

  /**
   * Gets access token silently if user is already logged in
   */
  private getTokenSilent(): Promise<msal.AuthenticationResult | null> {
    const accounts = this.msalInstance.getAllAccounts();
    if (accounts.length === 0) {
      return Promise.resolve(null);
    }
    
    const request = {
      ...loginRequest,
      account: accounts[0]
    };
    
    return this.msalInstance.acquireTokenSilent(request);
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
    this.msalInstance.logout();
    
    // Clear local user state
    this.ssoUserSubject.next(null);
    
    // Use existing auth service to clear tokens
    this.authService.logout();
  }
}
