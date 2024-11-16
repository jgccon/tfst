# infrastructure/envs/dev/storage/terragrunt.hcl
locals {
  env_vars = read_terragrunt_config(find_in_parent_folders("env.hcl")).locals
}

terraform {
  source = "../../../modules/storage"
}

include {
  path = find_in_parent_folders()
}

dependency "resource_group" {
  config_path = "../resource_group"
}

inputs = {
  storage_account_name = "tfstpublicstorage${local.env_vars.environment_name}"
  resource_group_name  = dependency.resource_group.outputs.resource_group_name
  location             = local.env_vars.location
  tags                 = local.env_vars.default_tags
}
