# Visión General de la Arquitectura

Este documento proporciona una visión general de la arquitectura de los proyectos TFST.AuthServer, TFST.API y tfst-demo, detallando cómo interactúan entre sí.

## TFST.AuthServer

TFST.AuthServer es el componente central de autenticación que utiliza OpenIddict para gestionar la autenticación de usuarios. Proporciona un flujo de trabajo seguro para la emisión de tokens y la validación de credenciales.

## TFST.API

TFST.API actúa como la interfaz entre los clientes y el servidor de autenticación. Expone varios endpoints que permiten a los clientes interactuar con los recursos protegidos, utilizando tokens de acceso emitidos por el AuthServer.

## tfst-demo

El cliente tfst-demo es una aplicación de ejemplo que demuestra cómo interactuar con TFST.API. Utiliza el flujo de autenticación proporcionado por TFST.AuthServer para obtener tokens y acceder a los recursos.

## Interacción entre Componentes

1. **Autenticación**: Los usuarios se autentican a través de TFST.AuthServer, que valida las credenciales y emite un token de acceso.
2. **Acceso a la API**: El cliente tfst-demo utiliza el token de acceso para realizar solicitudes a TFST.API.
3. **Protección de Recursos**: TFST.API valida el token de acceso en cada solicitud para garantizar que solo los usuarios autenticados puedan acceder a los recursos.

Esta arquitectura modular permite una fácil escalabilidad y mantenimiento, asegurando que cada componente pueda evolucionar de manera independiente mientras se mantiene la integridad del sistema en su conjunto.