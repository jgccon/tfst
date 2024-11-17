
# The Full Stack Team

**The Full Stack Team** is an open-source, comprehensive platform for freelancers and professionals to manage clients, projects, contracts, billing, and work hours in a multitenant environment.

## Features

- **Multitenant Support**: Manage multiple clients with data isolation.
- **Project Management**: Assign freelancers to projects and track progress.
- **Client Management**: Handle contracts, invoicing, and billing.
- **Time Tracking**: Record work hours and productivity.
- **Employee Profiles**: Manage profiles and track certifications.
- **Scalable Architecture**: Built on modern technologies for high performance and scalability.

## Technologies Used

- **Backend**: .NET (ASP.NET Core)
- **Frontend**: Angular
- **Database**: SQL Server, PostgreSQL, Cosmos DB (MongoDB API)
- **Authentication**: JWT, OAuth2, Azure AD B2C
- **Containerization**: Docker
- **CI/CD**: Azure DevOps, GitHub Actions
- **Cloud Platform**: Azure, Terraform for infrastructure as code

## Installation

### Prerequisites
- Git, .NET SDK 8.0, Node.js (v18.x), Angular CLI, Docker

### Steps
1. **Clone the Repository**:
   ```bash
   git clone https://github.com/JGCarmona-Consulting/tfst.git
   cd tfst
   ```
2. **Setup Backend & Frontend**:
   ```bash
   cd src/api && dotnet build
   cd ../webapp && npm install
   ```
3. **Run Locally**:
   ```bash
   dotnet run --project src/api && ng serve --project webapp
   ```

## Contributing

Please refer to [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

## License
Licensed under the MIT License. See [LICENSE](LICENSE) for details.
