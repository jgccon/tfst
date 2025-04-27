oidc.Log.setLogger(console);
oidc.Log.setLevel(oidc.Log.INFO);

const userManager = new oidc.UserManager({
    authority: 'https://localhost:6001',
    scope: 'openid profile email roles offline_access TFST_API',
    client_id: 'tfst_clientwebapp',
    redirect_uri: window.location.origin + '/signin-callback.html',
    post_logout_redirect_uri: `${window.location.origin}/index.html`,
    response_type: 'code',
    automaticSilentRenew: true,
    includeIdTokenInSilentRenew: true,
});

async function login() {
    try {
        await userManager.signinRedirect();
    } catch (error) {
        console.error('Error durante el login:', error);
    }
}

async function logout() {
    try {
        await userManager.signoutRedirect();
    } catch (error) {
        console.error('Error durante el logout:', error);
    }
}

async function refreshToken() {
    try {
        const user = await userManager.getUser();
        if (user) {
            await userManager.signinSilent();
            console.log('Token refrescado exitosamente');
        }
    } catch (error) {
        console.error('Error al refrescar el token:', error);
    }
}

async function callApi() {
    try {
        const user = await userManager.getUser();
        if (!user) {
            console.error('Usuario no autenticado');
            return;
        }

        const response = await fetch('https://localhost:5001/api', {
            headers: {
                'Authorization': `Bearer ${user.access_token}`
            }
        });

        if (!response.ok) {
            throw new Error(`Error HTTP: ${response.status}`);
        }

        const data = await response.text();
        return data;
    } catch (error) {
        console.error('Error llamando a la API:', error);
        throw error;
    }
}

window.login = login;
window.logout = logout;
window.refreshToken = refreshToken;
window.callApi = callApi;