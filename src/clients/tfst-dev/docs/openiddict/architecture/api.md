---
id: api
title: Api
---

# TFST.API

## Purpose
TFST.API acts as a resource server that protects endpoints using JWT tokens issued by TFST.AuthServer. It implements OpenIddict.Validation for token validation and authorization management.

## Main Components

### 1. OpenIddict Configuration
```csharp
builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        options.SetIssuer("https://localhost:6001/");
        options.AddAudiences("resource_server");
        options.AddEncryptionKey(new SymmetricSecurityKey(...));
        options.UseSystemNetHttp();
        options.UseAspNetCore();
    });
```

### 2. Security and Token Validation
- **Issuer**: `https://localhost:6001/`
- **Audience**: `resource_server`
- **Encryption Key**: Shared symmetric key with AuthServer
- **Automated validation of**:
  - Token signature
  - Expiration time
  - Valid audience
  - Correct issuer

### 3. CORS Configuration
- Allowed origins configured in appsettings:
  - Default: `http://localhost:7000`
- Headers: Allows any header
- Methods: Allows any HTTP method

### 4. Authentication and Authorization
- Scheme: OpenIddictValidationAspNetCoreDefaults
- Middleware configured in pipeline:
  ```csharp
  app.UseAuthentication();
  app.UseAuthorization();
  ```
- Endpoint protection using [Authorize] attribute

### 5. Request Flow
1. Client sends request with Bearer token
2. Middleware validates the token with OpenIddict
3. If valid, ClaimsPrincipal is established
4. The endpoint is executed if authorization is correct

### 6. Environment Configuration
- Development:
  ```json
  {
    "OpenIddict": {
      "Issuer": "https://localhost:6001/",
      "Audience": "resource_server"
    },
    "Security": {
      "EncryptionKey": "DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY="
    }
  }
  ```

## Security Aspects
- HTTPS mandatory in production
- Full validation of JWT tokens
- CORS configured for specific origins
- Encryption keys managed by configuration
- User claims available via ClaimsPrincipal

## Protected Endpoints
All endpoints under `/api` require valid authentication via JWT token.
Example:
```csharp
[Authorize]
[Route("api/[controller]")]
public class ResourceController : ControllerBase
{
    // Protected endpoints
}
```