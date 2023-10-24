using System;
using System.Threading;
using System.Threading.Tasks;
using AccessTokenFunctionApp.Infrastructure.DevOpsPersonalAccessToken;
using AccessTokenFunctionApp.Infrastructure.Interfaces;

namespace AccessTokenFunctionApp.Infrastructure;

public class DevOpsPat : IDevOpsPat
{
    private const string PatBaseName = "Auto Renewal Pat";
    private const int DaysToExpiry = 30;

    private readonly IDevOpsPatApiWrapper _devOpsPatApiWrapper;
    public DevOpsPat(IDevOpsPatApiWrapper devOpsPatApiWrapper)
    {
        _devOpsPatApiWrapper = devOpsPatApiWrapper;
    }

    public async Task<string?> RenewPat(CancellationToken cancellationToken = default)
    {
        var expiryDate = DateTime.Now.AddDays(DaysToExpiry).Date;
        var name = $"{PatBaseName} - Exp: {expiryDate:d}";

        var patOption = new CreatePersonalAccessTokenOptions(true, name, "app_token", expiryDate.ToString("O"));
        var results = await _devOpsPatApiWrapper.CreateAsync(patOption, cancellationToken);
        return results.PersonalAccessToken?.Token;
    }
    
}