locals {
  # Automatically load environment-level variables
  environment_vars = read_terragrunt_config(find_in_parent_folders("env.hcl"))

  # Extract necessary variables for tfstate storage dynamically based on environment name
  environment_name        = local.environment_vars.locals.environment_name
  location                = local.environment_vars.locals.location
  tfstate_rg_name         = try(local.environment_vars.locals.tfstate_rg_name, "tfst-tfstate-${local.environment_name}")
  tfstate_storage_account = try(local.environment_vars.locals.tfstate_storage_account, "tfsttfstate${local.environment_name}")
}

# Generate the Azure provider block
generate "provider" {
  path      = "provider.tf"
  if_exists = "overwrite_terragrunt"
  contents  = <<EOF
    provider "azurerm" {
      features {}
    }
EOF
}

# Generate Terraform version and provider configuration
generate "versions" {
  path      = "versions_override.tf"
  if_exists = "overwrite_terragrunt"
  contents  = <<EOF
    terraform {
      required_version = ">=1.3.0"
      required_providers {
        azurerm = {
          source  = "hashicorp/azurerm"
          version = "4.8.0"
        }
      }
    }
EOF
}

# Configure remote state storage in Azure dynamically for the environment
remote_state {
  backend = "azurerm"
  config = {
    resource_group_name  = local.tfstate_rg_name
    storage_account_name = local.tfstate_storage_account
    container_name       = "tfstate-container"
    key                  = "${path_relative_to_include()}/terraform.tfstate"
  }
  generate = {
    path      = "backend.tf"
    if_exists = "overwrite_terragrunt"
  }
}
