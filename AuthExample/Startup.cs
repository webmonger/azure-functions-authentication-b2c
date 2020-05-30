using System;
using System.IO;
using AuthExample.AuthTokenBinding;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AuthExample.Startup))]
[assembly: WebJobsStartup(typeof(AuthExample.WebJobsStartup))]

namespace AuthExample
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddOptions();

            // General Setup
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Options Setup
            builder.Services
                .Configure<AuthenticationConfig>(config.GetSection("OpenIdConnectConfig"));
        }
    }

    public class WebJobsStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            Console.WriteLine("WebJobsStartup.Configure");
            builder.AddAccessTokenBinding();
        }
    }
}