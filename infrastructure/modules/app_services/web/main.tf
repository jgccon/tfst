terraform {
  backend "azurerm" {}
}

resource "azurerm_linux_web_app" "web_service" {
  name                = "web-${var.environment}"
  location            = var.location
  resource_group_name = var.resource_group_name
  service_plan_id     = var.service_plan_id

  site_config {
    application_stack {
      node_version = "18-lts" # Use application_stack to specify the Node.js version
    }
    https_only = true
  }

  app_settings = {
    "NODE_ENV" = "development"
    "API_URL"  = var.api_url
  }

  tags = var.tags
}

