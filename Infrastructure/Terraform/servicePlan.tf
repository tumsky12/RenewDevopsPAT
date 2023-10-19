resource "azurerm_service_plan" "sp" {
  name                = "${var.resource_prefix}-sp-${var.resource_environment}"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  os_type             = "Windows"
  sku_name            = "Y1"
}
