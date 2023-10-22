module "kv" {
  source                  = "C:\\agent\\_work\\1\\s\\CoreTerraformModules/Modules/KeyVault"
  resource_group_location = azurerm_resource_group.rg.location
  resource_prefix         = var.resource_prefix
  resource_group_name     = azurerm_resource_group.rg.name
  resource_name           = "kv-vm"
  resource_environment    = var.resource_environment
}

resource "azurerm_role_assignment" "ra_kv_admin_current" {
  scope                = module.kv.key_vault_id
  role_definition_name = "Key Vault Administrator"
  principal_id         = azurerm_windows_function_app.wfa.identity[0].principal_id
}
