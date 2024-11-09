# infrastructure/envs/terragrunt.hcl
locals {
  # Load the environment variables from the env.hcl file
  environment_vars = read_terragrunt_config(find_in_parent_folders("env.hcl"))

  # Extract the environment name and location from the env.hcl file
  environment_name        = local.environment_vars.locals.environment_name
  location                = local.environment_vars.locals.location
  tfstate_rg_name         = try(local.environment_vars.locals.tfstate_rg_name, "tfst-tfstate-${local.environment_name}")
  tfstate_storage_account = try(local.environment_vars.locals.tfstate_storage_account, "tfsttfstate${local.environment_name}")
}

# Azure provider configuration
generate "provider" {
  path      = "provider.tf"
  if_exists = "overwrite"
  contents  = <<EOF
    provider "azurerm" {
      features {}
    }
EOF
}

# Terraform version and provider requirements
generate "versions" {
  path      = "versions_override.tf"
  if_exists = "overwrite_terragrunt"
  contents  = <<EOF
    terraform {
      required_version = ">=1.3.0"
      required_providers {
        azurerm = {
          source  = "hashicorp/azurerm"
          version = ">=4.8.0" # Minimum version required for the azurerm_client_config data source
        }
      }
    }
EOF
}

# Configure remote state
remote_state {
  backend = "azurerm"
  config = {
    resource_group_name  = local.tfstate_rg_name
    storage_account_name = local.tfstate_storage_account
    container_name       = "tfstate-container"
    key                  = "${path_relative_to_include()}/terraform.tfstate"
  }
}
