using System.Threading.Tasks;
using AccessTokenFunctionApp.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace AccessTokenFunctionApp.Services;

public class RenewPatTokenService : IRenewPatTokenService
{
    private readonly IPatKeyVault _patKeyVault;
    private readonly IDevOpsPat _devOpsPat;
    private readonly ILogger<RenewPatTokenService> _logger;
    public RenewPatTokenService(IPatKeyVault patKeyVault,
        IDevOpsPat devOpsPat,
        ILogger<RenewPatTokenService> logger)
    {
        _patKeyVault = patKeyVault;
        _devOpsPat = devOpsPat;
        _logger = logger;
    }

    public async Task<bool> RenewPatToken()
    {
        var newPersonalAccessToken = await _devOpsPat.RenewPat();
        if (newPersonalAccessToken == null)
        {
            _logger.LogError("Failed to renew pat");
            return false;
        }

        _logger.LogInformation("Successfully renewed pat");
        var success = await _patKeyVault.SetPatSecretAsync(newPersonalAccessToken);
        if (!success)
        {
            _logger.LogError("Failed to update key vault secret");
        }

        _logger.LogInformation("Successfully updated key vault secret");
        return success;
    }
}
