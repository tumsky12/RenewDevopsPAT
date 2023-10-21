﻿using AccessTokenFunctionApp;
using AccessTokenFunctionApp.Infrastructure;
using AccessTokenFunctionApp.Infrastructure.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace AccessTokenFunctionApp;

public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IPatKeyVault, PatKeyVault>();
            builder.Services.AddSingleton<IDevOpsPat, DevOpsPat>();
        }
    }
