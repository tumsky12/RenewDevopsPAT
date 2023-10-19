module "kv" {
  source                  = "../../../SharedInfrastructure/TerraformModules/KeyVault"
  resource_group_location = azurerm_resource_group.rg.location
  resource_prefix         = var.resource_prefix
  resource_group_name     = azurerm_resource_group.rg.name
  resource_name           = "kv-vm"
  resource_environment    = var.resource_environment
}
