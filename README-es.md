# The Full Stack Team (TFST)

**TFST** es una plataforma de código abierto diseñada para revolucionar la **gestión de proyectos y freelance** mediante la integración de **transparencia, contratos inteligentes y mecanismos de confianza descentralizados**. Proporciona a los profesionales, empresas y reclutadores las herramientas que necesitan para gestionar **clientes, proyectos, contratos, facturación y horas de trabajo**, todo en un entorno **escalable y multiusuario**.

## 🌍 ¿Por qué TFST?

TFST es más que un mercado de freelancers: es un **CENTRO** donde los mejores talentos de TI se conectan con las mejores oportunidades de una manera **transparente y eficiente**.

- **Para empresas** → Accede a freelancers de TI verificados sin intermediarios.
- **Para profesionales de TI** → Pagos justos, oportunidades globales y un **sistema de crecimiento basado en la reputación**.
- **Para reclutadores** → Perfiles técnicos preevaluados y **procesos de contratación optimizados**.

---

## 🚀 Características

- **🔹 Confianza descentralizada** → Contratos inteligentes basados ​​en blockchain para pagos y reputación.
- **🔹 Compatibilidad con múltiples inquilinos** → Administra múltiples clientes con **datos aislados**.
- **🔹 Gestión de proyectos y clientes** → Asigna profesionales a proyectos y **sigue el progreso**.
- **🔹 Facturación y contratos** → Facturación automatizada y **acuerdos seguros**.
- **🔹 Seguimiento del tiempo** → Registra las horas de trabajo y **monitorea la productividad**.
- **🔹 Hoja de ruta transparente** → Desarrollo abierto con un enfoque **impulsado por la comunidad**.

---

## 🛠️ Pila tecnológica (flexible)

TFST está construida con tecnologías **modernas y escalables**, pero **permanece abierta a mejoras** a medida que la plataforma evoluciona.

- **Infraestructura y nube** → Azure, Terraform, Terragrunt
- **Frontend** → Angular
- **Backend** → .NET
- **Bases de datos** → PostgreSQL o SQL Server (por determinar), CosmosDB (Mongo)
- **Containerización** → Docker
- **CI/CD y automatización** → Azure DevOps
- **IA y blockchain** → Aún por definir, se están explorando las soluciones más adecuadas

---

## 📌 Hoja de ruta

### **MVP (primeros 3 meses)**
✅ **Registro** de autónomos y clientes con validación KYC.
✅ Sistema de perfiles con **filtrado basado en habilidades**.
✅ Contratación inicial y **pagos basados ​​en contratos inteligentes**.

### **Fase 2 (próximos 6 meses)**
✅ **Gestión de proyectos** completa con seguimiento del tiempo y pagos automáticos.
✅ **Sistema de reputación** basado en la validación del cliente y evaluaciones técnicas.
✅ **Mercado de consultoría** para tutoría y capacitación.

### **Desafíos que estamos abordando**
✅ **Escalabilidad** → Arquitectura de microservicios para **soportar alto tráfico**.
✅ **Seguridad** → Auditoría de contratos inteligentes y **protección de datos**.
✅ **Experiencia del usuario** → UI/UX simple e intuitiva para **altas tasas de conversión**.

---

## ⚡ Instalación

### Requisitos previos
Asegúrese de tener instalado lo siguiente:
- **Git**
- **.NET SDK 8.0**
- **Node.js (v18.x) y npm**
- **Angular CLI**
- **Docker (opcional)**

### Pasos
1. **Clonar el repositorio**:
```bash
git clone https://github.com/JGCarmona-Consulting/tfst.git
cd tfst
```

2. **Configuración del backend**:
```bash
cd src/api
dotnet build
```

3. **Configuración del frontend**:
```bash
cd ../webapp
npm install
ng serve
```

4. **Ejecutar la aplicación localmente**:
```bash
dotnet run --project src/api
ng serve --project webapp
```
# CI/CD con Azure DevOps

[YA REALIZADO AQUI](https://dev.azure.com/jgcarmona/TheFullStackTeam/)

## Guía de Contribución
¡Damos la bienvenida a contribuciones! Consulta [CONTRIBUTING-es.md](CONTRIBUTING-es.md) para más detalles.

## Documentación
Para documentación detallada, consulta la carpeta `/docs` o visita nuestra [página de documentación](docs/README.md).

## Licencia
Licenciado bajo la Licencia MIT. Consulta [LICENSE](LICENSE) para más detalles.
