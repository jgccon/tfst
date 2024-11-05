# resource_group/main.tf
resource "azurerm_resource_group" "core" {
  name     = var.rg_name
  location = var.location
}

output "rg_name" {
  value = azurerm_resource_group.core.name
}
