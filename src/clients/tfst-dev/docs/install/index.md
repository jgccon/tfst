---
id: install
title: Install & Setup Guide
slug: /install
sidebar_position: 7
---

# ⚙️ TFST — Full Installation & Setup Guide

Welcome to the complete installation guide for **The Full-Stack Team (TFST)**. This document will help you set up the platform from scratch on your local environment or with Docker.

---

## 📋 Prerequisites

Make sure you have the following installed on your machine:

- **Git**
- **Docker**
- **.NET SDK 7 or 10**
- **Node.js (v18+) and npm**
- **Angular CLI**
- **Visual Studio 2022** (for backend) or **VSCode** (for frontend)
- **Optional**: Azure CLI and Terraform (for cloud deployments)

---

## 📦 Clone the Repository

```bash
git clone https://github.com/jgccon/tfst.git
cd tfst
```

This will fetch all the code, including backend, frontend clients, infrastructure and documentation.

---

## 🧪 Local Development Setup (Without Docker)

### 1️⃣ Setup HTTPS Certificate

```bash
cd src
mkdir -p certs
dotnet dev-certs https -ep ./certs/tfst_dev_cert.pfx -p Password123*
dotnet dev-certs https --trust
```

This will generate a self-signed certificate for HTTPS local development.

---

### 2️⃣ Backend Setup (TFST.API)

- Open the `TFST.sln` file in **Visual Studio 2022**
- Select `TFST.API` as the startup project
- Press `Ctrl + F5` to run without debugging

Ensure your database connection string is properly set (SQL Server or PostgreSQL). You may use secrets or `appsettings.Development.json`.

---

### 3️⃣ Frontend Setup (TFST Clients)

There are multiple clients inside `src/clients/`:

- `tfst-app` → SaaS application
- `tfst-dev` → Documentation portal
- `full-stack-team` → Institutional site
- `tfst-demo` → Experimental/testing space

To run one (e.g., `tfst-app`):

```bash
cd src/clients/tfst-app
npm install
ng serve
```

You can run multiple clients in parallel by using different ports.

---

## 🐳 Docker-Based Setup (Optional)

To run everything via Docker Compose:

```bash
docker-compose up -d
```

Make sure your `.env` file (or `docker-compose.override.yml`) includes proper service names and ports:

```env
ASPNETCORE_URLS=http://+:8080
TFST_DB_HOST=tfst-sql
```

You may need to update the connection string inside the API config to reflect service names like `tfst-sql` or `tfst-db`.

---

## 🧯 Troubleshooting

### 🔒 Certificate issues
Make sure `certs/tfst_dev_cert.pfx` exists and is trusted. If not, re-run the certificate setup.

### 🐘 Database timeouts
Check that your SQL service is running and accessible from your host/container.

### ⚡ Angular errors
Try deleting `node_modules/` and reinstalling dependencies:

```bash
rm -rf node_modules package-lock.json
npm install
```

### 🐳 Docker issues
Rebuild containers:
```bash
docker-compose down
docker-compose build --no-cache
docker-compose up
```

---

## 🤝 Contribute

Once you're up and running, check out:

- [Code of Conduct](../code-of-conduct)
- [Contribution Guidelines](../contributing)

And consider contributing to the roadmap, writing docs, or helping triage issues.

---

Happy hacking! 💻✨
