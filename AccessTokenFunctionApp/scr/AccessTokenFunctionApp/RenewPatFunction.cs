using System;
using System.Threading.Tasks;
using AccessTokenFunctionApp.Infrastructure.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AccessTokenFunctionApp;

public class RenewPatFunction
{
    private readonly IPatKeyVault _patKeyVault;
    private readonly IDevOpsPat _devOpsPat;
    public RenewPatFunction(IPatKeyVault patKeyVault,
        IDevOpsPat devOpsPat)
    {
        _patKeyVault = patKeyVault;
        _devOpsPat = devOpsPat;
    }

    [FunctionName("RenewPatFunction")]
    public async Task Run([TimerTrigger("* * */10 * * *")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"RenewPatFunction executed at: {DateTime.Now}. Is past due: {myTimer.IsPastDue}");
        var newPersonalAccessToken = await _devOpsPat.RenewPat();
        if (newPersonalAccessToken != null)
        {
            await _patKeyVault.SetPatSecretAsync(newPersonalAccessToken);
        }
    }
}
