# core/outputs.tf
output "rg_name" {
  value = azurerm_resource_group.core.name
}

output "region" {
  value = var.location
}

output "client_config" {
  value = {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id
  }
}

output "subscription_id" {
  value = data.azurerm_subscription.current.subscription_id
}