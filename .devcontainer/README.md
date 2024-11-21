
# Dockerfile for `act` with Azure CLI Support

This container is designed to run GitHub Actions workflows locally using `act`, including support for Azure CLI (`az`) and additional tools like `terragrunt`.

## Instructions

### 1. Build the Image
Run the following command from the root of the repository:
```bash
docker build -t my-act-image -f .devcontainer/Dockerfile .
```

### 2. Use the Image with `act`
When executing `act`, specify the custom image for the platforms used in your workflows:
```bash
act workflow_dispatch -W .github/workflows/api-dev-cd.yml -P ubuntu-latest=my-act-image:latest --pull=false
```

## Tools Included
- Azure CLI
- Terragrunt
- jq
- curl

## Purpose
This container ensures a consistent environment for testing workflows locally, particularly those involving Azure resources.
