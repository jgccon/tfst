---
id: installation
title: Installation
---
# Instalación e implementación de TFST.AuthServer, TFST.API y tfst-demo

Este documento proporciona instrucciones paso a paso para instalar e implementar los proyectos TFST.AuthServer, TFST.API y tfst-demo.

## Requisitos previos

Antes de comenzar la instalación, asegúrese de cumplir con los siguientes requisitos previos:

- SDK de .NET (versión específica)
- Base de datos (SQL Server)

## Pasos de instalación

### 1. Clonar el repositorio

Clone el repositorio de GitHub que contiene los proyectos:

```bash
git clone https://github.com/jgccon/tfst.git
cd tfst
```

### 2. Configurar la base de datos

Cree una base de datos en su sistema de gestión de bases de datos y configure las cadenas de conexión en los archivos de configuración correspondientes.

### 3. Instalar dependencias

Para cada proyecto, acceda a la carpeta del proyecto y ejecute el siguiente comando para instalar las dependencias:

```bash
cd TFST.AuthServer
dotnet restore

cd ../TFST.API
dotnet restore
```

### 4. Configurar variables de entorno

Asegúrese de configurar las variables de entorno necesarias para cada proyecto. Esto puede incluir cadenas de conexión, claves secretas y otros parámetros de configuración.

### 5. Ejecutar migraciones

Ejecute las migraciones de la base de datos para inicializar la estructura de la base de datos:

```bash
cd TFST.AuthServer
dotnet ef database update
```

### 6. Iniciar los proyectos

Inicie cada proyecto en sus respectivas carpetas:

```bash
cd TFST.AuthServer
dotnet run

cd ../TFST.API
dotnet run

cd ../tfst-demo
dotnet run
```

## Verificación

Una vez que todos los proyectos estén en ejecución, verifique que pueda acceder a las siguientes URL:

- TFST.API: `https://localhost:5001`
- TFST.AuthServer: `https://localhost:6001`
- tfst-demo: `http://localhost:7000`
