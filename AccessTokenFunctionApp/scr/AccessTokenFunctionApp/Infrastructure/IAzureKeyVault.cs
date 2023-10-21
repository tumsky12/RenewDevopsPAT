using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessTokenFunctionApp.Infrastructure;

internal interface IAzureKeyVault
{
    Task<IEnumerable<string>> ListSecretsAsync(CancellationToken cancellation);
    Task<string> GetSecretAsync(string secretName, CancellationToken cancellationToken);
    Task SetSecretAsync(string secretName, string secretValue, CancellationToken cancellationToken);
    Task DeleteSecretAsync(string secretName, CancellationToken cancellationToken);
}

