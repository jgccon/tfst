# /infrastructure/modules/app_services/api/outputs.tf
output "api_url" {
  value = azurerm_linux_web_app.api.default_hostname
}
