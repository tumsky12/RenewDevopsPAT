using System.Threading.Tasks;

namespace AccessTokenFunctionApp;

public interface IRenewPatTokenService
{
    Task<bool> RenewPatToken();
}