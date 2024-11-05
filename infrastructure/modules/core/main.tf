# core/main.ts
data "azurerm_client_config" "current" {}
data "azurerm_subscription" "current" {}

resource "azurerm_resource_group" "core" {
  name     = "core-${var.environment_name}"
  location = var.location
  tags     = var.tags
}