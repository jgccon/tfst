# Demo Client Application

## Introducción

El cliente de demostración (tfst-demo) es una aplicación web simple que implementa autenticación OpenID Connect utilizando la biblioteca oidc-client-ts. Interactúa con TFST.AuthServer para autenticación y TFST.API para acceder a recursos protegidos.

## Rol en la Arquitectura

El cliente de demostración actúa como un consumidor de la API, permitiendo a los usuarios realizar operaciones que requieren autenticación. Se comunica con el servidor de autenticación para obtener tokens de acceso y utiliza estos tokens para acceder a los recursos protegidos en la API.

## Comunicación con la API

1. **Autenticación**: El cliente envía las credenciales del usuario al servidor de autenticación. Si las credenciales son válidas, el servidor devuelve un token de acceso.
   
2. **Acceso a Recursos**: Con el token de acceso, el cliente puede realizar solicitudes a la API. El token se incluye en el encabezado de autorización de cada solicitud.

3. **Manejo de Errores**: El cliente debe manejar errores de autenticación y autorización, proporcionando mensajes claros al usuario en caso de que se produzcan problemas.

## Configuración

Para configurar el cliente de demostración, asegúrese de que los siguientes parámetros estén correctamente establecidos:

- **URL del Servidor de Autenticación**: La dirección del servidor de autenticación TFST.AuthServer.
- **URL de la API**: La dirección de la API de TFST que el cliente utilizará para realizar solicitudes.

## Componentes Principales

### 1. Configuración de OIDC

```javascript
const userManager = new oidc.UserManager({
    authority: 'https://localhost:6001',                    // URL del AuthServer
    client_id: 'tfst_clientwebapp',                        // ID del cliente registrado
    response_type: 'code',                                 // Flujo de código de autorización
    scope: 'openid profile email roles offline_access TFST_API', // Scopes solicitados
    redirect_uri: `${window.location.origin}/signin-callback.html`,
    post_logout_redirect_uri: `${window.location.origin}/index.html`,
    automaticSilentRenew: true,                           // Renovación automática de tokens
    includeIdTokenInSilentRenew: true
});
```

### 2. Flujo de Autenticación PKCE

El flujo PKCE (Proof Key for Code Exchange) se maneja automáticamente por oidc-client-ts:

1. **Inicio de Sesión**:
```javascript
async function login() {
    try {
        // Genera automáticamente code_verifier y code_challenge
        await userManager.signinRedirect();
    } catch (error) {
        console.error('Error durante el login:', error);
    }
}
```

2. **Callback de Autenticación** (signin-callback.html):
```javascript
// Procesa la respuesta de autenticación y valida el code_verifier
await userManager.signinCallback();
```

### 3. Gestión de Tokens

```javascript
// Obtener usuario y tokens actuales
const user = await userManager.getUser();
if (user) {
    console.log('Access Token:', user.access_token);
    console.log('ID Token:', user.id_token);
    console.log('Refresh Token:', user.refresh_token);
}

// Renovar tokens
async function refreshToken() {
    try {
        await userManager.signinSilent();
        console.log('Token renovado exitosamente');
    } catch (error) {
        console.error('Error al renovar el token:', error);
    }
}
```

### 4. Llamadas a la API

```javascript
async function callApi() {
    const user = await userManager.getUser();
    if (!user) throw new Error('No autenticado');

    const response = await fetch('https://localhost:5001/api', {
        headers: {
            'Authorization': `Bearer ${user.access_token}`
        }
    });

    if (!response.ok) throw new Error(`Error HTTP: ${response.status}`);
    return await response.text();
}
```

### 5. Cierre de Sesión

```javascript
async function logout() {
    await userManager.signoutRedirect();
}
```

## Eventos y Manejo del Estado

```javascript
userManager.events.addUserLoaded(user => {
    console.log('Usuario cargado:', user);
});

userManager.events.addSilentRenewError(error => {
    console.error('Error en renovación silenciosa:', error);
});

userManager.events.addAccessTokenExpiring(() => {
    console.log('Token a punto de expirar');
});
```

## Flujo Completo de Autenticación

1. Usuario hace clic en "Login"
2. userManager.signinRedirect():
   - Genera code_verifier y code_challenge
   - Redirecciona al AuthServer
3. Usuario se autentica en AuthServer
4. AuthServer redirecciona a signin-callback.html
5. userManager.signinCallback():
   - Valida la respuesta
   - Almacena tokens
   - Redirecciona a la aplicación

## Seguridad

- PKCE protege contra ataques de intercepción de código de autorización
- Los tokens se almacenan en memoria
- Renovación automática de tokens configurada
- Validación de audiencia y emisor en tokens

## Configuración Requerida en AuthServer

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

## Buenas Prácticas

1. Manejar errores de autenticación y API apropiadamente
2. Implementar interceptores para renovación automática de tokens
3. Validar estado de autenticación antes de llamadas a la API
4. Utilizar HTTPS en producción
5. Mantener las URLs de redirección actualizadas