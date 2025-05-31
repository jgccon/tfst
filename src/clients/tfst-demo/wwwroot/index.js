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
        console.error('Error during login:', error);
    }
}

async function logout() {
    try {
        await userManager.signoutRedirect();
    } catch (error) {
        console.error('Error during logout:', error);
    }
}

async function refreshToken() {
    try {
        const user = await userManager.getUser();
        if (user) {
            await userManager.signinSilent();
            console.log('Token refreshed successfully');
        }
    } catch (error) {
        console.error('Error refreshing token:', error);
    }
}

async function callApi() {
    try {

        const response = await fetch('https://localhost:5001/health');

        if (!response.ok) {
            throw new Error(`Error HTTP: ${response.status} ${response.statusText} ${await response.text()}`);
        }

        const data = await response.text();
        return data;
    } catch (error) {
        console.error('Fail calling API:', error);
        throw error;
    }
}

async function callApiToken() {
    try {
        const user = await userManager.getUser();
        if (!user) {
            console.error('User not authenticated');
            return;
        }

        const response = await fetch('https://localhost:5001/api', {
            headers: {
                'Authorization': `Bearer ${user.access_token}`
            }
        });

        if (!response.ok) {
            throw new Error(`Error HTTP: ${response.status} ${response.statusText} ${await response.text()}`);
        }

        const data = await response.text();
        return data;
    } catch (error) {
        console.error('Fail calling API:', error);
        throw error;
    }
}

