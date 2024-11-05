resource "azurerm_storage_account" "public_storage" {
  name                     = "public${var.environment_name}"
  rg_name                  = var.rg_name
  location                 = var.location
  account_tier             = "Standard"
  account_replication_type = "LRS"

  allow_blob_public_access = true
}
