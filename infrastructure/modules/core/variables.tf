# core/variables.tf
variable "location" {
  type        = string
  description = "The Azure region to be used for all network resources"
}

variable "environment_name" {
  type        = string
  description = "A name for the target environment of the deployment, e.g. \"dev\" or \"prod\""
}

variable "tags" {
  type        = map(string)
  description = "Tags to be used for the network resources"
}

