---
id: 08-crosscutting-concepts
title: Crosscutting Concepts
sidebar_position: 8
---

#8. Conceptos transversales

## Seguridad
- **Autenticación**: Gestionada mediante JWT y Auth0/Azure AD B2C.
- **Autorización**: Control de acceso basado en roles para diferentes niveles de usuario.

## Registro y monitorización
- **Registro**: Utilice Serilog o un marco similar para el registro estructurado y Open Telemetry.
- **Monitorización**: Azure Monitor para supervisar el estado de la aplicación.

## Gestión de la configuración
- Almacene las opciones de configuración en Azure App Configuration o en variables de entorno.

## Protección de datos y cumplimiento normativo

### **Cumplimiento normativo**
TFST cumple con el **RGPD (Reglamento General de Protección de Datos)** para los usuarios europeos y la **CCPA (Ley de Privacidad del Consumidor de California)** para los usuarios de Estados Unidos.

- **Derechos del usuario**:
- Acceso a sus datos personales.
- Derecho a solicitar la eliminación de datos.
- Derecho a restringir el tratamiento de sus datos.

### **Medidas de Seguridad de Datos**
- **Cifrado de Datos**: Todos los datos sensibles se cifran en reposo y en tránsito.
- **Seguridad de Tokens**: Los tokens OAuth2 tienen una vida útil corta y se almacenan de forma segura.
- **Registros de Acceso**: Los registros de acceso de los usuarios se almacenan de forma segura para su auditoría.

#### **Delegado de Protección de Datos (DPD)**
Para cualquier consulta relacionada con la privacidad o para ejercer sus derechos sobre los datos, los usuarios pueden contactar con:
📧 **juan@jgcarmona.com**

