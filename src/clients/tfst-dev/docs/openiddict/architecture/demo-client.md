---
id: demo-client
title: Demo Client
---

# Demo Client Application

## Introduction

The demo client (tfst-demo) is a simple web application that implements OpenID Connect authentication using the oidc-client-ts library. It interacts with TFST.AuthServer for authentication and TFST.API to access protected resources.

## Role in Architecture

The demo client acts as an API consumer, allowing users to perform operations that require authentication. It communicates with the authentication server to obtain access tokens and uses these tokens to access protected resources in the API.

## Communication with the API

1. **Authentication**: The client sends user credentials to the authentication server. If the credentials are valid, the server returns an access token.
   
2. **Resource Access**: With the access token, the client can make requests to the API. The token is included in the authorization header of each request.

3. **Error Handling**: The client must handle authentication and authorization errors, providing clear messages to the user if problems occur.

## Configuration

To configure the demo client, ensure the following parameters are correctly set:

- **Authentication Server URL**: The address of the TFST.AuthServer authentication server.
- **API URL**: The address of the TFST API that the client will use to make requests.

## Main Components

### 1. OIDC Configuration

```javascript
const userManager = new oidc.UserManager({
    authority: 'https://localhost:6001',                    // AuthServer URL
    client_id: 'tfst_clientwebapp',                        // Registered client ID
    response_type: 'code',                                 // Authorization code flow
    scope: 'openid profile email roles offline_access TFST_API', // Requested scopes
    redirect_uri: `${window.location.origin}/signin-callback.html`,
    post_logout_redirect_uri: `${window.location.origin}/index.html`,
    automaticSilentRenew: true,                           // Automatic token renewal
    includeIdTokenInSilentRenew: true
});
```

### 2. PKCE Authentication Flow

The PKCE (Proof Key for Code Exchange) flow is automatically handled by oidc-client-ts:

1. **Login**:
```javascript
async function login() {
    try {
        // Automatically generates code_verifier and code_challenge
        await userManager.signinRedirect();
    } catch (error) {
        console.error('Error during login:', error);
    }
}
```

2. **Authentication Callback** (signin-callback.html):
```javascript
// Processes authentication response and validates code_verifier
await userManager.signinCallback();
```

### 3. Token Management

```javascript
// Get current user and tokens
const user = await userManager.getUser();
if (user) {
    console.log('Access Token:', user.access_token);
    console.log('ID Token:', user.id_token);
    console.log('Refresh Token:', user.refresh_token);
}

// Renew tokens
async function refreshToken() {
    try {
        await userManager.signinSilent();
        console.log('Token successfully renewed');
    } catch (error) {
        console.error('Error renewing token:', error);
    }
}
```

### 4. API Calls

```javascript
async function callApi() {
    const user = await userManager.getUser();
    if (!user) throw new Error('Not authenticated');

    const response = await fetch('https://localhost:5001/api', {
        headers: {
            'Authorization': `Bearer ${user.access_token}`
        }
    });

    if (!response.ok) throw new Error(`HTTP Error: ${response.status}`);
    return await response.text();
}
```

### 5. Logout

```javascript
async function logout() {
    await userManager.signoutRedirect();
}
```

## Events and State Management

```javascript
userManager.events.addUserLoaded(user => {
    console.log('User loaded:', user);
});

userManager.events.addSilentRenewError(error => {
    console.error('Silent renewal error:', error);
});

userManager.events.addAccessTokenExpiring(() => {
    console.log('Token about to expire');
});
```

## Complete Authentication Flow

1. User clicks "Login"
2. userManager.signinRedirect():
   - Generates code_verifier and code_challenge
   - Redirects to AuthServer
3. User authenticates in AuthServer
4. AuthServer redirects to signin-callback.html
5. userManager.signinCallback():
   - Validates the response
   - Stores tokens
   - Redirects to application

## Security

- PKCE protects against authorization code interception attacks
- Tokens are stored in memory
- Automatic token renewal configured
- Audience and issuer validation in tokens

## Required Configuration in AuthServer

```json
{
  "AuthServer": {
    "TfstApp": {
      "ClientId": "tfst_clientwebapp",
      "RedirectUris": [
        "http://localhost:7000/signin-callback.html"
      ],
      "PostLogoutRedirectUris": "http://localhost:7000/index.html"
    }
  }
}
```

## Good Practices

1. Handle authentication and API errors appropriately
2. Implement interceptors for automatic token renewal
3. Validate authentication status before API calls
4. Use HTTPS in production
5. Keep redirect URLs updated