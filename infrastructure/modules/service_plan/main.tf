# modules/service_plan/main.tf
resource "azurerm_service_plan" "service_plan" {
  name                = "service-plan-${var.environment}"
  location            = var.location
  resource_group_name = var.resource_group_name
  sku_name            = var.sku_name
  os_type             = var.os_type
  tags                = var.tags
}