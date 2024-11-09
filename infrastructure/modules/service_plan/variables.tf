# modules/app_service_plan/variables.tf
variable "location" {}
variable "resource_group_name" {}
variable "environment" {}

variable "sku_name" {
  type    = string
  default = "F1" # Free tier
}

variable "os_type" {
  type    = string
  default = "Linux"
}

variable "tags" {
  type    = map(string)
  default = {}
}
