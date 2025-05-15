# Flujo de Autorización (Acceso a Recursos Protegidos)

Este flujo describe cómo la aplicación cliente (la SPA JavaScript) utiliza los tokens obtenidos en el flujo de autenticación OIDC (específicamente el `access_token`) para acceder a la API protegida.

Asumimos que el usuario ya ha completado exitosamente el flujo de autenticación y que el cliente ha obtenido y almacenado un `access_token` válido y no expirado (esto es manejado automáticamente por `oidc-client-ts`).

## 1. Preparación de la Solicitud en el Cliente

1.  El cliente, la aplicación JavaScript (SPA) necesita hacer una llamada a un endpoint protegido en la API (el Resource Server).
2.  Antes de enviar la solicitud HTTP (GET, POST, PUT, DELETE, etc.) al endpoint de la API, el cliente recupera el `access_token` almacenado localmente. Esto se hace usualmente llamando a `userManager.getUser()` después de que el usuario se ha autenticado y cargado.
3.  El cliente incluye el `access_token` en el encabezado HTTP `Authorization`. El formato estándar es usar el esquema `Bearer` seguido de un espacio y el token.

    Ejemplo de cómo se vería la solicitud HTTP saliente desde el navegador (usando Fetch API):

    ```javascript
    const user = await userManager.getUser(); // Obtener el usuario y sus tokens almacenados

    if (user && user.access_token) {
        const url = 'https://localhost:5001/api/datosprotegidos'; // URL del endpoint protegido en la API

        fetch(url, {
            method: 'GET', // O POST, PUT, DELETE, etc.
            headers: {
                'Authorization': `Bearer ${user.access_token}`, // Incluir el Access Token
                'Content-Type': 'application/json' // Si se envía un cuerpo JSON
            }
        })
        .then(response => {
            if (!response.ok) {
                // Manejar errores (ej. 401 Unauthorized, 403 Forbidden)
                console.error('Error al acceder a la API:', response.status);
                if (response.status === 401) {
                    // El token puede haber expirado o ser inválido.
                    // oidc-client-ts con automaticSilentRenew debería manejar esto,
                    // pero si falla, podría ser necesario redirigir al login.
                }
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json(); // O response.text() si no es JSON
        })
        .then(data => {
            console.log('Datos de la API:', data);
            // Procesar los datos recibidos de la API
        })
        .catch(error => {
            console.error('Hubo un problema con la solicitud fetch:', error);
            // Manejar errores de red u otros errores
        });
    } else {
        console.warn('Usuario no autenticado o access_token no disponible.');
        // Quizás redirigir al login si no hay usuario
        // userManager.signinRedirect();
    }
    ```

    Lo crucial es el encabezado `Authorization: Bearer [access_token_obtenido]`.

## 2. Recepción y Validación en la API (Resource Server)

1.  La API (`TFST.API`) recibe la solicitud HTTP entrante.
2.  El middleware de autenticación y autorización configurado en la API intercepta la solicitud **antes** de que llegue a la lógica de negocio (los controladores/endpoints específicos).
3.  Este middleware:
    * Extrae el token del encabezado `Authorization`.
    * Realiza la validación del `access_token`. Para un JWT (JSON Web Token) emitido por OpenIddict, esto típicamente implica:
        * Verificar la **firma** del token utilizando la clave pública o el certificado del AuthServer OpenIddict para asegurar que el token no ha sido manipulado.
        * Verificar el **emisor** (`iss` claim) para asegurarse de que el token proviene del AuthServer esperado.
        * Verificar la **audiencia** (`aud` claim) para asegurarse de que el token está destinado a esta API específica (el `aud` claim en el token debe coincidir con un identificador configurado para la API como recurso en OpenIddict).
        * Verificar la **fecha de expiración** (`exp` claim) para asegurarse de que el token aún es válido.
        * Otras validaciones como `nbf` (Not Before) y `iat` (Issued At).
    * Si el token es válido, el middleware autentica al usuario asociado con el token dentro del contexto de la solicitud HTTP actual. Los claims del token (como `sub`, `name`, `email`, `roles`, etc.) se convierten en las identidades y principios del usuario disponibles para la lógica de negocio de la API.
4.  Después de la autenticación (validación del token), el middleware de autorización de la API verifica si el usuario autenticado (basado en los claims del token) tiene los permisos necesarios para acceder al endpoint solicitado. Esto podría basarse en `scopes`, `roles`, o políticas de autorización más granulares definidas en la API (ej. usando atributos `[Authorize(Roles = "Admin")]` o `[Authorize(Policy = "RequiresApiAccess")]` en los controladores).

## 3. Resultado de la Autorización

1.  **Si el Token es Válido y el Usuario está Autorizado:**
    * La solicitud pasa las comprobaciones de autenticación y autorización.
    * El request llega al controlador/método de acción de la API.
    * La lógica de negocio de la API se ejecuta, procesa la solicitud y devuelve los datos o la respuesta apropiada al cliente (ej. estado HTTP 200 OK).
2.  **Si el Token es Inválido o el Usuario No está Autorizado:**
    * El middleware de autenticación o autorización intercepta la solicitud.
    * La API rechaza la solicitud antes de que llegue a la lógica de negocio.
    * La API devuelve un código de estado HTTP de error al cliente:
        * `401 Unauthorized`: Usualmente indica que la autenticación falló (el token falta, es inválido, expiró, la firma es incorrecta, etc.).
        * `403 Forbidden`: Usualmente indica que el usuario está autenticado (el token es válido), pero no tiene los permisos (roles/scopes/claims) necesarios para acceder a este recurso en particular.

## 4. Renovación y Reintento (Manejado por OIDC Client TS)

* Si el cliente recibe un `401 Unauthorized` (por ejemplo, porque el `access_token` acaba de expirar), `oidc-client-ts` (si `automaticSilentRenew` está habilitado) intentará automáticamente renovar el `access_token` en segundo plano usando el `refresh_token` (si se obtuvo).
* Si la renovación es exitosa, el `userManager` se actualizará con el nuevo `access_token`. La aplicación cliente puede entonces reintentar la llamada a la API con el nuevo token.
* Si la renovación falla (por ejemplo, el `refresh_token` ha expirado o ha sido revocado), `oidc-client-ts` puede disparar un evento (`accessTokenExpiring`, `accessTokenExpired`) y la aplicación cliente necesitará redirigir al usuario para que se vuelva a autenticar completamente (volver al paso 2.1 del flujo de autenticación).

Este flujo de autorización es la forma estándar en que las SPAs acceden a APIs protegidas utilizando tokens JWT emitidos por un servidor OAuth 2.0 / OIDC.