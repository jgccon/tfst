# /infrastructure/modules/app_services/api/outputs.tf
output "default_hostname" {
  value = azurerm_linux_web_app.api.default_hostname
}

output "name" {
  value = azurerm_linux_web_app.api.name
}