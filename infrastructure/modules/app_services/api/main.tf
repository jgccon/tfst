terraform {
  backend "azurerm" {}
}
resource "azurerm_linux_web_app" "api_service" {
  name                = "api-${var.environment}"
  location            = var.location
  resource_group_name = var.resource_group_name
  service_plan_id     = var.service_plan_id

  site_config {
    application_stack {
      dotnet_version = "8.0" # Use application_stack to specify the .NET version
    }
    https_only = true
  }

  app_settings = {
    "ASPNETCORE_ENVIRONMENT" = "Development"
  }

  tags = var.tags
}
