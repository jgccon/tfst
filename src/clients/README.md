# TFST Clients — Frontend Applications and Portals

This folder contains all web clients and public-facing portals for the **TFST platform**, each mapped to a specific domain, purpose, and audience.

## Overview

| Folder             | Domain            | Purpose                                         | Deployment Target     |
|--------------------|-------------------|--------------------------------------------------|------------------------|
| `tfst-app`         | `tfst.app`        | Main SaaS application for registered users       | Azure App Service (dev, beta) |
| `tfst-xyz`         | `tfst.xyz`        | Public-facing app for profiles and onboarding    | Azure App Service (dev, beta) |
| `tfst-dev`         | `tfst.dev`        | Technical documentation and dev portal           | Azure Static Web App (prod only) |
| `full-stack-team`  | `full-stack.team` | Institutional marketing site                     | Azure Static Web App (prod only) |

---

## Purpose by Domain

- **`tfst.app`** – Authenticated app for users to manage projects, contracts, and profiles.
- **`tfst.xyz`** – Public entry point into the TFST ecosystem. Explore talent, view profiles, and onboard.
- **`tfst.dev`** – Open-source hub and documentation site for developers and contributors.
- **`full-stack.team`** – Marketing, branding, and conversion funnel for companies and professionals.

---

## Environments

| Environment | App Services                     | Static Web Apps               |
|-------------|----------------------------------|-------------------------------|
| `dev`       | `api`, `auth`, `tfst.app`, `tfst.xyz` (Azure defaults) | — |
| `beta`      | Custom domains with SSL for all | — |
| `portals`   | —                                | `tfst.dev`, `full-stack.team` |

---

## Repositories & Structure

Each subfolder is either an Angular app (for app-like frontends) or a static site (Docusaurus, Astro, etc.). All are independently deployable and versioned:

- `tfst-app/` – Angular SPA (SaaS UI)
- `tfst-xyz/` – Angular SPA (public explorer)
- `tfst-dev/` – Docusaurus or Astro site
- `full-stack-team/` – Angular (static + i18n) or Jamstack site

---

## 🌐 Subdomain Naming Conventions

The TFST platform follows these domain and subdomain rules across environments:

### Application Services (App Service based)

| Component         | Dev Domain                        | Beta (Prod) Domain     |
|------------------|------------------------------------|------------------------|
| Auth Server       | `tfst-auth-dev.azurewebsites.net`       | `auth.tfst.app`        |
| API               | `tfst-api-dev.azurewebsites.net`        | `api.tfst.app`         |
| SaaS Frontend     | `tfst-app-dev.azurewebsites.net`   | `tfst.app`             |
| Public Frontend   | `tfst-xyz-dev.azurewebsites.net`   | `tfst.xyz`             |

### Static Portals (Azure Static Web Apps)

| Portal            | Dev Domain     | Production Domain    |
|-------------------|----------------|-----------------------|
| Documentation     | —              | `tfst.dev`            |
| Marketing Site    | —              | `full-stack.team`     |

- Development uses default Azure-generated domains (no custom SSL).
- Beta acts as production and uses custom domains with SSL certificates.
- Static sites are deployed only to production.