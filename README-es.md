# The Full Stack Team (TFST)

**The Full Stack Team (TFST)** es una plataforma de código abierto diseñada para optimizar la gestión de proyectos y clientes para freelancers y profesionales. Esta plataforma ofrece herramientas para gestionar clientes, proyectos, contratos, facturación y horas trabajadas en un entorno multitenant.

## Características

- **Soporte Multitenant**: Gestiona múltiples clientes con datos aislados.
- **Gestión de Proyectos**: Asigna profesionales a proyectos y realiza seguimiento.
- **Gestión de Clientes**: Maneja contratos, facturación y cobros.
- **Seguimiento de Horas**: Registra horas de trabajo y monitorea la productividad.
- **Arquitectura Escalable**: Construida utilizando tecnologías modernas para alto rendimiento.

## Tecnologías Utilizadas

- **Backend**: .NET (ASP.NET Core)
- **Frontend**: Angular
- **Bases de Datos**: SQL Server, PostgreSQL, Cosmos DB (MongoDB API)
- **Autenticación**: JWT, OAuth2, Azure AD B2C
- **Contenerización**: Docker
- **CI/CD**: Azure DevOps, GitHub Actions
- **Infraestructura como Código**: Terraform
- **Plataforma en la Nube**: Azure

## Instalación

### Requisitos Previos
Asegúrate de tener instalados:
- **Git**
- **.NET SDK 8.0**
- **Node.js (v18.x) y npm**
- **Angular CLI**
- **Docker (opcional)**

### Pasos
1. **Clona el Repositorio**:
   ```bash
   git clone https://github.com/JGCarmona-Consulting/tfst.git
   cd tfst
   ```

2. **Configuración del Backend**:
   ```bash
   cd src/api
   dotnet build
   ```

3. **Configuración del Frontend**:
   ```bash
   cd ../webapp
   npm install
   ng serve
   ```

4. **Ejecutar la Aplicación Localmente**:
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
