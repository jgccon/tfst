variable "location" {}
variable "resource_group_name" {}
variable "environment" {}
variable "service_plan_id" {}
variable "tags" {
  type    = map(string)
  default = {}
}

variable "always_on" {
  description = "Enable Always On setting for the Web App"
  type        = bool
}