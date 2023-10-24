using AccessTokenFunctionApp;
using AccessTokenFunctionApp.Infrastructure;
using AccessTokenFunctionApp.Infrastructure.Interfaces;
using AccessTokenFunctionApp.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace AccessTokenFunctionApp;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {

        builder.Services.AddSingleton<IPatKeyVault, PatKeyVault>();
        builder.Services.AddHttpClient();
        builder.Services.AddSingleton<IDevOpsPatApiWrapper, DevOpsPatApiWrapper>();
        builder.Services.AddSingleton<IDevOpsPat, DevOpsPat>();
        builder.Services.AddSingleton<IRenewPatTokenService, RenewPatTokenService>();
    }
}
