variable "location" {}
variable "resource_group_name" {}
variable "environment" {}
variable "service_plan_id" {}
variable "tags" {
  type    = map(string)
  default = {}
}

variable "api_url" {
  description = "The URL of the API service"
  type        = string
}
