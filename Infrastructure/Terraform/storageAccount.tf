module "sa" {
  source                  = "C:\\agent\\_work\\1\\s\\CoreTerraformModules/Modules/StorageAccount"
  resource_group_location = azurerm_resource_group.rg.location
  resource_prefix         = var.resource_prefix
  resource_group_name     = azurerm_resource_group.rg.name
  resource_name           = "sa"
  resource_environment    = var.resource_environment
}
