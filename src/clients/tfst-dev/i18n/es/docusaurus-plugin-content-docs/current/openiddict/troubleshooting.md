---
id: troubleshooting
title: Troubleshooting
---

# Guía de solución de problemas

## Problemas comunes

### Problema 1: Error de autenticación
- **Descripción**: Los usuarios no pueden autenticarse.
- **Solución**: Asegúrese de que AuthServer esté en ejecución y de que se estén utilizando las credenciales de cliente correctas. Revise los registros para ver si hay mensajes de error.

### Problema 2: La API no responde
- **Descripción**: La API no responde a las solicitudes.
- **Solución**: Verifique que el servicio de la API esté en funcionamiento. Revise las configuraciones de red y asegúrese de que el punto final de la API esté correctamente especificado.

### Problema 3: Errores de configuración de OpenIddict
- **Descripción**: Errores relacionados con la configuración de OpenIddict.
- **Solución**: Revise la configuración de OpenIddict en los archivos de configuración. Asegúrese de que todos los parámetros necesarios estén configurados correctamente.

## Consejos de depuración
- Use los registros para obtener información detallada sobre el comportamiento de la aplicación.
- Revise la salida de la consola para detectar errores o advertencias durante el tiempo de ejecución. - Consulte la documentación para verificar cualquier configuración que se haya pasado por alto.

## Recursos adicionales
- [Documentación de OpenIddict](https://documentation.openiddict.com/)
- [Documentación de la API](./architecture/api.md)

Esta guía tiene como objetivo ayudar a los usuarios a resolver problemas comunes al usar los proyectos TFST.AuthServer, TFST.API y tfst-demo.