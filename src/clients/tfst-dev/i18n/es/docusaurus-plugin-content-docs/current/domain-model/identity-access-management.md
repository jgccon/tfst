---
id: identity-access-management
title: Identity Access Management
---
# Gestión de Identidad y Acceso

## Resumen
Gestiona la autenticación, la autorización, los roles de usuario y las políticas de seguridad. Garantiza un control de acceso adecuado dentro de la plataforma.

## Entidades Principales

### **Entidades Gestionadas por el Servidor de Identidad (OpenIddict)**
Estas entidades gestionan la **autenticación**, la emisión de tokens y los proveedores de identidad externos.
- **Cuenta**: Representa a un usuario autenticado.
- **Credenciales de Proveedor Externo**: Proveedores de OAuth (Google, GitHub, etc.).
- **Token**: Tokens de Acceso y Actualización de OAuth2.
- **MFA (Autenticación Multifactor)**: Almacena datos de autenticación de segundo factor.

### **Entidades Gestionadas por la API de TFST**
Estas entidades gestionan la **autorización** dentro de la plataforma.
- **Usuario**: Representa a una persona dentro de TFST.
- **Rol**: Define el nivel de acceso de un usuario (Administrador, Reclutador, Freelancer).
- **Permisos**: Define las acciones que pueden realizar los roles.

## Estrategia de Roles y Reclamos

### **Roles**
Los roles se incluyen en los reclamos JWT y se utilizan para la autorización de la API.

- **Administrador** (reclamo `admin`) → Acceso completo a todas las funciones del sistema.
- **Usuario** (reclamo `user`) → Acceso de usuario estándar.
- **Gerente** (reclamo `manager`) → Puede gestionar los recursos de la organización.

### **Reclamos Adicionales**
Los reclamos adicionales se utilizan para un control más preciso de los privilegios de los usuarios.

- **Perfil Profesional** (`is_professional: true/false`) → Indica si un usuario tiene un perfil profesional. - **Membresía de la Organización** (`organization_id: xyz`) → Vincula un usuario a una organización.

## Relaciones

- **Servidor de Identidad**:
- **Usuario de Cuenta (1:1)**: Cada cuenta está vinculada a un usuario en TFST.
- **Credenciales de Proveedor Externo de Cuenta (1:N)**: Un usuario puede autenticarse con múltiples proveedores.
- **Tokens de Cuenta (1:N)**: Un usuario puede tener múltiples tokens activos.

- **API de TFST**:
- **Roles de Usuario (1:N)**: Un usuario puede tener múltiples roles.
- **Permisos de Rol (1:N)**: Los roles definen los permisos para las acciones del sistema.

```sirena
%%{init: {
"themeCSS": [
"[id*=Cuenta] .er.entityBox { trazo: verde claro; }",
"[id*=CredencialesDeProveedorExterno] .er.entityBox { trazo: verde claro; }",
"[id*=Token] .er.entityBox { trazo: verde claro; }",
"[id*=MFA] .er.entityBox { trazo: verde claro; }",
"[id*=Usuario] .er.entityBox { trazo: azul claro; }",
"[id*=Rol] .er.entityBox { trazo: azul claro; }",
"[id*=Permisos] .er.entityBox { trazo: azul claro; }"
]
}}%%
erDiagram
Cuenta ||--|| Usuario: links_to
Cuenta ||--o{ ExternalProviderCredentials: authenticates_with
Cuenta ||--o{ Token: generates
Cuenta ||--o{ MFA: secures
Usuario ||--o{ Rol: assignment
Rol ||--o{ Permissions: grants
```

## Separación de responsabilidades

### **Servidor de identidad (OpenIddict)**
- Gestiona la **autenticación** (inicio de sesión, OAuth, MFA).
- Emite tokens de OAuth2/OpenID Connect**.
- Gestiona las **sesiones de usuario** y los **tokens de actualización**.

### **API TFST**
- Valida los tokens** en cada solicitud.
- Aplica el control de acceso basado en roles (RBAC).
- Protege los recursos mediante notificaciones de tokens.

## Características principales
- Registro y autenticación de usuarios (correo electrónico, OAuth, SSO).
- Control de acceso basado en roles (RBAC).
- Autenticación multifactor (MFA).
- Gestión de tokens de API para integraciones externas.

## Mejoras futuras
- Gestión de permisos precisa.
- Integración con proveedores de autenticación empresarial (Azure AD, Okta).

