# modules/service_plan/outputs.tf
output "service_plan_id" {
  value = azurerm_service_plan.service_plan.id
}
