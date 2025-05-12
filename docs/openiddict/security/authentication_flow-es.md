# Flujo de Autenticación con OIDC Client TS

## 1. Configuración del Cliente
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
## 1.1.Descubrimiento de Metadatos del Proveedor (OIDC Discovery)
Antes de iniciar cualquier flujo de autenticación, `oidc-client-ts` utiliza la `authority` configurada (`https://localhost:6001`) para descubrir automáticamente las características y endpoints del AuthServer. Esto lo hace consultando el endpoint de descubrimiento de metadatos, típicamente ubicado en:

`https://localhost:6001/.well-known/openid-configuration`

Este endpoint devuelve un documento JSON que contiene, entre otras cosas:

- Las URLs exactas para los endpoints de autorización (`authorization_endpoint`), token (`token_endpoint`), userinfo (`userinfo_endpoint`), etc.
- Los métodos de firma soportados para los ID Tokens.
- Los scopes soportados.
- La ubicación de las claves públicas del servidor (JWKS endpoint).

`oidc-client-ts` utiliza esta información para configurar internamente las URLs correctas y validar los tokens recibidos, haciendo que la configuración en el cliente sea más simple y menos propensa a errores si las URLs cambian en el servidor (siempre que el `authority` base se mantenga).

## 2. Flujo de Autenticación

### 2.1. Inicio del Flujo (Login)
1. Usuario hace clic en "Login"
2. Se ejecuta `userManager.signinRedirect()`
3. OIDC Client TS automáticamente:
   - Genera code_verifier y code_challenge: 
     - `code_verifier`: cadena aleatoria
     - `code_challenge`: hash SHA256 de `code_verifier` (enviada al AuthServer)
   - Almacena el state y nonce:
     - `state`: cadena aleatoria para validar la respuesta
     - `nonce`: cadena aleatoria para proteger contra ataques de repetición
4. Redirige al AuthServer

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
1. El AuthServer autentica al usuario, obtiene su consentimiento (si es necesario) y, si todo es correcto, genera un código de autorización.
2. Redirige al navegador de vuelta a la `redirect_uri` del cliente, incluyendo el código de autorización y el state original en los parámetros de la URL.
```http
GET /signin-callback.html?code=xxxxx&state=xxxxx
```

### 2.3. Callback y Obtención de Tokens
1. El cliente (la página `signin-callback.html`) recibe el código de autorización y el state.
2. El script en `signin-callback.html` ejecuta await `userManager.signinCallback();`.
3. OIDC Client TS automáticamente:
   - Recupera el `code_verifier` almacenado localmente.
   - Envía una solicitud POST al token endpoint (`/connect/token`) del AuthServer para intercambiar el código de autorización por tokens.
```http
POST /connect/token
    Content-Type: application/x-www-form-urlencoded

    grant_type=authorization_code
    &client_id=tfst_clientwebapp  // Se incluye el client_id, pero NO un client_secret
    &code=xxxxx
    &redirect_uri=http://localhost:7000/signin-callback.html
    &code_verifier=xxxxx // Se incluye el code_verifier
```
4. El AuthServer recibe la solicitud:
   - Verifica el `client_id` y la `redirect_uri`.
   - Busca el código de autorización recibido y el `code_challenge` asociado que se envió en el paso de autorización.
   - Aplica la función de hash al `code_verifier` recibido y verifica que el resultado coincida con el `code_challenge` almacenado.
   - Si todas las validaciones pasan, el AuthServer emite los tokens (Access Token, ID Token, Refresh Token si se solicitó).

5. El AuthServer responde al cliente con los tokens (usualmente en formato JSON):
```json
{
    "access_token": "xxxxx",
    "refresh_token": "xxxxx", // Presente si se solicitó el scope 'offline_access'
    "id_token": "xxxxx",      // Presente si se solicitó el scope 'openid'
    "expires_in": 3600,
    "token_type": "Bearer"
}
```
6. OIDC Client TS automáticamente:
   - Valida el `id_token` (firma, nonce, claims).
   - Almacena los tokens recibidos de forma segura (típicamente en `sessionStorage` o `localStorage`, configurado en el `userManager`).
   - Carga la información del usuario a partir del `id_token` y/o haciendo una solicitud al `userinfo` endpoint (si es necesario y configurado).
   - Completa el proceso de `signinCallback()`.

7. La aplicación cliente puede ahora considerar al usuario autenticado y utilizar los tokens. Típicamente, la página `signin-callback.html` redirigirá al usuario a la página principal de la aplicación (por ejemplo, `index.html`).

### 2.4. Uso de Tokens
1. Obtener usuario y token:
   ```javascript
   const user = await userManager.getUser();
   // Usar access_token para llamadas API
   fetch(url, {
       headers: {
           'Authorization': `Bearer ${user.access_token}`
       }
   });
   ```

### 2.5. Renovación Automática
1. automaticSilentRenew: true habilita:
   - Renovación automática antes de expiración
   - Uso de refresh_token sin intervención
   - Actualización de tokens en memoria

### 2.6. Cierre de Sesión
1. Usuario hace clic en "Logout"
2. Se ejecuta `userManager.signoutRedirect()`
3. Limpieza de sesión local y en AuthServer se realiza el logout
4. Redirección a la página de cierre de sesión

## 3. Eventos y Estado
```javascript
userManager.events.addUserLoaded(user => {
    // Usuario autenticado
});
userManager.events.addUserUnloaded(() => {
    // Usuario desconectado
});
```

## 4. Seguridad Implementada
- PKCE automático
- Validación de state/nonce
- Almacenamiento seguro de tokens
- Renovación silenciosa de tokens
- Manejo de errores de autenticación

## 5. Ventajas de OIDC Client TS
- Implementación estándar de OAuth 2.0/OIDC
- Manejo automático de PKCE
- Gestión de tokens transparente
- Renovación automática de sesión
- Eventos para sincronización de UI