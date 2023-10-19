resource "azurerm_resource_group" "rg" {
  location = var.resource_group_location
  name     = "${var.resource_prefix}-rg-${var.resource_environment}"
}
