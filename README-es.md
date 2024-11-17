
# The Full Stack Team

**The Full Stack Team** es una plataforma de código abierto para freelancers y profesionales que facilita la gestión de clientes, proyectos, contratos, facturación y horas de trabajo en un entorno multitenant.

## Funcionalidades

- **Soporte Multitenant**: Gestiona múltiples clientes con aislamiento de datos.
- **Gestión de Proyectos**: Asigna freelancers a proyectos y sigue el progreso.
- **Gestión de Clientes**: Maneja contratos, facturación y cobros.
- **Registro de Horas**: Registra horas trabajadas y productividad.
- **Perfiles de Empleados**: Gestiona perfiles y certificaciones.
- **Arquitectura Escalable**: Construido con tecnologías modernas para alto rendimiento.

## Tecnologías Utilizadas

- **Backend**: .NET (ASP.NET Core)
- **Frontend**: Angular
- **Base de Datos**: SQL Server, PostgreSQL, Cosmos DB (API MongoDB)
- **Autenticación**: JWT, OAuth2, Azure AD B2C
- **Contenerización**: Docker
- **CI/CD**: Azure DevOps, GitHub Actions
- **Plataforma Cloud**: Azure, Terraform para infraestructura como código

## Instalación

### Prerrequisitos
- Git, .NET SDK 8.0, Node.js (v18.x), Angular CLI, Docker

### Pasos
1. **Clonar el Repositorio**:
   ```bash
   git clone https://github.com/JGCarmona-Consulting/tfst.git
   cd tfst
   ```
2. **Configurar Backend & Frontend**:
   ```bash
   cd src/api && dotnet build
   cd ../webapp && npm install
   ```
3. **Ejecutar Localmente**:
   ```bash
   dotnet run --project src/api && ng serve --project webapp
   ```

## Contribuir

Consulta [CONTRIBUTING.md](CONTRIBUTING-es.md) para más detalles.

## Licencia
Licenciado bajo la licencia MIT. Ver [LICENSE](LICENSE) para más información.
