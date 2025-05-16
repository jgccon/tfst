---
id: identity-management
title: Identity  Management
---

# Identity & Access Management Features

## Feature: User Authentication & Login
- **User Story:**  
  *As a user, I want to log into TFST securely using email/password or external authentication, so I can access my account.*
- **Next Level:**  
  - Multi-Factor Authentication (MFA).  
  - Support for OAuth providers (Google, GitHub, LinkedIn).  

## Feature: Authorization & Role Management
- **User Story:**  
  *As an admin, I want to assign roles and permissions to users, so I can control what actions they can perform.*
- **Next Level:**  
  - Fine-grained permission management.  
  - Role-based access control (RBAC).  

## Feature: API Authentication with OAuth2 & JWT
- **User Story:**  
  *As a developer, I want to authenticate API requests using OAuth2 tokens, so I can access protected resources securely.*
- **Next Level:**  
  - Token expiration & refresh tokens.  
  - API scopes for granular permissions.  

## Feature: Single Sign-On (SSO)
- **User Story:**  
  *As a user, I want to log in once and access multiple services within TFST, so I don’t have to re-authenticate.*
- **Next Level:**  
  - Support for SAML or OpenID Connect for enterprise integrations.  
  - Session management across multiple portals.

---
## Frontend vs Backend Authentication Responsibilities

### **Frontend (Angular)**
- Stores and manages `access_token` and `id_token`.
- Uses silent authentication to refresh tokens (if configured).
- Handles user session state (logged-in/out).
- Redirects users to Identity Server for login and logout.
- Stores `claims` from the token to manage UI-based access control.

### **Backend (TFST API)**
- Validates tokens on every request.
- Extracts **claims** from the token to enforce permissions.
- Cancels sessions when roles or permissions change.

---

### **Autentication Sequence Diagram**
```mermaid
sequenceDiagram
    participant User
    participant Angular
    participant IdentityServer
    participant TFST.API

    User->>Angular: Enters TFST Portal
    Angular->>IdentityServer: Redirects to login (OAuth/OpenID)
    IdentityServer->>User: Requests credentials
    User->>IdentityServer: Sends email/password
    IdentityServer->>IdentityServer: Verifies user in ASP.NET Identity
    IdentityServer->>User: Requests MFA code (if enabled)
    User->>IdentityServer: Sends MFA code
    IdentityServer->>Angular: Returns access_token + id_token
    Angular->>TFST.API: Sends access_token in each request
    TFST.API->>IdentityServer: Verifies token and permissions
    TFST.API->>Angular: Returns protected data
```

### Components Diagram

```mermaid
graph TD
    A[User] -->|Logs in| B[Angular Frontend]
    B -->|Sends request| C[Identity Server]
    C -->|Validates user| D[ASP.NET Core Identity DB]
    C -->|Generates token| B
    B -->|Sends token| E[TFST API]
    E -->|Validates token with| C
    E -->|Returns protected resources| B
```

### Responsibilities

```mermaid
graph TD
    A[User] -->|Login request| B[Angular Frontend]
    B -->|Redirects| C[Identity Server]
    C -->|Authenticates| D[ASP.NET Core Identity]
    C -->|Issues access_token| B
    B -->|Sends token| E[TFST API]
    E -->|Validates token and extracts claims| B
```
