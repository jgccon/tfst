---
id: openiddict-overview
title: Overview
sidebar_position: 0
---
# Descripción general de OpenIddict

OpenIddict es una biblioteca de código abierto que ofrece una forma sencilla de implementar un servidor OpenID Connect en sus aplicaciones. Está diseñada para ser fácil de usar e integrarse con aplicaciones ASP.NET Core existentes, lo que permite a los desarrolladores proteger sus API y gestionar la autenticación y autorización de usuarios de forma eficaz.

## Objetivo de OpenIddict

El objetivo principal de OpenIddict es permitir a los desarrolladores crear flujos de autenticación seguros para sus aplicaciones. Admite diversos métodos de autenticación, como la autenticación basada en contraseña, el inicio de sesión con redes sociales y más. OpenIddict también permite la emisión de tokens de acceso, tokens de actualización y tokens de ID, esenciales para proteger los endpoints de la API.

## Integración con TFST.AuthServer

En el contexto del proyecto TFST.AuthServer, OpenIddict se utiliza para gestionar la autenticación de usuarios y emitir los tokens necesarios para acceder a los recursos protegidos en proyectos TFST.API a través del cliente tfst-demo. La integración implica la configuración de los servicios de OpenIddict en la aplicación ASP.NET Core, la configuración de las tablas de base de datos necesarias para el almacenamiento de tokens y la definición de flujos de autenticación.

## Características clave

- **Gestión de tokens**: OpenIddict gestiona la creación, validación y revocación de tokens.
- **Personalizable**: Los desarrolladores pueden personalizar los flujos de autenticación y los procesos de emisión de tokens para satisfacer sus necesidades específicas.
- **Compatibilidad con múltiples tipos de concesión**: OpenIddict admite varios tipos de concesión OAuth2, incluyendo código de autorización, credenciales de cliente y credenciales de contraseña del propietario del recurso.

## Sitio web de OpenIddict
Para obtener más información, documentación y ejemplos de uso, visite el [sitio web oficial de OpenIddict](https://openiddict.com/).