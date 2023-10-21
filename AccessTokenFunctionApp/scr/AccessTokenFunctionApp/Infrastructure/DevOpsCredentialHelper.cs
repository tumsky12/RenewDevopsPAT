using Azure.Identity;
using Azure.Core;

namespace AccessTokenFunctionApp.Infrastructure;

public static class DevOpsCredentialHelper
{
    // Azure DevOps resource and all of its scopes: https://learn.microsoft.com/en-us/azure/devops/organizations/accounts/manage-personal-access-tokens-via-api?view=azure-devops
    private const string DevOpsScope = "499b84ac-1321-427f-aa17-267ca6975798/.default";

    public static string GetToken()
    {
        var azureCredential = new DefaultAzureCredential();
        var tokenRequestContext = new TokenRequestContext(new[] { DevOpsScope });
        var azureAccessToken = azureCredential.GetToken(tokenRequestContext);
        return azureAccessToken.Token;
    }
}
