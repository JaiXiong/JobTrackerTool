import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from '../services/interceptor/auth.interceptor';
// Import the authentication configuration 
//import { msalConfig } from '../services/auth/auth.config';

// Microsoft Authentication imports
import {
  MSAL_INSTANCE,
  MsalService,
  MSAL_GUARD_CONFIG,
  MsalGuardConfiguration,
  MsalInterceptorConfiguration,
  MSAL_INTERCEPTOR_CONFIG,
  MsalBroadcastService,
  MsalGuard,
  MsalInterceptor
} from '@azure/msal-angular';
import { IPublicClientApplication, PublicClientApplication, InteractionType, BrowserCacheLocation } from '@azure/msal-browser';

const isIE = window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;
const isFirefox = window.navigator.userAgent.indexOf('Firefox') > -1;

// Factory function to create MSAL instance
export function MSALInstanceFactory(): IPublicClientApplication {
  return new PublicClientApplication({
    auth: {
        // 'Application (client) ID' of app registration in the Microsoft Entra admin center
        clientId: '57e608a4-c457-42ce-825e-fc46a80e9dcf',
        
        // Full directory URL, in the form of https://login.microsoftonline.com/<tenant>
        authority: `https://login.microsoftonline.com/5d54e111-b859-4ae3-a6d7-68d3394a4cbc`,
    
        // Must be the same redirectUri as what was provided in your Microsoft Entra app registration
        //redirectUri: 'https://wwwjobtracker.dev/loginauth',
        redirectUri: 'https://localhost:4200'
      },
      cache: {
          cacheLocation: BrowserCacheLocation.LocalStorage,
          storeAuthStateInCookie: isIE
      },
      system: {
          allowRedirectInIframe: true,   // Firefox iframe handling
          windowHashTimeout: 60000,     // Increase timeout for Firefox
          iframeHashTimeout: 6000,      // Increase iframe timeout
          navigateFrameWait: 0,         // Reduce frame wait time
      }
  });
}

// MSAL Interceptor is required to request access tokens in order to access the protected resource (Graph)
export function MSALInterceptorConfigFactory(): MsalInterceptorConfiguration {
  const protectedResourceMap = new Map<string, Array<string>>();
  protectedResourceMap.set('https://graph.microsoft.com/v1.0/me', ['user.read']);

  return {
    interactionType: isFirefox ? InteractionType.Popup : InteractionType.Redirect,
    protectedResourceMap
  };
}

// MSAL Guard configuration
export function MSALGuardConfigFactory(): MsalGuardConfiguration {
  return {
    interactionType: InteractionType.Redirect,
    authRequest: {
      scopes: ['user.read']
    }
  };
}

export const appConfig: ApplicationConfig = {
  providers: 
  [
    importProvidersFrom([BrowserAnimationsModule]),
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes), 
    //provideClientHydration(),
    provideHttpClient(withInterceptors([authInterceptor])),
    // Microsoft Authentication providers
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    },
    {
      provide: MSAL_INSTANCE,
      useFactory: MSALInstanceFactory
    },
    {
      provide: MSAL_GUARD_CONFIG,
      useFactory: MSALGuardConfigFactory
    },
    {
      provide: MSAL_INTERCEPTOR_CONFIG,
      useFactory: MSALInterceptorConfigFactory
    },
    MsalService,
    MsalGuard,
    MsalBroadcastService
  ]
};
