---
id: time_tracking-reporting
title: Time Tracking Reporting
---

# Seguimiento de Tiempo e Informes

## Resumen
El módulo **Seguimiento de Tiempo e Informes** ofrece a los freelancers una herramienta integrada para registrar las horas de trabajo, gestionar las entradas de tiempo y generar informes o facturas basadas en tarifas por hora.

Esta herramienta es **gratuita para todos los freelancers** registrados en TFST y se integra con el módulo **Pagos y Facturación** para facilitar la facturación automatizada.

## Entidades Principales

### **Entrada de Tiempo**
- Representa la sesión de trabajo registrada de un freelancer.
- Incluye:
- **Fecha** → Fecha de finalización del trabajo.
- **Horas Trabajadas** → Número total de horas trabajadas en esa fecha.
- **Referencia de Proyecto** → Vincula la entrada a un proyecto específico.
- **Referencia de Tarea (Opcional)** → Vincula la entrada a una tarea específica.
- **Tarifa por Hora** → Definida por el freelancer o el contrato. - **Entrada flexible**:
- Los freelancers **pueden registrar horas sin especificar una hora de inicio/fin**.
- Ejemplo: 
- ✅ **"Trabajé 5 horas hoy en el Proyecto X."**
- ✅ **"Dediqué 3 horas al desarrollo de API esta semana."**

### **Proyecto**
- Representa un trabajo o contrato freelance donde se registran las horas.
- Incluye:
- **Referencia del cliente** → Vincula el proyecto a un cliente específico.
- **Referencia del freelancer** → Freelancer asignado.
- **Tarifa por hora** → Tarifa base para la facturación.
- **Estado** → (Activo, Completado, Cancelado).

### **Factura (vinculada a pagos y facturación)**
- Las facturas se pueden **generar automáticamente** a partir de las horas registradas.
- Cálculo: 
``Importe total = Total de horas trabajadas x Tarifa por hora`
- Las facturas se almacenan en el módulo **Pagos y facturación**.

### **Informe de tiempo**
- Vista resumida de las horas registradas por:
- **Proyecto**
- **Cliente**
- **Periodo**
- **Tarea** (si corresponde)
- Se utiliza para el seguimiento interno o la facturación.

## Relaciones
- **Entradas de tiempo de freelancer (1:N)** → Un freelancer registra varias entradas de tiempo.
- **Entradas de tiempo de proyecto (1:N)** → Un proyecto acumula varios registros de tiempo.
- **Factura de entrada de tiempo (1:1)** → Los registros de tiempo se pueden agrupar en facturas.
- **Informes de freelancer (1:N)** → Los freelancers generan informes a partir de las horas registradas.

```mermaid
erDiagram
Freelancer ||--o{ Entrada de tiempo: registra
Entrada de tiempo ||--|| Proyecto: pertenece_a
Entrada de tiempo ||--o| Factura: facturada
Freelancer ||--o{ Informe de tiempo: genera
Proyecto ||--o{ Entrada de tiempo: acumula
```

## **Flujos de trabajo de validación y aprobación**
A medida que el sistema evolucione, se introducirán diferentes niveles de validación:

1. **Modo autogestionado (predeterminado)**
- Los freelancers registran sus propias horas sin validación externa.
- Diseñado para **seguimiento e informes personales**.

2. **Validación supervisada (para consultorías y equipos)**
- Los gestores de proyectos pueden **revisar y aprobar las horas** antes de facturar.
- Admite **flujos de trabajo de aprobación personalizados** (por ejemplo, revisiones semanales o mensuales).

3. **Validación Automatizada (Futuro)**
- Integración con **herramientas de seguimiento de tiempo** (p. ej., Toggl, Clockify, Jira).
- Validación basada en IA mediante **registros de actividad, confirmaciones e interacciones del proyecto**.

4. **Contratos Inteligentes para Pagos (Futuro)**
- Las horas validadas activan **pagos automáticos** mediante **contratos inteligentes con depósito en garantía**.
- Garantiza la transparencia entre freelancers y clientes.

## Características Clave
- **Registro de Tiempo Sencillo**
- Los freelancers seleccionan una **fecha** e introducen el total de **horas trabajadas**.
- No se necesitan horas de inicio ni de fin.

- **Tarifas por Hora Basadas en Proyecto**
- Establece diferentes tarifas por cliente o proyecto.
- Admite contratos de **precio fijo** o **por hora**.

- **Generación Automatizada de Facturas**
- Convierte las entradas de tiempo en **facturas** (cuando está habilitado).
- Admite **pagos parciales** para proyectos en curso.

- **Informes de tiempo completos**
- Filtra por **rango de fechas, proyecto, cliente**.
- Exporta a **PDF, XLSX, CSV** para informes de clientes.
## Mejoras futuras
- **Seguimiento de tiempo basado en IA** → Recomendaciones inteligentes para registrar horas.
- **Seguimiento a nivel de tarea** → Asigna entradas de tiempo a tareas específicas.
- **Compatibilidad con aplicaciones móviles** → Aplicación móvil nativa para seguimiento en tiempo real.
- **Validación blockchain** → Registros de prueba de trabajo inmutables.