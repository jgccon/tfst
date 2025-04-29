# TFST.API

## Propósito
TFST.API actúa como servidor de recursos que protege los endpoints mediante tokens JWT emitidos por TFST.AuthServer. Implementa OpenIddict.Validation para la validación de tokens y gestión de autorización.

## Componentes Principales

### 1. Configuración OpenIddict
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

### 2. Seguridad y Validación de Tokens
- **Issuer**: `https://localhost:6001/`
- **Audience**: `resource_server`
- **Encryption Key**: Clave simétrica compartida con AuthServer
- **Validación automatizada de**:
  - Firma del token
  - Tiempo de expiración
  - Audiencia válida
  - Issuer correcto

### 3. Configuración CORS
- Orígenes permitidos configurados en appsettings:
  - Por defecto: `http://localhost:7000`
- Headers: Permite cualquier header
- Métodos: Permite cualquier método HTTP

### 4. Autenticación y Autorización
- Esquema: OpenIddictValidationAspNetCoreDefaults
- Middleware configurado en pipeline:
  ```csharp
  app.UseAuthentication();
  app.UseAuthorization();
  ```
- Protección de endpoints mediante atributo [Authorize]

### 5. Flujo de Solicitudes
1. Cliente envía request con Bearer token
2. Middleware valida el token con OpenIddict
3. Si es válido, se establece ClaimsPrincipal
4. Se ejecuta el endpoint si autorización es correcta

### 6. Configuración del Entorno
- Desarrollo:
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

## Aspectos de Seguridad
- HTTPS obligatorio en producción
- Validación completa de tokens JWT
- CORS configurado para orígenes específicos
- Claves de encriptación gestionadas por configuración
- Claims del usuario disponibles vía ClaimsPrincipal

## Endpoints Protegidos
Todos los endpoints bajo `/api` requieren autenticación válida mediante token JWT.
Ejemplo:
```csharp
[Authorize]
[Route("api/[controller]")]
public class ResourceController : ControllerBase
{
    // Endpoints protegidos
}
```