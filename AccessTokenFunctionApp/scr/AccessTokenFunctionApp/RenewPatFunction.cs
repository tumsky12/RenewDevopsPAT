using System;
using System.Threading.Tasks;
using AccessTokenFunctionApp.Infrastructure;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AccessTokenFunctionApp;

public class RenewPatFunction
{
    private readonly IPatKeyVault _patKeyVault;
    public RenewPatFunction(IPatKeyVault patKeyVault, IConfiguration configuration)
    {
        _patKeyVault = patKeyVault;
    }

    [FunctionName("RenewPatFunction")]
    public async Task Run([TimerTrigger("*/5 * * * * *")] TimerInfo myTimer, ILogger log)
    {
        var secrets = await _patKeyVault.SetPatSecretAsync("test-token");
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
    }
}
