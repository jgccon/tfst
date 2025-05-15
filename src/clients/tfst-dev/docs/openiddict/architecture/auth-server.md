# TFST.AuthServer

## Purpose
TFST.AuthServer is the authentication/authorization server that implements OpenID Connect and OAuth 2.0 using OpenIddict. It manages user authentication and issues JWT tokens for accessing protected resources.

## Main Components

### 1. OpenIddict Configuration
```csharp
builder.Services.AddOpenIddict()
    .AddServer(options =>
    {
        // Endpoints
        options.SetAuthorizationEndpointUris("connect/authorize")
               .SetTokenEndpointUris("connect/token")
               .SetUserInfoEndpointUris("connect/userinfo");

        // Allowed flows
        options.AllowAuthorizationCodeFlow()
              .AllowRefreshTokenFlow()
              .RequireProofKeyForCodeExchange(); // PKCE required
```
### 2. Supported Scopes
- Standard Scopes:
    - `openid`: OpenID Connect Authentication
    - `profile`: Basic user information
    - `email`: Email address
    - `roles`: User roles

- Custom Scopes:
    - `TFST_API`: Custom scope that grants access to TFST.API as it configures the token audience with `resource_server`.

    ```json
    "ApiScopes": [
        {
            "Name": "TFST_API",
            "Resource": "resource_server"
        }
    ]
    ```

### 3. Security and Tokens
- PKCE (Proof Key for Code Exchange):
    - Required for the authorization code flow
    - Protects against interception attacks
    - Automatically implemented by OpenIddict

- Token Issuance:
```csharp
// JWT with user claims
identity.SetClaim(Claims.Subject, userId)
       .SetClaim(Claims.Email, email)
       .SetClaim(Claims.Name, username)
       .SetClaims(Claims.Role, roles);
```	

- Validation:
    - Issuer: `https://localhost:6001/`
    - Audience: `resource_server`
    - Encryption Key: Configured symmetric key

### 4. Registered Clients
```json
{
  "AuthServer": {
    "TfstApp": {
      "ClientId": "tfst_clientwebapp",
      "DisplayName": "TFST Frontend",
      "RedirectUris": ["http://localhost:7000/signin-callback.html"]
    },
    "ResourceServer": {
      "ClientId": "resource_server",
      "ClientSecret": "846B62D0-DEF9-4215-A99D-86E6B8DAB342"
    }
  }
}
```

### 5. Storage
- SQL Server database with `auth` schema
- Main tables:
    - `Users`: Users and credentials
    - `OpenIddictApplications`: Registered clients
    - `OpenIddictAuthorizations`: Authorizations
    - `OpenIddictTokens`: Issued tokens
    - `OpenIddictScopes`: Supported scopes

### 6. Authentication Process
    1. Client requests authorization with PKCE
    2. User authenticates (if necessary)
    3. AuthServer validates credentials
    4. Authorization code is issued
    5. Client exchanges code for tokens
    6. Access_token and refresh_token are issued

### 7. Refresh Tokens
    - Configurable duration (14 days by default)
    - Stored in `OpenIddictTokens`
    - Automatic rotation when used
    - Active user validation on each use

## Security Aspects
    - HTTPS required in production
    - Tokens encrypted with symmetric key
    - PKCE required for public clients
    - Redirect_uri validation
    - Secure storage of secrets
    - Rate limiting on critical endpoints

## Main Endpoints
- `/connect/authorize`: Start of authorization flow
- `/connect/token`: Token issuance and refresh
- `/connect/userinfo`: User information
- `/Account/Login`: Login UI
- `/Account/Register`: Registration UI