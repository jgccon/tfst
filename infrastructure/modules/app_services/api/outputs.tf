output "api_service_default_hostname" {
  value = azurerm_linux_web_app.api_service.default_hostname
}

output "api_url" {
  value = azurerm_app_service.api.default_site_hostname
}