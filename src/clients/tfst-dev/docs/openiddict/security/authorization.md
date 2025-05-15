# Authorization Flow (Protected Resource Access)

This flow describes how the client application (JavaScript SPA) uses the tokens obtained in the OIDC authentication flow (specifically the `access_token`) to access the protected API.

We assume that the user has successfully completed the authentication flow and that the client has obtained and stored a valid, non-expired `access_token` (this is automatically handled by `oidc-client-ts`).

## 1. Request Preparation in the Client

1. The client, the JavaScript application (SPA) needs to make a call to a protected endpoint in the API (the Resource Server).
2. Before sending the HTTP request (GET, POST, PUT, DELETE, etc.) to the API endpoint, the client retrieves the locally stored `access_token`. This is usually done by calling `userManager.getUser()` after the user has authenticated and loaded.
3. The client includes the `access_token` in the HTTP `Authorization` header. The standard format is to use the `Bearer` scheme followed by a space and the token.

    Example of how the outgoing HTTP request from the browser would look (using Fetch API):

    ```javascript
    const user = await userManager.getUser(); // Get the stored user and their tokens

    if (user && user.access_token) {
        const url = 'https://localhost:5001/api/protecteddata'; // Protected API endpoint URL

        fetch(url, {
            method: 'GET', // Or POST, PUT, DELETE, etc.
            headers: {
                'Authorization': `Bearer ${user.access_token}`, // Include Access Token
                'Content-Type': 'application/json' // If sending a JSON body
            }
        })
        .then(response => {
            if (!response.ok) {
                // Handle errors (e.g., 401 Unauthorized, 403 Forbidden)
                console.error('Error accessing API:', response.status);
                if (response.status === 401) {
                    // Token might have expired or be invalid.
                    // oidc-client-ts with automaticSilentRenew should handle this,
                    // but if it fails, might need to redirect to login.
                }
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json(); // Or response.text() if not JSON
        })
        .then(data => {
            console.log('API data:', data);
            // Process data received from API
        })
        .catch(error => {
            console.error('There was a problem with the fetch request:', error);
            // Handle network or other errors
        });
    } else {
        console.warn('User not authenticated or access_token not available.');
        // Maybe redirect to login if no user
        // userManager.signinRedirect();
    }
    ```

    The crucial part is the `Authorization: Bearer [obtained_access_token]` header.

## 2. Reception and Validation in the API (Resource Server)

1. The API (`TFST.API`) receives the incoming HTTP request.
2. The authentication and authorization middleware configured in the API intercepts the request **before** it reaches the business logic (specific controllers/endpoints).
3. This middleware:
    * Extracts the token from the `Authorization` header.
    * Performs `access_token` validation. For a JWT (JSON Web Token) issued by OpenIddict, this typically involves:
        * Verifying the token **signature** using AuthServer OpenIddict's public key or certificate to ensure the token hasn't been tampered with.
        * Verifying the **issuer** (`iss` claim) to ensure the token comes from the expected AuthServer.
        * Verifying the **audience** (`aud` claim) to ensure the token is intended for this specific API (the `aud` claim in the token must match an identifier configured for the API as a resource in OpenIddict).
        * Verifying the **expiration date** (`exp` claim) to ensure the token is still valid.
        * Other validations like `nbf` (Not Before) and `iat` (Issued At).
    * If the token is valid, the middleware authenticates the user associated with the token within the current HTTP request context. The token's claims (like `sub`, `name`, `email`, `roles`, etc.) become the user's identities and principals available to the API's business logic.
4. After authentication (token validation), the API's authorization middleware verifies if the authenticated user (based on token claims) has the necessary permissions to access the requested endpoint. This could be based on `scopes`, `roles`, or more granular authorization policies defined in the API (e.g., using `[Authorize(Roles = "Admin")]` or `[Authorize(Policy = "RequiresApiAccess")]` attributes on controllers).

## 3. Authorization Result

1.  **If the Token is Valid and the User is Authorized:**
    * The request passes the authentication and authorization checks.
    * The request reaches the API controller/action method.
    * The API's business logic executes, processes the request, and returns the appropriate data or response to the client (e.g., HTTP status 200 OK).
2.  **If the Token is Invalid or the User is Not Authorized:**
    * The authentication or authorization middleware intercepts the request.
    * The API rejects the request before it reaches the business logic.
    * The API returns an error HTTP status code to the client:
        * `401 Unauthorized`: Usually indicates that authentication failed (the token is missing, invalid, expired, the signature is incorrect, etc.).
        * `403 Forbidden`: Usually indicates that the user is authenticated (the token is valid), but does not have the necessary permissions (roles/scopes/claims) to access this particular resource.

## 4. Renewal and Retry (Handled by OIDC Client TS)

* If the client receives a `401 Unauthorized` (for example, because the `access_token` just expired), `oidc-client-ts` (if `automaticSilentRenew` is enabled) will automatically try to renew the `access_token` in the background using the `refresh_token` (if obtained).
* If the renewal is successful, the `userManager` will be updated with the new `access_token`. The client application can then retry the API call with the new token.
* If the renewal fails (for example, the `refresh_token` has expired or has been revoked), `oidc-client-ts` may trigger an event (`accessTokenExpiring`, `accessTokenExpired`) and the client application will need to redirect the user to re-authenticate completely (return to step 2.1 of the authentication flow).

This authorization flow is the standard way SPAs access protected APIs using JWTs issued by an OAuth 2.0 / OIDC server.