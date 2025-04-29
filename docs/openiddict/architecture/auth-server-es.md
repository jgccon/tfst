# TFST.AuthServer

## Propósito
TFST.AuthServer es el servidor de autenticación/autorización que implementa OpenID Connect y OAuth 2.0 usando OpenIddict. Gestiona la autenticación de usuarios y emite tokens JWT para acceder a recursos protegidos.

## Componentes Principales

### 1. Configuración OpenIddict
```csharp
builder.Services.AddOpenIddict()
    .AddServer(options =>
    {
        // Endpoints
        options.SetAuthorizationEndpointUris("connect/authorize")
               .SetTokenEndpointUris("connect/token")
               .SetUserInfoEndpointUris("connect/userinfo");

        // Flujos permitidos
        options.AllowAuthorizationCodeFlow()
              .AllowRefreshTokenFlow()
              .RequireProofKeyForCodeExchange(); // PKCE obligatorio
```
### 2. Scopes Soportados
- Scopes Estándar:
    - `openid`: Autenticación OpenID Connect
    - `profile`: Información básica del usuario
    - `email`: Correo electrónico
    - `roles`: Roles del usuario

- Scopes Personalizados:
    - `TFST_API`: Scope personalizado que otorga acceso a TFST.API ya que configura la audiencia del token con `resource_server`.

    ```json
    "ApiScopes": [
        {
            "Name": "TFST_API",
            "Resource": "resource_server"
        }
    ]
    ```

### 3. Seguridad y Tokens
- PKCE (Proof Key for Code Exchange):
    - Obligatorio para el flujo de código de autorización
    - Protege contra ataques de intercepción
    - Implementado automáticamente por OpenIddict

- Emisión de Tokens:
```csharp
// JWT con claims del usuario
identity.SetClaim(Claims.Subject, userId)
       .SetClaim(Claims.Email, email)
       .SetClaim(Claims.Name, username)
       .SetClaims(Claims.Role, roles);
```	

- Validación:
    - Issuer: `https://localhost:6001/`
    - Audience: `resource_server`
    - Encryption Key: Clave simétrica configurada

### 4. Clientes Registrados
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

### 5. Almacenamiento
- Base de datos SQL Server con esquema `auth`
- Tablas principales:
    - `Users`: Usuarios y credenciales
    - `OpenIddictApplications`: Clientes registrados
    - `OpenIddictAuthorizations`: Autorizaciones
    - `OpenIddictTokens`: Tokens emitidos
    - `OpenIddictScopes`: Scopes soportados

### 6. Proceso de Autenticación
    1. Cliente solicita autorización con PKCE
    2. Usuario se autentica (si es necesario)
    3. AuthServer valida credenciales
    4. Se emite código de autorización
    5. Cliente intercambia código por tokens
    6. Se emiten access_token y refresh_token

### 7. Refresh Tokens
    - Duración configurable (por defecto 14 días)
    - Almacenados en `OpenIddictTokens`
    - Rotación automática al usar
    - Validación de usuario activo en cada uso

## Aspectos de Seguridad
    - HTTPS obligatorio en producción
    - Tokens encriptados con clave simétrica
    - PKCE obligatorio para clientes públicos
    - Validación de redirect_uri
    - Almacenamiento seguro de secretos
    - Rate limiting en endpoints críticos

## Endpoints Principales
- `/connect/authorize`: Inicio del flujo de autorización
- `/connect/token`: Emisión de tokens y refresh de tokens
- `/connect/userinfo`: Información del usuario
- `/Account/Login`: UI de login
- `/Account/Register`: UI de registro