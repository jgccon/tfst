# GitHub Actions Workflows

This directory contains GitHub Actions workflows for **The Full Stack Team** project. Each component (or "container" in C4 terminology) has its own set of workflows organized in separate folders. This structure enables efficient and modular CI/CD processes by allowing workflows to be triggered only when relevant parts of the project change.

## Structure

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

## Adding New Components

To add workflows for a new component (e.g., a notification service):
1. Create a new folder under `.github/workflows` named after the component (e.g., `Notifications`).
2. Add workflows specific to that component, such as `ci.yml` for testing and building, `cd.yml` for deployment, or any other custom workflows.
3. Update this README if needed to reflect the changes.

This structure allows for organized, efficient, and scalable workflow management in GitHub Actions, enhancing the CI/CD process for **The Full Stack Team** project.
