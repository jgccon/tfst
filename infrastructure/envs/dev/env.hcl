# envs/env.hcl
locals {
  environment_name = "dev"
  location         = "westeurope"

  default_tags = {
    project     = "TheFullStackTeam"
    owner       = "Juan G Carmona"
  }
}