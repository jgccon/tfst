
# The Full Stack Team

**The Full Stack Team** is an open-source, comprehensive human resources management platform designed to streamline the administration of employee profiles, projects, contracts, billing, and work hours in a multitenant environment.

## Features

- **Multitenant Support**: Manage multiple clients or departments with data isolation.
- **Employee Management**: Create and maintain employee profiles.
- **Project Assignment**: Assign employees to projects and track progress.
- **Contract Management**: Handle contracts, invoicing, and billing.
- **Time Tracking**: Record and monitor work hours and productivity.
- **Scalable Architecture**: Built with modern technologies for scalability and performance.

## Technologies Used

- **Backend**: .NET (ASP.NET Core)
- **Frontend**: Angular
- **Database**: SQL Server or PostgreSQL, with optional MongoDB
- **Authentication**: JWT, Auth0, or Azure AD B2C
- **Containerization**: Docker
- **CI/CD**: GitHub Actions
- **Cloud Platform**: Azure (primary), adaptable to others

## Installation

### Prerequisites

- **Git**
- **.NET SDK 7.0**
- **Node.js (v16.x) and npm**
- **Angular CLI**
- **Docker and Docker Compose** (optional for containerized setup)

### Steps

1. **Clone the Repository**

   ```bash
   git clone https://github.com/juangcarmona/tfst.git
   cd tfst
   ```

2. **Backend Setup**

   ```bash
   cd src/backend
   dotnet restore
   dotnet build
   ```

3. **Frontend Setup**

   ```bash
   cd ../frontend
   npm install
   ng build
   ```

4. **Run the Application**

   - **Option 1: Run via .NET and Angular CLI**

     - Start the backend:

       ```bash
       cd ../backend
       dotnet run
       ```

     - Start the frontend:

       ```bash
       cd ../frontend
       ng serve
       ```

     - Access the application at `http://localhost:4200`.

   - **Option 2: Run with Docker Compose**

     ```bash
     cd ../../infrastructure
     docker-compose up -d
     ```

     - Access the application at `http://localhost:8080` (or configured port).

## Usage

- **Access the Application**: Navigate to the frontend URL in your browser.
- **Authentication**: Register or log in using the provided authentication mechanism.
- **Explore Features**: Manage employees, projects, contracts, and track time.

# GitHub Actions Workflows

This repository includes a set of GitHub Actions workflows to manage CI/CD processes for **The Full Stack Team** project. Each component (or "container" in C4 terminology) project. Each component has its own workflow to ensure modularity and efficiency.

## Workflow Structure

- **API/**: Contains workflows specific to the backend API.
  - **ci.yml**: Continuous Integration workflow for the API. Runs tests and builds the API on changes to the `API` folder.
  - **pr.yml**: Pull Request workflow for the API. Runs on pull requests targeting the `API` folder, ensuring code quality and passing tests before merging.
  - **cd.yml**: Continuous Deployment workflow for the API. Deploys the API to production when changes are pushed to the main branch in the `API` folder.

- **WebApp/**: Contains workflows specific to the frontend WebApp.
  - **ci.yml**: Continuous Integration workflow for the WebApp. Runs tests and builds the WebApp on changes to the `WebApp` folder.
  - **pr.yml**: Pull Request workflow for the WebApp. Ensures quality checks and passing tests on PRs targeting the `WebApp` folder.
  - **cd.yml**: Continuous Deployment workflow for the WebApp. Deploys the WebApp to production on changes to the main branch in the `WebApp` folder.

## Benefits of This Structure

1. **Component Isolation**: Each component has its own CI/CD workflows, triggered only when changes are made to the corresponding folder. This reduces unnecessary builds and increases efficiency.
2. **Scalability**: New components (e.g., additional services or containers) can be added by creating a new folder with its own workflows.
3. **Modularity**: Keeping workflows separate makes it easier to manage, debug, and modify them independently for each component.

## Documentation

The documentation follows the [Arc42 template](https://arc42.org/), a standardized approach for software architecture documentation, ensuring clarity and consistency across all sections. For a full overview of the template structure, see the [Arc42 Template Guide](docs/arc42-template-EN.md).

Detailed documentation is available in the [`/docs`](docs/README.md) directory, following the Arc42 template.

- [Documentation Index](docs/README.md)
- [Architecture Overview](docs/05_building_block_view.md)
- [API Documentation](docs/API.md) *(to be created)*

## Contributing

We welcome contributions from the community!

- **Fork the Repository**
- **Create a Feature Branch**
- **Commit Your Changes**
- **Open a Pull Request**

Please read our [Contributing Guidelines](CONTRIBUTING.md) for more details.

## License

This project is licensed under the [MIT License](LICENSE).

## Contact

- **Project Maintainer**: [Your Name](mailto:juan@jgcarmona.com)
- **Issues**: Please use the [GitHub Issues](https://github.com/juangcarmona/tfst/issues) for bug reports and feature requests.

---
