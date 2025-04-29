# Instalación y Despliegue de TFST.AuthServer, TFST.API y tfst-demo

Este documento proporciona instrucciones paso a paso para instalar y desplegar los proyectos TFST.AuthServer, TFST.API y tfst-demo.

## Requisitos Previos

Antes de comenzar con la instalación, asegúrate de tener los siguientes requisitos previos:

- .NET SDK (versión específica)
- Base de datos (SQL Server)

## Pasos de Instalación

### 1. Clonar el Repositorio

Clona el repositorio de GitHub que contiene los proyectos:

```bash
git clone https://github.com/jgccon/tfst.git
cd tfst
```

### 2. Configurar la Base de Datos

Crea una base de datos en tu sistema de gestión de bases de datos y configura las cadenas de conexión en los archivos de configuración correspondientes.

### 3. Instalar Dependencias

Para cada proyecto, navega a la carpeta del proyecto y ejecuta el siguiente comando para instalar las dependencias:

```bash
cd TFST.AuthServer
dotnet restore

cd ../TFST.API
dotnet restore
```

### 4. Configurar Variables de Entorno

Asegúrate de configurar las variables de entorno necesarias para cada proyecto. Esto puede incluir cadenas de conexión, claves secretas y otros parámetros de configuración.

### 5. Ejecutar Migraciones

Ejecuta las migraciones de la base de datos para inicializar la estructura de la base de datos:

```bash
cd TFST.AuthServer
dotnet ef database update
```

### 6. Iniciar los Proyectos

Inicia cada uno de los proyectos en sus respectivas carpetas:

```bash
cd TFST.AuthServer
dotnet run

cd ../TFST.API
dotnet run

cd ../tfst-demo
dotnet run
```

## Verificación

Una vez que todos los proyectos estén en funcionamiento, verifica que puedas acceder a las siguientes URL:

- TFST.API: `https://localhost:5001`
- TFST.AuthServer: `https://localhost:6001`
- tfst-demo: `http://localhost:7000`