---
id: authorization
title: Authorization
---
# Flujo de Autorización (Acceso a Recursos Protegidos)

Este flujo describe cómo la aplicación cliente (SPA JavaScript) utiliza los tokens obtenidos en el flujo de autenticación OIDC (específicamente, el `access_token`) para acceder a la API protegida.

Asumimos que el usuario ha completado correctamente el flujo de autenticación y que el cliente ha obtenido y almacenado un `access_token` válido y vigente (esto lo gestiona automáticamente `oidc-client-ts`).

## 1. Preparación de la Solicitud en el Cliente

1. El cliente, la aplicación JavaScript (SPA), debe realizar una llamada a un endpoint protegido en la API (el Servidor de Recursos).
2. Antes de enviar la solicitud HTTP (GET, POST, PUT, DELETE, etc.) al endpoint de la API, el cliente recupera el `access_token` almacenado localmente. Esto se realiza generalmente llamando a `userManager.getUser()` después de que el usuario se haya autenticado y cargado. 3. El cliente incluye el `access_token` en la cabecera HTTP `Authorization`. El formato estándar es usar el esquema `Bearer` seguido de un espacio y el token.

Ejemplo de cómo se vería la solicitud HTTP saliente del navegador (usando la API Fetch):

```javascript
const user = await userManager.getUser(); // Obtener el usuario almacenado y sus tokens

if (user && user.access_token) {
const url = 'https://localhost:5001/api/protecteddata'; // URL del punto final de la API protegida

fetch(url, {
method: 'GET', // O POST, PUT, DELETE, etc.
headers: {
'Authorization': `Bearer ${user.access_token}`, // Incluir token de acceso
'Content-Type': 'application/json' // Si se envía un cuerpo JSON
}
})
.then(response => {
if (!response.ok) {
// Gestionar errores (p. ej., 401 No autorizado, 403 Prohibido)
console.error('Error al acceder a la API:', response.status);
if (response.status === 401) {
// El token podría haber expirado o no ser válido.
// oidc-client-ts con automaticSilentRenew debería gestionar esto,
// pero si falla, podría ser necesario redirigir al inicio de sesión.
}
throw new Error(`Error HTTP! estado: ${response.status}`);
}
devuelve response.json(); // O response.text() si no es JSON
})
.then(data => {
console.log('Datos de la API:', data);
// Procesar los datos recibidos de la API
})
.catch(error => {
console.error('Hubo un problema con la solicitud de obtención:', error);
// Gestionar errores de red u otros errores
});
} else {
console.warn('Usuario no autenticado o token de acceso no disponible.');
// Podría redirigir al inicio de sesión si no hay usuario
// userManager.signinRedirect();
}
```

La parte crucial es la cabecera `Authorization: Bearer [obtained_access_token]`.

## 2. Recepción y validación en la API (Servidor de recursos)

1. La API (`TFST.API`) recibe la solicitud HTTP entrante.
2. El middleware de autenticación y autorización configurado en la API intercepta la solicitud **antes** de que llegue a la lógica de negocio (controladores/endpoints específicos).
3. Este middleware:
* Extrae el token del encabezado `Authorization`.
* Realiza la validación `access_token`. Para un JWT (JSON Web Token) emitido por OpenIddict, esto generalmente implica:
* Verificar la **firma** del token utilizando la clave pública o el certificado de AuthServer OpenIddict para garantizar que el token no haya sido manipulado.
* Verificar el **emisor** (notificación `iss`) para garantizar que el token provenga del AuthServer esperado. * Verificar la **audiencia** (declaración `aud`) para garantizar que el token esté destinado a esta API específica (la declaración `aud` del token debe coincidir con un identificador configurado para la API como recurso en OpenIddict).
* Verificar la **fecha de caducidad** (declaración `exp`) para garantizar que el token siga siendo válido.
* Otras validaciones como `nbf` (Not Before) e `iat` (Issed At).
* Si el token es válido, el middleware autentica al usuario asociado con él en el contexto de la solicitud HTTP actual. Las declaraciones del token (como `sub`, `name`, `email`, `roles`, etc.) se convierten en las identidades y los principales del usuario, disponibles para la lógica de negocio de la API.
4. Tras la autenticación (validación del token), el middleware de autorización de la API verifica si el usuario autenticado (según las declaraciones del token) tiene los permisos necesarios para acceder al endpoint solicitado. Esto podría basarse en ámbitos, roles o políticas de autorización más granulares definidas en la API (p. ej., usando los atributos `[Authorize(Roles = "Admin")]` o `[Authorize(Policy = "RequiresApiAccess")]` en los controladores).

## 3. Resultado de la autorización

1. **Si el token es válido y el usuario está autorizado:**
* La solicitud supera las comprobaciones de autenticación y autorización.
* La solicitud llega al controlador/método de acción de la API.
* La lógica de negocio de la API se ejecuta, procesa la solicitud y devuelve los datos o la respuesta correspondientes al cliente (p. ej., estado HTTP 200 OK).

2. **Si el token no es válido o el usuario no está autorizado:**
* El middleware de autenticación o autorización intercepta la solicitud.
* La API rechaza la solicitud antes de que llegue a la lógica de negocio. * La API devuelve un código de estado HTTP de error al cliente:
* `401 No autorizado`: Generalmente indica que la autenticación falló (el token falta, no es válido, ha caducado, la firma es incorrecta, etc.).
* `403 Prohibido`: Generalmente indica que el usuario está autenticado (el token es válido), pero no tiene los permisos necesarios (roles/ámbitos/reclamos) para acceder a este recurso en particular.

## 4. Renovación y reintento (Gestionado por el cliente OIDC TS)

* Si el cliente recibe un `401 No autorizado` (por ejemplo, porque el `access_token` acaba de caducar), `oidc-client-ts` (si `automaticSilentRenew` está habilitado) intentará renovar automáticamente el `access_token` en segundo plano utilizando el `refresh_token` (si se obtuvo).
* Si la renovación se realiza correctamente, `userManager` se actualizará con el nuevo `access_token`. La aplicación cliente puede entonces reintentar la llamada a la API con el nuevo token.
* Si la renovación falla (por ejemplo, si el token `refresh_token` ha expirado o ha sido revocado), `oidc-client-ts` puede activar un evento (`accessTokenExpiring`, `accessTokenExpired`) y la aplicación cliente deberá redirigir al usuario para que se vuelva a autenticar por completo (regrese al paso 2.1 del flujo de autenticación).

Este flujo de autorización es la forma estándar en que las SPA acceden a las API protegidas mediante JWT emitidos por un servidor OAuth 2.0/OIDC.