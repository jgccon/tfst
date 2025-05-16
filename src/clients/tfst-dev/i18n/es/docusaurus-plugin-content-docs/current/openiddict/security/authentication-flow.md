---
id: authentication-flow
title: Authentication Flow
---

# Flujo de autenticación con el cliente OIDC TS

## 1. Configuración del cliente
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

## 1.1. Descubrimiento de metadatos del proveedor (Descubrimiento OIDC)
Antes de iniciar cualquier flujo de autenticación, `oidc-client-ts` utiliza la `authority` configurada (`https://localhost:6001`) para descubrir automáticamente las funciones y los endpoints del servidor de autenticación. Para ello, consulta el punto final de descubrimiento de metadatos, que normalmente se encuentra en:

`https://localhost:6001/.well-known/openid-configuration`

Este punto final devuelve un documento JSON que contiene, entre otros datos:

- Las URL exactas de los puntos finales de autorización (`authorization_endpoint`), token (`token_endpoint`), información de usuario (`userinfo_endpoint`), etc.
- Los métodos de firma compatibles con los tokens de identificación.
- Los ámbitos compatibles.
- La ubicación de las claves públicas del servidor (punto final JWKS).

`oidc-client-ts` utiliza esta información para configurar internamente las URL correctas y validar los tokens recibidos, lo que simplifica la configuración del cliente y reduce la probabilidad de errores si las URL cambian en el servidor (siempre que la `authority` base se mantenga).

## 2. Flujo de autenticación

### 2.1. Inicio del flujo (Inicio de sesión)
1. El usuario hace clic en "Iniciar sesión".
2. Se ejecuta `userManager.signinRedirect()`.
3. El cliente OIDC TS automáticamente:
- Genera code_verifier y code_challenge:
- `code_verifier`: cadena aleatoria
- `code_challenge`: hash SHA256 de `code_verifier` (enviado a AuthServer).
- Almacena el estado y el nonce:
- `state`: cadena aleatoria para validar la respuesta.
- `nonce`: cadena aleatoria para proteger contra ataques de repetición.
4. Redirecciona a AuthServer.

### 2.2. Proceso en AuthServer
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
1. AuthServer autentica al usuario, obtiene su consentimiento (si es necesario) y, si todo es correcto, genera un código de autorización.
2. Redirige el navegador al `redirect_uri` del cliente, incluyendo el código de autorización y el estado original en los parámetros de la URL.
```http
GET /signin-callback.html?code=xxxxx&state=xxxxx
```

### 2.3. Devolución de llamada y recuperación de tokens
1. El cliente (página `signin-callback.html`) recibe el código de autorización y el estado.
2. El script en `signin-callback.html` ejecuta `await``userManager.signinCallback();`.
3. El cliente OIDC TS automáticamente:
- Recupera el `code_verifier` almacenado.
- Envía una solicitud POST al punto final del token del servidor de autenticación (`/connect/token`) para intercambiar el código de autorización por tokens. ```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=authorization_code
&client_id=tfst_clientwebapp // Se incluye el client_id, pero NO un client_secret
&code=xxxxx
&redirect_uri=http://localhost:7000/signin-callback.html
&code_verifier=xxxxx // Se incluye el code_verifier
```
4. El AuthServer recibe la solicitud:
- Verifica el `client_id` y el `redirect_uri`.
- Busca el código de autorización recibido y el `code_challenge` asociado que se envió en el paso de autorización.
- Aplica la función hash al `code_verifier` recibido y verifica que el resultado coincida con el `code_challenge` almacenado.
- Si se superan todas las validaciones, el AuthServer emite los tokens (token de acceso, token de ID y token de actualización si se solicita).

5. El AuthServer responde al cliente con los tokens (normalmente en formato JSON):
```json
{
"access_token": "xxxxx",
"refresh_token": "xxxxx", // Presente si se solicitó el ámbito 'offline_access'
"id_token": "xxxxx", // Presente si se solicitó el ámbito 'openid'
"expires_in": 3600,
"token_type": "Bearer"
}
```
6. El cliente OIDC TS realiza automáticamente lo siguiente:
- Valida el `id_token` (firma, nonce, notificaciones).
- Almacena los tokens recibidos de forma segura (normalmente en `sessionStorage` o `localStorage`, configurados en `userManager`).
- Carga la información del usuario desde el `id_token` o mediante una solicitud al endpoint `userinfo` (si es necesario y está configurado).
- Completa el proceso `signinCallback()`.

7. La aplicación cliente ahora puede considerar al usuario autenticado y usar los tokens. Normalmente, la página `signin-callback.html` redirigirá al usuario a la página principal de la aplicación (p. ej., `index.html`).

### 2.4. Uso de tokens
1. Obtener usuario y token:
```javascript
const user = await userManager.getUser();
// Usar access_token para llamadas a la API
fetch(url, {
headers: {
'Authorization': `Bearer ${user.access_token}`
}
});
```

### 2.5. Renovación automática
1. automaticSilentRenew: true habilita:
- Renovación automática antes del vencimiento
- Uso de refresh_token sin intervención
- Actualización del token en memoria

### 2.6. Cerrar sesión
1. El usuario hace clic en "Cerrar sesión".
2. Se ejecuta `userManager.signoutRedirect()`.
3. Se realiza la limpieza de la sesión local y el cierre de sesión en AuthServer.
4. Se redirige a la página de cierre de sesión.

## 3. Eventos y estado
```javascript
userManager.events.addUserLoaded(user => {
// Usuario autenticado
});
userManager.events.addUserUnloaded(() => {
// Usuario desconectado
});
```

## 4. Seguridad implementada
- PKCE automático
- Validación de estado/nonce
- Almacenamiento seguro de tokens
- Renovación silenciosa de tokens
- Gestión de errores de autenticación

## 5. Ventajas de OIDC Client TS
- Implementación estándar de OAuth 2.0/OIDC
- Gestión automática de PKCE
- Gestión transparente de tokens
- Renovación automática de sesiones
- Eventos para la sincronización de la interfaz de usuario