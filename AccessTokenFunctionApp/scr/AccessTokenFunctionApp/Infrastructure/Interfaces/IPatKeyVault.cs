using System.Threading;
using System.Threading.Tasks;

namespace AccessTokenFunctionApp.Infrastructure.Interfaces;

public interface IPatKeyVault
{
    Task<bool> SetPatSecretAsync(string patToken, CancellationToken cancellationToken = default);
}