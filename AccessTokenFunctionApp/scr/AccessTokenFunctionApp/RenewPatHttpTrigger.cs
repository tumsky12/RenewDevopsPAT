using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AccessTokenFunctionApp;

public class RenewPatHttpTrigger
{
    private readonly IRenewPatTokenService _renewPatTokenService;
    private readonly ILogger<RenewPatHttpTrigger> _logger;
    public RenewPatHttpTrigger(IRenewPatTokenService renewPatTokenService
        , ILogger<RenewPatHttpTrigger> logger)
    {
        _renewPatTokenService = renewPatTokenService;
        _logger = logger;
    }

    [FunctionName("RenewPatRequestTrigger")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
    {
        _logger.LogInformation("RenewPatFunction executed via http request");

        var success = await _renewPatTokenService.RenewPatToken();
        if (success)
        {
            return new OkResult();
        }
        return new InternalServerErrorResult();
    }
}
