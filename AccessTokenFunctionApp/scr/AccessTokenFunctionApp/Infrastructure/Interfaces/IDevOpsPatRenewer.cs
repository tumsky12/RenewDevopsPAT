using System.Threading;
using System.Threading.Tasks;

namespace AccessTokenFunctionApp.Infrastructure.Interfaces;

public interface IDevOpsPatRenewer
{
    Task<string?> Renew(CancellationToken cancellationToken = default);
}