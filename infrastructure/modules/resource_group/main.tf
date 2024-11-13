# modules/resource_group/main.tf
resource "azurerm_resource_group" "rg" {
  name     = "rg-tfst-${var.environment}"
  location = var.location
}
