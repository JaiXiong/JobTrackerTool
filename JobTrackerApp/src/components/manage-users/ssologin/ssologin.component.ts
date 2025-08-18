import { Component } from '@angular/core';
import { MatIconModule, MatIconRegistry } from "@angular/material/icon";
import { MatButtonModule } from '@angular/material/button';
import { DomSanitizer } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { SsoAuthService } from '../../../services/auth/sso-auth.service';

@Component({
  selector: 'app-ssologin',
  standalone: true,
  imports: 
  [
    MatIconModule,
    MatButtonModule,
    HttpClientModule
  ],
  providers: [
    MatIconRegistry
  ],
  templateUrl: './ssologin.component.html',
  styleUrl: './ssologin.component.scss'
})
export class SsologinComponent {
  constructor(
    private iconRegistry: MatIconRegistry,
    private domSanitizer: DomSanitizer,
    private ssoAuthService: SsoAuthService
  ) {
    // Register icons with standard path
    // this.iconRegistry.addSvgIcon(
    //   'microsoft_azure', 
    //   this.domSanitizer.bypassSecurityTrustResourceUrl('assets/Icons/microsoft_azure.svg')
    // );

    this.iconRegistry.addSvgIcon(
      'microsoft', 
      this.domSanitizer.bypassSecurityTrustResourceUrl('assets/Icons/microsoft.svg')
    );
    
    this.iconRegistry.addSvgIcon(
      'google',
      this.domSanitizer.bypassSecurityTrustResourceUrl('assets/Icons/google.svg')
    );
  }

  /**
   * Initiates the Microsoft SSO login flow
   */
  loginWithMicrosoft() {
    console.log('Microsoft SSO login initiated');
    this.ssoAuthService.loginWithMicrosoft();
  }

  /**
   * Initiates the Google SSO login flow
   */
  loginWithGoogle() {
    console.log('Google SSO login initiated');
    this.ssoAuthService.loginWithGoogle();
  }
}
