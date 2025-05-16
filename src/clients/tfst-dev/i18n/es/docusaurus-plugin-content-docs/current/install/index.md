---
id: install
title: Install & Setup Guide
slug: /install
sidebar_position: 7
---

# ⚙️ TFST — Guía completa de instalación y configuración

Bienvenido a la guía completa de instalación de **The Full-Stack Team (TFST)**. Este documento le ayudará a configurar la plataforma desde cero en su entorno local o con Docker.

---

## 📋 Requisitos previos

Asegúrate de tener instalado en tu equipo lo siguiente:

- **Git**
- **Docker**
- **.NET SDK 7 o 10**
- **Node.js (v18+) y npm**
- **Angular CLI**
- **Visual Studio 2022** (para backend) o **VSCode** (para frontend)
- **Opcional**: Azure CLI y Terraform (para implementaciones en la nube)

---

## 📦 Clonar el repositorio

```bash
git clone https://github.com/jgccon/tfst.git
cd tfst
```

Esto recuperará todo el código, incluyendo el backend, los clientes frontend, la infraestructura y la documentación.

---

## 🧪 Configuración de desarrollo local (sin Docker)

### 1️⃣ Configurar certificado HTTPS

```bash
cd src
mkdir -p certs
dotnet dev-certs https -ep ./certs/tfst_dev_cert.pfx -p Password123*
dotnet dev-certs https --trust
```

Esto generará un certificado autofirmado para desarrollo local HTTPS.

---

### 2️⃣ Configuración del backend (TFST.API)

- Abra el archivo `TFST.sln` en **Visual Studio 2022**
- Seleccione `TFST.API` como proyecto de inicio
- Presione `Ctrl + F5` para ejecutar sin depurar

Asegúrese de que la cadena de conexión de su base de datos esté configurada correctamente (SQL Server o PostgreSQL). Puedes usar secretos o `appsettings.Development.json`.

---

### 3️⃣ Configuración del frontend (clientes TFST)

Hay varios clientes en `src/clients/`:

- `tfst-app` → Aplicación SaaS
- `tfst-dev` → Portal de documentación
- `full-stack-team` → Sitio institucional
- `tfst-demo` → Espacio experimental/de pruebas

Para ejecutar uno (por ejemplo, `tfst-app`):

```bash
cd src/clients/tfst-app
npm install
ng serve
```

Puedes ejecutar varios clientes en paralelo usando diferentes puertos.

---

## 🐳 Configuración basada en Docker (opcional)

Para ejecutar todo mediante Docker Compose:

```bash
docker-compose up -d
```

Asegúrate de que tu archivo `.env` (o `docker-compose.override.yml`) incluya los nombres de servicio y los puertos correctos:

```env
ASPNETCORE_URLS=http://+:8080
TFST_DB_HOST=tfst-sql
```

Es posible que tengas que actualizar la cadena de conexión dentro de la configuración de la API para que refleje nombres de servicio como `tfst-sql` o `tfst-db`.

---

## 🧯 Solución de problemas

### 🔒 Problemas con el certificado
Asegúrate de que `certs/tfst_dev_cert.pfx` exista y sea de confianza. De lo contrario, vuelve a ejecutar la configuración del certificado.

### 🐘 Tiempos de espera de la base de datos
Comprueba que tu servicio SQL esté en ejecución y sea accesible desde tu host/contenedor.

### ⚡ Errores de Angular
Prueba a eliminar `node_modules/` y reinstalar las dependencias:

```bash
rm -rf node_modules package-lock.json
npm install
```

### 🐳 Problemas con Docker
Reconstruir contenedores:
```bash
docker-compose down
docker-compose build --no-cache
docker-compose up
```

---

## 🤝 Contribuye

Una vez que estés en funcionamiento, consulta:

- [Código de conducta](../code-of-conduct)
- [Pautas de contribución](../contributing)

Y considera contribuir a la hoja de ruta, escribir documentación o ayudar a resolver problemas.

---

¡Que disfrutes del hacking! 💻✨
