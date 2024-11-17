locals {
  env_vars = read_terragrunt_config(find_in_parent_folders("env.hcl")).locals
}

terraform {
  source = "../../../modules/resource_group"
}


include {
  path = find_in_parent_folders()
}

inputs = {
  environment = "prod"
  location    = "westeurope"
}
