# Authentication Flow with OIDC Client TS

## 1. Client Configuration
```javascript
const userManager = new oidc.UserManager({
    authority: 'https://localhost:6001',
    client_id: 'tfst_clientwebapp',
    redirect_uri: '.../signin-callback.html',
    response_type: 'code',
    scope: 'openid profile email roles offline_access TFST_API',
    automaticSilentRenew: true,
});
```

## 1.1. Provider Metadata Discovery (OIDC Discovery)
Before starting any authentication flow, `oidc-client-ts` uses the configured `authority` (`https://localhost:6001`) to automatically discover the AuthServer's features and endpoints. It does this by querying the metadata discovery endpoint, typically located at:

`https://localhost:6001/.well-known/openid-configuration`

This endpoint returns a JSON document containing, among other things:

- The exact URLs for the authorization (`authorization_endpoint`), token (`token_endpoint`), userinfo (`userinfo_endpoint`) endpoints, etc.
- The supported signing methods for ID Tokens.
- The supported scopes.
- The location of the server's public keys (JWKS endpoint).

`oidc-client-ts` uses this information to internally configure the correct URLs and validate received tokens, making client configuration simpler and less error-prone if URLs change on the server (as long as the base `authority` remains the same).

## 2. Authentication Flow

### 2.1. Flow Start (Login)
1. User clicks "Login"
2. `userManager.signinRedirect()` is executed
3. OIDC Client TS automatically:
   - Generates code_verifier and code_challenge: 
     - `code_verifier`: random string
     - `code_challenge`: SHA256 hash of `code_verifier` (sent to AuthServer)
   - Stores state and nonce:
     - `state`: random string to validate response
     - `nonce`: random string to protect against replay attacks
4. Redirects to AuthServer

### 2.2. Process in AuthServer
```http
GET /connect/authorize
    ?client_id=tfst_clientwebapp
    &redirect_uri=http://localhost:7000/signin-callback.html
    &response_type=code
    &scope=openid profile email roles offline_access TFST_API
    &code_challenge=xxxxx
    &code_challenge_method=S256
    &state=xxxxx
    &nonce=xxxxx
```
1. The AuthServer authenticates the user, obtains consent (if necessary), and if everything is correct, generates an authorization code.
2. Redirects the browser back to the client's `redirect_uri`, including the authorization code and original state in the URL parameters.
```http
GET /signin-callback.html?code=xxxxx&state=xxxxx
```

### 2.3. Callback and Token Retrieval
1. The client (the `signin-callback.html` page) receives the authorization code and state.
2. The script in `signin-callback.html` executes await `userManager.signinCallback();`.
3. OIDC Client TS automatically:
   - Retrieves the stored `code_verifier`.
   - Sends a POST request to the AuthServer's token endpoint (`/connect/token`) to exchange the authorization code for tokens.
```http
POST /connect/token
    Content-Type: application/x-www-form-urlencoded

    grant_type=authorization_code
    &client_id=tfst_clientwebapp  // client_id is included, but NOT a client_secret
    &code=xxxxx
    &redirect_uri=http://localhost:7000/signin-callback.html
    &code_verifier=xxxxx // code_verifier is included
```
4. The AuthServer receives the request:
   - Verifies the `client_id` and `redirect_uri`.
   - Looks up the received authorization code and the associated `code_challenge` that was sent in the authorization step.
   - Applies the hash function to the received `code_verifier` and verifies that the result matches the stored `code_challenge`.
   - If all validations pass, the AuthServer issues the tokens (Access Token, ID Token, Refresh Token if requested).

5. The AuthServer responds to the client with the tokens (usually in JSON format):
```json
{
    "access_token": "xxxxx",
    "refresh_token": "xxxxx", // Present if the 'offline_access' scope was requested
    "id_token": "xxxxx",      // Present if the 'openid' scope was requested
    "expires_in": 3600,
    "token_type": "Bearer"
}
```
6. OIDC Client TS automatically:
   - Validates the `id_token` (signature, nonce, claims).
   - Stores the received tokens securely (typically in `sessionStorage` or `localStorage`, configured in the `userManager`).
   - Loads user information from the `id_token` and/or by making a request to the `userinfo` endpoint (if necessary and configured).
   - Completes the `signinCallback()` process.

7. The client application can now consider the user authenticated and use the tokens. Typically, the `signin-callback.html` page will redirect the user to the main page of the application (e.g., `index.html`).

### 2.4. Token Usage
1. Get user and token:
   ```javascript
   const user = await userManager.getUser();
   // Use access_token for API calls
   fetch(url, {
       headers: {
           'Authorization': `Bearer ${user.access_token}`
       }
   });
   ```

### 2.5. Automatic Renewal
1. automaticSilentRenew: true enables:
   - Automatic renewal before expiration
   - Use of refresh_token without intervention
   - Token updates in memory

### 2.6. Logout
1. User clicks "Logout"
2. `userManager.signoutRedirect()` is executed
3. Local session cleanup and logout in AuthServer is performed
4. Redirect to logout page

## 3. Events and State
```javascript
userManager.events.addUserLoaded(user => {
    // User authenticated
});
userManager.events.addUserUnloaded(() => {
    // User disconnected
});
```

## 4. Security Implemented
- PKCE automatic
- State/nonce validation
- Secure token storage
- Silent token renewal
- Handling of authentication errors

## 5. Advantages of OIDC Client TS
- Standard implementation of OAuth 2.0/OIDC
- Automatic PKCE handling
- Transparent token management
- Automatic session renewal
- Events for UI synchronization