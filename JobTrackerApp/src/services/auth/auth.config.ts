//import { BrowserCacheLocation } from "@azure/msal-browser";

// Required for MSAL
import { IPublicClientApplication, PublicClientApplication, InteractionType, BrowserCacheLocation, LogLevel } from '@azure/msal-browser';
import { MsalGuard, MsalInterceptor, MsalBroadcastService, MsalInterceptorConfiguration, MsalModule, MsalService, MSAL_GUARD_CONFIG, MSAL_INSTANCE, MSAL_INTERCEPTOR_CONFIG, MsalGuardConfiguration, MsalRedirectComponent } from '@azure/msal-angular';


const isIE = window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;
const isFirefox = window.navigator.userAgent.indexOf('Firefox') > -1;
/**
 * Configuration for Microsoft Authentication
 */
// export const msalConfig = {
//   auth: {
//     // 'Application (client) ID' of app registration in the Microsoft Entra admin center
//     clientId: '57e608a4-c457-42ce-825e-fc46a80e9dcf',
    
//     // Full directory URL, in the form of https://login.microsoftonline.com/<tenant>
//     authority: `https://login.microsoftonline.com/5d54e111-b859-4ae3-a6d7-68d3394a4cbc`,

//     // Must be the same redirectUri as what was provided in your Microsoft Entra app registration
//     //redirectUri: 'https://wwwjobtracker.dev/loginauth',
//     redirectUri: 'https://localhost:4200'
//   },
//   cache: {
//       cacheLocation: BrowserCacheLocation.LocalStorage,
//       storeAuthStateInCookie: isIE
//   },
//   system: {
//       allowRedirectInIframe: true,   // Firefox iframe handling
//       windowHashTimeout: 60000,     // Increase timeout for Firefox
//       iframeHashTimeout: 6000,      // Increase iframe timeout
//       navigateFrameWait: 0,         // Reduce frame wait time

//       loggerOptions: {
//       loggerCallback: (level: LogLevel, message: string) => {
//         console.log(message);
//       },
//       logLevel: LogLevel.Info,
//       piiLoggingEnabled: false
//     }
//   }
// }
  

/**
 * Add here the endpoints and scopes when obtaining an access token for protected API calls
 */
// export const protectedResources = {
//   apiJobTracker: {
//     endpoint: "https://your-api-endpoint-here/",
//     scopes: ["api://your-scope-here/access"]
//   }
// };

/**
 * Authentication request configuration options
 */
export const loginRequest = {
  scopes: ["openid", "profile", "User.Read"]
};

/**
 * Google auth config
 */
export const googleAuthConfig = {
  clientId: 'Enter_the_Google_Client_Id_Here'
};
