# dev/storage/terragrunt.htl

locals {
  # Automatically load environment-level variables
  env_vars = read_terragrunt_config(find_in_parent_folders("env.hcl")).locals
}

terraform {
  source = "../../../modules/storage"
}

include {
  path = find_in_parent_folders()
}


dependency "core" {
  config_path = "../core"
}

inputs = {
  location         = local.env_vars.location
  environment_name = local.env_vars.environment_name
  tags             = local.env_vars.default_tags
  rg_name          = dependency.core.outputs.rg_name
}

