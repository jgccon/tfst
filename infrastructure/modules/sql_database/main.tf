# infrastructure/modules/sql_database/main.tf
# SQL Server
resource "azurerm_mssql_server" "shared" {
  name                         = var.sql_server_name
  resource_group_name          = var.resource_group_name
  location                     = var.location
  version                      = "12.0"
  administrator_login          = var.admin_username
  administrator_login_password = var.admin_password
}

# SQL Database
resource "azurerm_mssql_database" "shared" {
  name         = var.database_name
  server_id    = azurerm_mssql_server.shared.id
  collation    = "SQL_Latin1_General_CP1_CI_AS"  # Default collation
  sku_name     = var.sku_name
  max_size_gb  = var.max_size_gb
  license_type = "LicenseIncluded"

  tags = var.tags

  # Prevent accidental destruction of the database
  lifecycle {
    prevent_destroy = true
  }
}