---
id: intro
title: Community
sidebar_position: 5
---
# Contributing to The Full-Stack Team

Thank you for considering contributing to **The Full-Stack Team**! Your contributions are invaluable for improving the platform and building a robust open-source management solution for freelancers and professionals. This guide provides instructions and best practices for contributing to the project.

## Table of Contents

- [How to Contribute](#how-to-contribute)
- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Branching Strategy](#branching-strategy)
- [Pull Request Process](#pull-request-process)
<!-- - [Contact](#contact) -->

## How to Contribute

We welcome various types of contributions, including:
- Reporting bugs and submitting feature requests.
- Writing documentation and enhancing existing docs.
- Contributing code for new features, bug fixes, and improvements.
- Improving test coverage and ensuring platform stability.

## Code of Conduct

Please read and adhere to our [Code of Conduct](CODE_OF_CONDUCT.md) to foster a welcoming and respectful community.

## Getting Started

1. **Fork the Repository**: 
   - Go to the [repository](https://github.com/JGCarmona-Consulting/tfst) and click on "Fork".
2. **Clone Your Fork**:
   ```bash
   git clone https://github.com/your-username/tfst.git
   cd tfst
   ```
3. **Set Up the Project**:
   - Follow the [Installation Guide](../install/index.md) to set up the backend and frontend locally.

## Branching Strategy

Our project follows the branching strategy below:

- The **dev** branch is the main development branch. All features and fixes should branch off from **dev**.
  - Use branch names such as:
    - `feature/short-description`
    - `fix/issue-id`
    - `docs/short-description`
  - Create Pull Requests (PRs) targeting **dev**.
  - Merged PRs to **dev** are automatically deployed to our testing environment.
- The **main** branch is the production branch.
  - Only a restricted group of contributors can create PRs to **main**.
  - PRs to **main** are used for production deployments.

## Pull Request Process

1. **Create a Branch**: 
   ```bash
   git checkout -b feature/your-feature-name
   ```
2. **Make Your Changes**: Ensure the code is properly formatted, and all tests pass.
3. **Commit and Push**:
   ```bash
   git add .
   git commit -m "Add description of changes"
   git push origin feature/your-feature-name
   ```
4. **Open a Pull Request**:
   - Go to the original repository on GitHub.
   - Click on the **Pull Requests** tab and then **New Pull Request**.
   - Provide a descriptive title and summarize your changes in the pull request description.
   - Link the PR to relevant issues using `#issue-number`.

5. **Review and Merge**: A project maintainer will review your PR. Be prepared to make adjustments based on feedback.

## Communication & Support

For discussions and quick updates, join our [WhatsApp group](https://chat.whatsapp.com/Jnoi9xHbMQ09fJNpxJA0LJ).

If you have questions about the contribution process, please use [GitHub Issues](https://github.com/juangcarmona/tfst/issues) or contact the maintainer directly via email at [juan@jgcarmona.com].
