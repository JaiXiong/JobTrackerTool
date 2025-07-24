import { Component, OnInit, NgZone, AfterViewInit, Renderer2, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';

declare global {
  interface Window {
    grecaptcha: any;
    onCaptchaLoaded: () => void;
    verifyCaptcha: (token: string) => void;
  }
}

@Component({
  selector: 'app-verification',
  templateUrl: './verification.component.html',
  styleUrls: ['./verification.component.scss'],
  standalone: true,
  imports: [
    CommonModule
  ],
  
})
export class VerificationComponent implements OnInit, AfterViewInit {
  public captchaRendered: boolean = false;
  private captchaLoaded: boolean = false;
  
  constructor(
    private renderer: Renderer2,
    private zone: NgZone,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      // Add reCAPTCHA script to the page
      const script = this.renderer.createElement('script');
      script.src = 'https://www.google.com/recaptcha/api.js?onload=onCaptchaLoaded&render=explicit';
      script.async = true;
      script.defer = true;
      
      // Set up callback for when reCAPTCHA is loaded
      window.onCaptchaLoaded = () => {
        this.zone.run(() => {
          this.captchaLoaded = true;
          this.renderReCaptcha();
        });
      };
      
      // Set up callback for when captcha is verified
      window.verifyCaptcha = (token: string) => {
        this.zone.run(() => {
          console.log('Captcha verified with token:', token);
          this.onVerify(token);
        });
      };
      
      this.renderer.appendChild(document.body, script);
    }
  }

  ngAfterViewInit(): void {
    if (isPlatformBrowser(this.platformId) && this.captchaLoaded) {
      this.renderReCaptcha();
    }
  }

  renderReCaptcha(): void {
    if (!isPlatformBrowser(this.platformId) || this.captchaRendered || !this.captchaLoaded) {
      return;
    }
    
    const element = document.querySelector('.g-recaptcha');
    if (!element) {
      console.error('reCAPTCHA element not found');
      return;
    }
    
    try {
      window.grecaptcha.render(element, {
        sitekey: '6LcNroorAAAAALMvB18TElfj5bE-uHtEaOKOft8Y',
        callback: 'verifyCaptcha'
      });
      this.captchaRendered = true;
    } catch (error) {
      console.error('Error rendering reCAPTCHA:', error);
    }
  }
  
  onVerify(token: string): void {
    // Handle verification success
    // You can emit an event, call a service, etc.
    console.log('Verification successful with token:', token);
    // TODO: Send this token to your backend for verification
  }
}