resource "azurerm_windows_function_app" "wfa" {
  name                = "${var.resource_prefix}-wfa-${var.resource_environment}"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location

  storage_account_name       = module.sa.storage_account_name
  storage_account_access_key = module.sa.primary_access_key
  #storage_uses_managed_identity = true
  #functions_extension_version = "~4"
  service_plan_id = azurerm_service_plan.sp.id

  app_settings = {
    "PAT_KEY_VAULT_NAME"       = module.kv.key_vault_name
    "DEVOPS_ORGANIZATION_NAME" = var.devops_organization_name
    "DEVOPS_TOKEN_SCOPES"      = var.devops_token_scopes
    "DEVOPS_TOKEN_ALL_ORGS"    = var.devops_token_all_orgs
  }

  identity {
    type = "SystemAssigned"
  }

  site_config {}
}
