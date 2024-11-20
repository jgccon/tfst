# The Full Stack Team (TFST)

**The Full Stack Team (TFST)** is an open-source platform designed to streamline project and client management for freelancers and professionals. This platform offers tools for managing clients, projects, contracts, billing, and work hours, all in a multitenant environment.

## Features

- **Multitenant Support**: Manage multiple clients with isolated data.
- **Project Management**: Assign professionals to projects and track progress.
- **Client Management**: Handle contracts, invoicing, and billing.
- **Time Tracking**: Record work hours and monitor productivity.
- **Scalable Architecture**: Built using modern technologies for high performance.

## Technologies Used

- **Backend**: .NET (ASP.NET Core)
- **Frontend**: Angular
- **Databases**: SQL Server, PostgreSQL, Cosmos DB (MongoDB API)
- **Authentication**: JWT, OAuth2, Azure AD B2C
- **Containerization**: Docker
- **CI/CD**: Azure DevOps, GitHub Actions
- **Infrastructure as Code**: Terraform
- **Cloud Platform**: Azure

## Installation

### Prerequisites
Ensure you have the following installed:
- **Git**
- **.NET SDK 8.0**
- **Node.js (v18.x) and npm**
- **Angular CLI**
- **Docker (optional)**

### Steps
1. **Clone the Repository**:
   ```bash
   git clone https://github.com/JGCarmona-Consulting/tfst.git
   cd tfst
   ```

2. **Backend Setup**:
   ```bash
   cd src/api
   dotnet build
   ```

3. **Frontend Setup**:
   ```bash
   cd ../webapp
   npm install
   ng serve
   ```

4. **Run the Application Locally**:
   ```bash
   dotnet run --project src/api
   ng serve --project webapp
   ```

# CI/CD with Azure DevOps

[ALREADY DONE HERE](https://dev.azure.com/jgcarmona/TheFullStackTeam/)

## Contribution Guidelines
We welcome contributions! Please refer to [CONTRIBUTING.md](CONTRIBUTING.md) for more details.

## Documentation
For detailed documentation, refer to the `/docs` folder or visit our [documentation page](docs/README.md).

## License
Licensed under the MIT License. See [LICENSE](LICENSE) for more details.
