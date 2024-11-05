# envs/env.hcl
locals {
  environment_name = "dev"
  location         = "westeurope"

  default_tags = {
    environment = local.environment_name
    project     = "TheFullStackTeam"
    owner       = "Juan G Carmona"
    GITC-ProdStage = false
  }
}