using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessTokenFunctionApp.Infrastructure;

public interface IPatKeyVault
{
    Task<bool> SetPatSecretAsync(string patToken, CancellationToken cancellationToken = default);
}