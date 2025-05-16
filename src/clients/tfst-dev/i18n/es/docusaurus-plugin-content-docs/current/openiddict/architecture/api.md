---
id: api
title: Api
---

# TFST.API

## Propósito
TFST.API actúa como un servidor de recursos que protege los endpoints mediante tokens JWT emitidos por TFST.AuthServer. Implementa OpenIddict.Validation para la validación de tokens y la gestión de autorizaciones.

## Componentes principales

### 1. Configuración de OpenIddict
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

### 2. Seguridad y validación de tokens
- **Emisor**: `https://localhost:6001/`
- **Audiencia**: `resource_server`
- **Clave de cifrado**: Clave simétrica compartida con AuthServer
- **Validación automatizada de**:
- Firma del token
- Tiempo de expiración
- Audiencia válida
- Emisor correcto

### 3. Configuración de CORS
- Orígenes permitidos configurados en la configuración de la aplicación:
- Predeterminado: `http://localhost:7000`
- Encabezados: Permite cualquier encabezado
- Métodos: Permite cualquier método HTTP

### 4. Autenticación y autorización
- Esquema: OpenIddictValidationAspNetCoreDefaults
- Middleware configurado en la canalización:
```csharp
app.UseAuthentication();
app.UseAuthorization(); ```
- Protección de endpoints mediante el atributo [Authorize]

### 5. Flujo de solicitud
1. El cliente envía la solicitud con el token de portador
2. El middleware valida el token con OpenIddict
3. Si es válido, se establece ClaimsPrincipal
4. El endpoint se ejecuta si la autorización es correcta

### 6. Configuración del entorno
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

## Aspectos de seguridad
- HTTPS obligatorio en producción
- Validación completa de tokens JWT
- CORS configurado para orígenes específicos
- Cifrado Claves administradas por configuración
- Reclamos de usuario disponibles a través de ClaimsPrincipal

## Puntos finales protegidos
Todos los puntos finales bajo `/api` requieren autenticación válida mediante token JWT.
Ejemplo:
```csharp
[Authorize]
[Route("api/[controller]")]
public class ResourceController : ControllerBase
{
// Puntos finales protegidos
}
```