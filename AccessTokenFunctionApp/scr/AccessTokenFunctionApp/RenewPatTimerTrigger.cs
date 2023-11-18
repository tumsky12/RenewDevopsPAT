using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AccessTokenFunctionApp;

public class RenewPatTimerTrigger
{
    private readonly IRenewPatTokenService _renewPatTokenService;
    private readonly ILogger<RenewPatTimerTrigger> _logger;
    public RenewPatTimerTrigger(IRenewPatTokenService renewPatTokenService,
        ILogger<RenewPatTimerTrigger> logger)
    {
        _renewPatTokenService = renewPatTokenService;
        _logger = logger;
    }

    [FunctionName("RenewPatTimerTrigger")]
    public async Task Run([TimerTrigger("* */1 * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("RenewPatFunction executed via trigger timer. Is past due {IsPastDue}", myTimer.IsPastDue);
        await _renewPatTokenService.RenewPatToken();
    }
}
