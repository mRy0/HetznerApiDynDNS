using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration.KeyPerFile;
using HetznerApiDynDNS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using HetznerApiDynDNS.Tools;

var appBuilder = Host.CreateApplicationBuilder(args);
var configBuilder = new ConfigurationBuilder();

configBuilder
    .AddCommandLine(args)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile($"appsettings.{appBuilder.Environment.EnvironmentName}.json", optional: true)
    .AddKeyPerFile("/run/secrets", optional: true) 
    .AddEnvironmentVariables(); 

if (appBuilder.Environment.IsDevelopment())
    configBuilder.AddUserSecrets<Program>();

var config = configBuilder.Build();

appBuilder.Services
    .AddOptions<AppConfig>()
    .Bind(config)
    .ValidateOnStart();

var appConfig = config.Get<AppConfig>();

if(appConfig is null)
    throw new Exception("no config");

if (string.IsNullOrWhiteSpace(appConfig.ApiKey))
    throw new Exception("api key is empty");

if (string.IsNullOrWhiteSpace(appConfig.ZoneName))
    throw new Exception("no zone name");

if (appConfig.ARecordNames is null || !appConfig.ARecordNames.Any())
    throw new Exception("no records");


appBuilder.Services.AddLogging(config =>
{
    config.AddSimpleConsole(options =>
    {
        options.SingleLine = true;
        options.TimestampFormat = "HH:mm:ss ";

    });
});

appBuilder.Services.AddHostedService<Updater>();
var app = appBuilder.Build();

var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("");
logger.LogInformation($"apikey: {appConfig.ApiKey.MaskString()}");
logger.LogInformation($"zonename: {appConfig.ZoneName}");
logger.LogInformation($"A-records: {string.Join(',', appConfig.ARecordNames)}");


await app.RunAsync();



