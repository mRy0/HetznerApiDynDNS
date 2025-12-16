using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration.KeyPerFile;
using HetznerApiDynDNS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddCommandLine(args)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddKeyPerFile("/run/secrets", optional: true)   // Docker Secrets
    .AddEnvironmentVariables();                      // ENV gewinnt immer

if (builder.Environment.IsDevelopment())
    builder.Configuration.AddUserSecrets<Program>();

builder.Services
    .AddOptions<AppConfig>()
    .Bind(builder.Configuration)
    .ValidateOnStart();

builder.Services.AddLogging(config =>
{
    config.AddSimpleConsole(options =>
    {
        options.SingleLine = true;
        options.TimestampFormat = "HH:mm:ss ";

    });
});


builder.Services.AddHostedService<Updater>();

var app = builder.Build();

var config = app.Services.GetRequiredService<IOptions<AppConfig>>().Value;

if (string.IsNullOrWhiteSpace(config.Hetzner.ApiKey))
    throw new Exception("api key is empty");

await app.RunAsync();


