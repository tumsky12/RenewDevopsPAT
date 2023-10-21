using Microsoft.Extensions.Configuration;
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
    public DevOpsPat(IConfiguration configuration)
    {
        var azureAccessToken = DevOpsCredentialHelper.GetToken();
        var organizationName = configuration.GetValue<string>("DEVOPS_ORGANIZATION_NAME") ?? throw new Exception("DEVOPS_ORGANIZATION_NAME configuration variable not found");
        _devOpsPatApiWrapper = new DevOpsPatApiWrapper(organizationName, azureAccessToken);
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