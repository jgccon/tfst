# envs/env.hcl
locals {
  environment_name = "dev"
  location         = "westeurope"

  # Select Service Plan SKU
  service_sku = "f1" # We can change this value to "d1" or "b1"

  default_tags = {
    project = "TheFullStackTeam"
    owner   = "Juan G Carmona"
  }
}