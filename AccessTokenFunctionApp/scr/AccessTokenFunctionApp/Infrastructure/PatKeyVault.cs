using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

namespace AccessTokenFunctionApp.Infrastructure;

public class PatKeyVault : IPatKeyVault
{
    private const string PatSecretName = "pat-token";
    private readonly SecretClient _secretClient;
    public PatKeyVault(IConfiguration configuration)
    {

        var patKeyVaultName = configuration.GetValue<string>("PAT_KEY_VAULT_NAME") ?? throw new Exception("PAT_KEY_VAULT_NAME configuration variable not found");
        var keyVaultUri = $"https://{patKeyVaultName}.vault.azure.net";
        _secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
    }

    public async Task<bool> SetPatSecretAsync(string patToken, CancellationToken cancellationToken = default)
        => await SetSecretAsync(PatSecretName, patToken, cancellationToken);

    private async Task<bool> SetSecretAsync(string name, string secretValue, CancellationToken cancellationToken = default)
    {
        var secret = await _secretClient.SetSecretAsync(name, secretValue, cancellationToken);
        return secret.HasValue;
    }

}