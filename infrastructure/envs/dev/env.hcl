# envs/dev/env.hcl
locals {
  environment_name = "dev"
  location         = "westeurope"

  resource_group_name = "rg-tfst-dev"

  # Select Service Plan SKU
  service_sku = "d1" # We can change this value to "d1" or "b1"

  default_tags = {
    project = "TheFullStackTeam"
    owner   = "Juan G Carmona"
  }
}