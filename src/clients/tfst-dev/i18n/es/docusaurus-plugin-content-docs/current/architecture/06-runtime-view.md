---
id: 06-runtime-view
title: Runtime View
sidebar_position: 6
---

# 6. Vista de Tiempo de Ejecución

## Escenarios Comunes

1. **Inicio de Sesión**: El usuario se autentica mediante Auth0 o Azure AD B2C; recibe JWT para acceder a la API.
2. **Gestión de Empleados**: El usuario de RR. HH. crea/actualiza los perfiles de los empleados, que se almacenan en la base de datos.
3. **Seguimiento de Tiempo**: El usuario registra las horas trabajadas; los datos se procesan y almacenan en SQL Server.
4. **Proceso de Facturación**: El sistema calcula la facturación basándose en las horas trabajadas registradas y genera una factura.