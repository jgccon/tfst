variable "storage_account_name" {
  description = "Name of the storage account."
  type        = string
}

variable "rg_name" {
  type        = string
  description = "The name of the Resource Group that is used for the resource"
}


variable "location" {
  description = "Azure region."
  type        = string
}
