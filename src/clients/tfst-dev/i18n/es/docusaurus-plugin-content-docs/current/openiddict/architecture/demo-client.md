---
id: demo-client
title: Demo Client
---

# Aplicación Cliente de Demostración

## Introducción

El cliente de demostración (tfst-demo) es una aplicación web sencilla que implementa la autenticación OpenID Connect mediante la biblioteca oidc-client-ts. Interactúa con TFST.AuthServer para la autenticación y con TFST.API para acceder a recursos protegidos.

## Función en la Arquitectura

El cliente de demostración actúa como un consumidor de API, permitiendo a los usuarios realizar operaciones que requieren autenticación. Se comunica con el servidor de autenticación para obtener tokens de acceso y los utiliza para acceder a recursos protegidos en la API.

## Comunicación con la API

1. **Autenticación**: El cliente envía las credenciales del usuario al servidor de autenticación. Si las credenciales son válidas, el servidor devuelve un token de acceso.

2. **Acceso a Recursos**: Con el token de acceso, el cliente puede realizar solicitudes a la API. El token se incluye en el encabezado de autorización de cada solicitud.

3. **Gestión de Errores**: El cliente debe gestionar los errores de autenticación y autorización, proporcionando mensajes claros al usuario en caso de problemas.

## Configuración

Para configurar el cliente de demostración, asegúrese de que los siguientes parámetros estén configurados correctamente:

- **URL del servidor de autenticación**: La dirección del servidor de autenticación TFST.AuthServer.
- **URL de la API**: La dirección de la API de TFST que el cliente utilizará para realizar solicitudes.

## Componentes principales

### 1. Configuración de OIDC

```javascript
const userManager = new oidc.UserManager({
authority: 'https://localhost:6001', // URL del servidor de autenticación
client_id: 'tfst_clientwebapp', // ID del cliente registrado
response_type: 'code', // Flujo del código de autorización
scope: 'openid profile email roles offline_access TFST_API', // Ámbitos solicitados
redirect_uri: `${window.location.origin}/signin-callback.html`,
post_logout_redirect_uri: `${window.location.origin}/index.html`,
automaticSilentRenew: true, // Renovación automática del token
cludeIdTokenInSilentRenew: true
}); ```

### 2. Flujo de autenticación PKCE

El flujo PKCE (Clave de prueba para intercambio de código) es gestionado automáticamente por oidc-client-ts:

1. **Inicio de sesión**:
```javascript
async function login() {
try {
// Genera automáticamente code_verifier y code_challenge
await userManager.signinRedirect();
} catch (error) {
console.error('Error durante el inicio de sesión:', error);
}
}
```

2. **Devolución de llamada de autenticación** (signin-callback.html):
```javascript
// Procesa la respuesta de autenticación y valida code_verifier
await userManager.signinCallback();
```

### 3. Gestión de tokens

```javascript
// Obtiene el usuario actual y los tokens
const user = await userManager.getUser(); if (usuario) {
console.log('Token de acceso:', user.access_token);
console.log('Token de ID:', user.id_token);
console.log('Actualizar token:', user.refresh_token);
}

// Renovar tokens
función asíncrona refreshToken() {
try {
await userManager.signinSilent();
console.log('Token renovado correctamente');
} catch (error) {
console.error('Error al renovar el token:', error);
}
}
```

### 4. Llamadas a la API

```javascript
función asíncrona callApi() {
const user = await userManager.getUser();
if (!usuario) throw new Error('No autenticado');

const response = await fetch('https://localhost:5001/api', {
headers: {
'Autorización': `Portador ${user.access_token}`
}
});

if (!response.ok) throw new Error(`HTTP Error: ${response.status}`);
return await response.text();
}
```

### 5. Cerrar sesión

```javascript
async function logout() {
await userManager.signoutRedirect();
}
```

## Gestión de eventos y estados

```javascript
userManager.events.addUserLoaded(user => {
console.log('Usuario cargado:', usuario);
});

userManager.events.addSilentRenewError(error => {
console.error('Error de renovación silenciosa:', error);
});

userManager.events.addAccessTokenExpiring(() => {
console.log('Token a punto de expirar');
});
```

## Flujo de autenticación completo

1. El usuario hace clic en "Iniciar sesión"
2. userManager.signinRedirect():
- Genera code_verifier y code_challenge
- Redirecciona a AuthServer
3. El usuario se autentica en AuthServer
4. AuthServer redirige a signin-callback.html
5. userManager.signinCallback():
- Valida la respuesta
- Almacena tokens
- Redirecciona a la aplicación

## Seguridad

- PKCE protege contra ataques de interceptación de código de autorización
- Los tokens se almacenan en memoria
- Renovación automática de tokens configurada
- Validación de audiencia y emisor en tokens

## Configuración requerida en AuthServer

```json
{
"AuthServer": {
"TfstApp": {
"ClientId": "tfst_clientwebapp",
"RedirectUris": [
"http://localhost:7000/signin-callback.html"
],
"PostLogoutRedirectUris": "http://localhost:7000/index.html"
}
}
}
```

## Buenas prácticas

1. Gestionar adecuadamente los errores de autenticación y API
2. Implementar interceptores para la renovación automática de tokens
3. Validar el estado de la autenticación antes de las llamadas a la API
4. Usar HTTPS en producción
5. Mantener las URL de redireccionamiento actualizadas