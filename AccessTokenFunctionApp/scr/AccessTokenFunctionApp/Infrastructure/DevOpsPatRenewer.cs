using System;
using System.Threading;
using System.Threading.Tasks;
using AccessTokenFunctionApp.Infrastructure.DevOpsPersonalAccessToken;
using AccessTokenFunctionApp.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace AccessTokenFunctionApp.Infrastructure;

public class DevOpsPatRenewer : IDevOpsPatRenewer
{
    private const string PatBaseName = "Auto Renewal Pat";
    private const int DaysToExpiry = 30;

    private readonly IDevOpsPatApiWrapper _devOpsPatApiWrapper;
    private readonly ILogger<DevOpsPatRenewer> _logger;
    public DevOpsPatRenewer(IDevOpsPatApiWrapper devOpsPatApiWrapper, ILogger<DevOpsPatRenewer> logger)
    {
        _devOpsPatApiWrapper = devOpsPatApiWrapper;
        _logger = logger;
    }

    public async Task<string?> Renew(CancellationToken cancellationToken = default)
    {
        var expiryDate = DateTime.Now.AddDays(DaysToExpiry).Date;
        var name = $"{PatBaseName} - Exp: {expiryDate:d}";

        var patOption = new CreatePersonalAccessTokenOptions(true, name, "app_token", expiryDate.ToString("O"));
        var results = await _devOpsPatApiWrapper.CreateAsync(patOption, cancellationToken);
        return results.PersonalAccessToken?.Token;
    }

}