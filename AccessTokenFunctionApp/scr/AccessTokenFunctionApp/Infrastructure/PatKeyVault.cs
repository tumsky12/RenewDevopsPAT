using System;
using System.Threading;
using System.Threading.Tasks;
using AccessTokenFunctionApp.Infrastructure.Interfaces;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AccessTokenFunctionApp.Infrastructure;

public class PatKeyVault : IPatKeyVault
{
    private const string PatSecretName = "pat-token";
    private readonly SecretClient _secretClient;
    private readonly ILogger<PatKeyVault> _logger;
    public PatKeyVault(IConfiguration configuration, ILogger<PatKeyVault> logger)
    {
        var patKeyVaultName = configuration.GetValue<string>("PAT_KEY_VAULT_NAME") ?? throw new Exception("PAT_KEY_VAULT_NAME configuration variable not found");
        var keyVaultUri = $"https://{patKeyVaultName}.vault.azure.net";
        _secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
        
        _logger = logger;
    }

    public async Task<bool> SetPatSecretAsync(string patToken, CancellationToken cancellationToken = default)
        => await SetSecretAsync(PatSecretName, patToken, cancellationToken);

    private async Task<bool> SetSecretAsync(string name, string secretValue, CancellationToken cancellationToken = default)
    {
        var secret = await _secretClient.SetSecretAsync(name, secretValue, cancellationToken);
        return secret.HasValue;
    }

}