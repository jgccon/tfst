locals {
  env_vars = read_terragrunt_config(find_in_parent_folders("env.hcl")).locals
}

terraform {
  source = "../../../../modules/app_services/api"
}

include {
  path = find_in_parent_folders()
}

dependency "service_plan" {
  config_path = "../../service_plan"
}

dependency "resource_group" {
  config_path = "../../resource_group"
}

inputs = {
  environment         = local.env_vars.environment_name
  location            = local.env_vars.location
  resource_group_name = dependency.resource_group.outputs.resource_group_name
  service_plan_id = dependency.service_plan.outputs.service_plan_id
  tags                = local.env_vars.default_tags

  api_app_settings = {
    "ASPNETCORE_ENVIRONMENT" = "Development"
  }
}
