variable "resource_group_location" {
  default     = "uksouth"
  description = "Location of the resource group."
}

variable "resource_prefix" {
  default     = "pat"
  description = "Prefix of the resource."
}

variable "resource_environment" {
  default = "dev"
  validation {
    condition     = contains(["prd", "stg", "dev"], var.resource_environment)
    error_message = "The environment must be either 'prd', 'stg' or 'dev'."
  }
}

variable "devops_organization_name" {
  description = "Name of devops organization."
}

variable "devops_token_scopes" {
  default     = "app_token"
  description = "The token scopes for accessing Azure DevOps resources."
}

variable "devops_token_all_orgs" {
  default     = true
  type        = bool
  description = "Should the new pat be valid for all accessible organizations."
}
