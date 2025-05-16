---
id: auth-server
title: Auth Server
---

# TFST.AuthServer

## Propósito
TFST.AuthServer es el servidor de autenticación/autorización que implementa OpenID Connect y OAuth 2.0 mediante OpenIddict. Gestiona la autenticación de usuarios y emite tokens JWT para acceder a recursos protegidos.

## Componentes principales

### 1. Configuración de OpenIddict
```csharp
builder.Services.AddOpenIddict()
.AddServer(options =>
{
// Puntos de conexión
options.SetAuthorizationEndpointUris("connect/authorize")
.SetTokenEndpointUris("connect/token")
.SetUserInfoEndpointUris("connect/userinfo");

// Flujos permitidos
options.AllowAuthorizationCodeFlow()
.AllowRefreshTokenFlow()
.RequireProofKeyForCodeExchange(); // Se requiere PKCE
```
### 2. Ámbitos compatibles
- Ámbitos estándar:
- `openid`: Autenticación de OpenID Connect
- `profile`: Información básica del usuario
- `email`: Dirección de correo electrónico
- `roles`: Roles de usuario

- Ámbitos personalizados:
- `TFST_API`: Ámbito personalizado que otorga acceso a TFST.API al configurar la audiencia del token con `resource_server`.

```json
"ApiScopes": [
{
"Name": "TFST_API",
"Resource": "resource_server"
}
]
```

### 3. Seguridad y tokens
- PKCE (Clave de prueba para el intercambio de código):
- Requerida para el flujo de código de autorización
- Protege contra ataques de interceptación
- Implementada automáticamente por OpenIddict

- Emisión de tokens:
```csharp
// JWT con notificaciones de usuario
identity.SetClaim(Claims.Subject, userId)
.SetClaim(Claims.Email, email)
.SetClaim(Claims.Name, username)
.SetClaims(Claims.Role, roles);
```

- Validación:
- Emisor: `https://localhost:6001/`
- Público: `resource_server`
- Clave de cifrado: Clave simétrica configurada

### 4. Clientes registrados
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
- Base de datos de SQL Server con esquema `auth`
- Principal Tablas:
- `Usuarios`: Usuarios y credenciales
- `AplicacionesOpenIddict`: Clientes registrados
- `AutorizacionesOpenIddict`: Autorizaciones
- `TokensOpenIddict`: Tokens emitidos
- `ÁmbitosOpenIddict`: Ámbitos admitidos

### 6. Proceso de autenticación
1. El cliente solicita autorización con PKCE
2. El usuario se autentica (si es necesario)
3. El servidor de autenticación valida las credenciales
4. Se emite el código de autorización
5. El cliente intercambia el código por tokens
6. Se emiten los tokens de acceso y de actualización

### 7. Tokens de actualización
- Duración configurable (14 días por defecto)
- Almacenados en `TokensOpenIddict`
- Rotación automática al usar
- Validación activa del usuario en cada uso

## Aspectos de seguridad
- HTTPS requerido en producción
- Tokens cifrados con clave simétrica
- PKCE requerido para Clientes públicos
- Validación de Redirect_uri
- Almacenamiento seguro de secretos
- Limitación de velocidad en endpoints críticos

## Endpoints principales
- `/connect/authorize`: Inicio del flujo de autorización
- `/connect/token`: Emisión y actualización de tokens
- `/connect/userinfo`: Información del usuario
- `/Account/Login`: Interfaz de inicio de sesión
- `/Account/Register`: Interfaz de registro