/**
 * Configuration for Microsoft Authentication
 */
export const msalConfig = {
  auth: {
    // 'Application (client) ID' of app registration in the Microsoft Entra admin center
    clientId: 'E57e608a4-c457-42ce-825e-fc46a80e9dcf',
    
    // Full directory URL, in the form of https://login.microsoftonline.com/<tenant>
    authority: `https://login.microsoftonline.com/5d54e111-b859-4ae3-a6d7-68d3394a4cbc`,

    // Must be the same redirectUri as what was provided in your Microsoft Entra app registration
    redirectUri: 'https://wwwjobtracker.dev/loginauth',
  }
};

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
