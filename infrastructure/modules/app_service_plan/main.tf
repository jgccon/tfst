resource "azurerm_app_service_plan" "app_service_plan" {
  name                = "app-service-plan-${var.environment}"
  location            = var.location
  resource_group_name = var.resource_group_name
  sku {
    tier     = var.sku.tier
    size     = var.sku.size
    capacity = var.sku.capacity
  }
  tags = var.tags
}