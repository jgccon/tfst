# modules/app_services/web/main.tf
resource "azurerm_linux_web_app" "webapp" {
  name                = "tfst-webapp-${var.environment}"
  location            = var.location
  resource_group_name = var.resource_group_name
  service_plan_id     = var.service_plan_id
  https_only          = true

  site_config {
    application_stack {
      node_version = "18-lts" # Use application_stack to specify the Node.js version
    }
    always_on = var.always_on
  }

  app_settings = {
    "NODE_ENV" = "development"
    "API_URL"  = var.api_url
  }

  tags = var.tags
}

