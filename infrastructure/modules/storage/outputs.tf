# infrastructure/modules/storage/outputs.tf
output "storage_account_name" {
  value = azurerm_storage_account.storage.name
}

output "storage_container_name" {
  value = azurerm_storage_container.public_container.name
}


