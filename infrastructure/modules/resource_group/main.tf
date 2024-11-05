# modules/resource_group/main.tf
terraform {
  backend "azurerm" {}
}

resource "azurerm_resource_group" "rg" {
  name     = "rg-tfst-${var.environment}"
  location = var.location
}
