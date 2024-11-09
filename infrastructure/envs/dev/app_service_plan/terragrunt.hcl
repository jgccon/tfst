dependency "resource_group" {
  config_path = "../resource_group"
}

locals {
  sku = {
    tier     = "Standard"
    size     = "S1"
    capacity = 1
  }

  env_vars = read_terragrunt_config(find_in_parent_folders("env.hcl")).locals
}

terraform {
  source = "../../../modules/app_service_plan"
}

include {
  path = find_in_parent_folders()
}

inputs = {
  environment         = local.env_vars.environment_name
  location            = local.env_vars.location
  resource_group_name = dependency.resource_group.outputs.resource_group_name
  tags                = local.env_vars.default_tags
}
