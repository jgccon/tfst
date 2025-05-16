
# Contribuir a The Full-Stack Team

¡Gracias por considerar contribuir a **The Full-Stack Team**! Tus aportaciones son invaluables para mejorar la plataforma y construir una solución de gestión robusta para freelancers y profesionales. Esta guía proporciona instrucciones y mejores prácticas para contribuir al proyecto.

## Tabla de Contenidos

- [Cómo Contribuir](#cómo-contribuir)
- [Código de Conducta](#código-de-conducta)
- [Primeros Pasos](#primeros-pasos)
- [Estrategia de Ramas](#estrategia-de-ramas)
- [Proceso para Pull Requests](#proceso-para-pull-requests)
- [Contacto](#contacto)

## Cómo Contribuir

Aceptamos varios tipos de contribuciones, incluyendo:
- Reportar errores y enviar solicitudes de nuevas funcionalidades.
- Escribir documentación y mejorar la existente.
- Contribuir con código para nuevas características, corrección de errores y mejoras.
- Mejorar la cobertura de pruebas y asegurar la estabilidad de la plataforma.

## Código de Conducta

Por favor, lee y respeta nuestro [Código de Conducta](CODE_OF_CONDUCT-es.md) para fomentar una comunidad acogedora y respetuosa.

## Primeros Pasos

1. **Haz un Fork del Repositorio**: 
   - Ve al [repositorio](https://github.com/JGCarmona-Consulting/tfst) y haz clic en "Fork".
2. **Clona tu Fork**:
   ```bash
   git clone https://github.com/tu-usuario/tfst.git
   cd tfst
   ```
3. **Configura el Proyecto**:
   - Sigue la [Guía de Instalación](README-es.md#instalación) para configurar el backend y frontend localmente.

## Estrategia de Ramas

Nuestra estrategia de ramas es la siguiente:

- La rama **dev** es la rama principal de desarrollo. Todas las características y correcciones deben crearse a partir de **dev**.
  - Utiliza nombres de ramas como:
    - `feature/nombre-corto`
    - `fix/id-issue`
    - `docs/nombre-corto`
  - Crea Pull Requests (PRs) dirigidos a **dev**.
  - Los PRs fusionados en **dev** se despliegan automáticamente en nuestro entorno de pruebas.
- La rama **main** es la rama de producción.
  - Solo un grupo restringido de contribuidores puede crear PRs hacia **main**.
  - Los PRs hacia **main** se utilizan para despliegues en producción.

## Proceso para Pull Requests

1. **Crea una Rama**: 
   ```bash
   git checkout -b feature/nueva-funcionalidad
   ```
2. **Realiza tus Cambios**: Asegúrate de que el código esté formateado correctamente y que todas las pruebas pasen.
3. **Haz Commit y Push**:
   ```bash
   git add .
   git commit -m "Descripción de los cambios"
   git push origin feature/nueva-funcionalidad
   ```
4. **Abre un Pull Request**:
   - Ve al repositorio original en GitHub.
   - Haz clic en la pestaña **Pull Requests** y luego en **New Pull Request**.
   - Proporciona un título descriptivo y resume tus cambios en la descripción del PR.
   - Enlaza el PR con issues relevantes usando `#número-del-issue`.

5. **Revisión y Merge**: Un mantenedor del proyecto revisará tu PR. Prepárate para realizar ajustes según el feedback recibido.

## Comunicación y Soporte

Para discusiones y actualizaciones rápidas, únete a nuestro [grupo de WhatsApp](https://chat.whatsapp.com/Jnoi9xHbMQ09fJNpxJA0LJ).

Si tienes preguntas sobre el proceso de contribución, utiliza [GitHub Issues](https://github.com/juangcarmona/tfst/issues) o contacta directamente al mantenedor a través del correo [juan@jgcarmona.com].
