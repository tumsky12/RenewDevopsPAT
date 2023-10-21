using System.Threading;
using System.Threading.Tasks;

namespace AccessTokenFunctionApp.Infrastructure.Interfaces;

public interface IDevOpsPat
{
    Task<string?> RenewPat(CancellationToken cancellationToken = default);
}