variable "resource_group_location" {
  default     = "uksouth"
  description = "Location of the resource group."
}

variable "resource_prefix" {
  default     = "pat"
  description = "Prefix of the resource."
}

variable "resource_environment" {
  default = "prod"
  validation {
    condition     = contains(["prod", "stg", "dev"], var.resource_environment)
    error_message = "The environment must be either 'prod', 'stg' or 'dev'."
  }
}
