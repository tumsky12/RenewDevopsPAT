using System;
using System.Net.Http;
using AccessTokenFunctionApp;
using AccessTokenFunctionApp.Infrastructure;
using AccessTokenFunctionApp.Infrastructure.Interfaces;
using AccessTokenFunctionApp.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(Startup))]

namespace AccessTokenFunctionApp;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {

        builder.Services.AddSingleton<IPatKeyVault, PatKeyVault>();
        builder.Services.AddHttpClient();
        builder.Services.AddSingleton<IDevOpsPatApiWrapper, DevOpsPatApiWrapper>(x =>
            new DevOpsPatApiWrapper(
                DevOpsCredentialHelper.GetToken(),
                x.GetService<IHttpClientFactory>() ?? throw new InvalidOperationException(),
                x.GetService<IConfiguration>() ?? throw new InvalidOperationException(),
                x.GetService<ILogger<DevOpsPatApiWrapper>>() ?? throw new InvalidOperationException())
            );
        builder.Services.AddSingleton<IDevOpsPatRenewer, DevOpsPatRenewer>();
        builder.Services.AddSingleton<IRenewPatTokenService, RenewPatTokenService>();
    }
}
