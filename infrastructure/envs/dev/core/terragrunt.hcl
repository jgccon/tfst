# dev/core/terragrunt.htl
locals {
  # Automatically load environment-level variables
  env_vars = read_terragrunt_config(find_in_parent_folders("env.hcl")).locals
}

terraform {
  source = "../../../modules/core"
}

include {
  path = find_in_parent_folders()
}

inputs = {
  location         = local.env_vars.location
  environment_name = local.env_vars.environment_name
  tags             = local.env_vars.default_tags
}