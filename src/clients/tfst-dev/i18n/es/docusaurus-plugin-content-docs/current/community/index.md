---
id: intro
title: Community
sidebar_position: 5
---
# Contribuyendo al The Full-Stack Team

¡Gracias por considerar contribuir al **The Full-Stack Team**! Tus contribuciones son invaluables para mejorar la plataforma y crear una solución robusta de gestión de código abierto para freelancers y profesionales. Esta guía proporciona instrucciones y buenas prácticas para contribuir al proyecto.

## Índice

- [Cómo Contribuir](#how-to-contribute)
- [Código de Conducta](#code-of-conduct)
- [Introducción](#getting-started)
- [Estrategia de Ramificación](#branching-strategy)
- [Proceso de Solicitud de Incorporación](#pull-request-process)
- [Contacto](#contact)

## Cómo Contribuir

Agradecemos diversos tipos de contribuciones, incluyendo:
- Reportar errores y enviar solicitudes de funcionalidades.
- Redactar documentación y mejorar la existente.
- Contribuir con código para nuevas funcionalidades, corrección de errores y mejoras.

- Mejorar la cobertura de pruebas y garantizar la estabilidad de la plataforma.

## Código de Conducta

Lea y respete nuestro [Código de Conducta](CODE_OF_CONDUCT.md) para fomentar una comunidad acogedora y respetuosa.

## Primeros pasos

1. **Bifurcar el repositorio**:

- Vaya al [repositorio](https://github.com/JGCarmona-Consulting/tfst) y haga clic en "Bifurcar".

2. **Clonar su bifurcación**:
```bash
git clone https://github.com/your-username/tfst.git
cd tfst
```
3. **Configurar el proyecto**:

- Siga la [Guía de instalación](../install/index.md) para configurar el backend y el frontend localmente.

## Estrategia de ramificación

Nuestro proyecto sigue la siguiente estrategia de ramificación:

- La rama **dev** es la rama principal de desarrollo. Todas las características y correcciones deben derivarse de **dev**.
- Usar nombres de rama como:
- `feature/short-description`
- `fix/issue-id`
- `docs/short-description`
- Crear solicitudes de extracción (PR) dirigidas a **dev**.
- Las PR fusionadas a **dev** se implementan automáticamente en nuestro entorno de pruebas.
- La rama **main** es la rama de producción.
- Solo un grupo restringido de colaboradores puede crear PR para **main**.
- Las PR para **main** se utilizan para implementaciones de producción.

## Proceso de solicitud de extracción

1. **Crear una rama**: 
```bash
git checkout -b feature/your-feature-name
```
2. **Realizar los cambios**: Asegurarse de que el código tenga el formato correcto y de que todas las pruebas sean correctas.
3. **Confirmar y enviar**:
```bash
git add . git commit -m "Añadir descripción de los cambios"
git push origin feature/your-feature-name
```
4. **Abrir una solicitud de extracción**:
- Acceder al repositorio original en GitHub.
- Hacer clic en la pestaña **Solicitudes de extracción** y luego en **Nueva solicitud de extracción**.
- Proporcionar un título descriptivo y resumir los cambios en la descripción de la solicitud de extracción.
- Vincular la solicitud de extracción a las incidencias relevantes usando `#issue-number`.

5. **Revisar y fusionar**: Un responsable del proyecto revisará tu solicitud de extracción. Prepárate para realizar ajustes según los comentarios.

## Comunicación y soporte

Para debates y actualizaciones rápidas, únete a nuestro [grupo de WhatsApp](https://chat.whatsapp.com/Jnoi9xHbMQ09fJNpxJA0LJ).

Si tiene preguntas sobre el proceso de contribución, utilice [GitHub Issues](https://github.com/juangcarmona/tfst/issues) o contacte directamente con el responsable del proyecto por correo electrónico a [juan@jgcarmona.com].