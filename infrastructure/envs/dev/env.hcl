# envs/dev/env.hcl
locals {
  environment_name       = "dev"
  aspnetcore_environment = "Development"
  location               = "westeurope"

  resource_group_name = "rg-tfst-dev"

  # Select Service Plan SKU
  service_sku = "D1" # We can change this value to "D1" or "B1"

  # SQL Database Configuration
  sql_server_name = "sql-server-dev"
  database_name   = "shared-sql-db"
  admin_username  = "sqladmin"
  admin_password  = "SuperSecurePassword123"
  sku_name        = "GP_S_Gen5_2" # Free tier
  max_size_gb     = 32

  default_tags = {
    project = "TheFullStackTeam"
    owner   = "Juan G Carmona"
  }
}