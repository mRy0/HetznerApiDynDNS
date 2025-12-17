using HetznerApiDynDNS.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace HetznerApiDynDNS
{
    public class Updater : BackgroundService
    {
        private IServiceProvider _serviceProvider;
        private ILogger<Updater> _logger;

        public Updater(IServiceProvider serviceProvider, ILogger<Updater> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string lastIp = "";
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var currentIp = await Tools.IP.GetCurrentIP4(stoppingToken);
                    if (string.IsNullOrWhiteSpace(currentIp) || !IPAddress.TryParse(currentIp, out IPAddress addr))
                        throw new Exception($"current ip is null or wrong: {currentIp}");

                    if (lastIp == currentIp)
                        continue;

                    _logger.LogInformation($"ip has changed from {lastIp} to {currentIp}");

                    var serviceScope = _serviceProvider.CreateScope();
                    var config = serviceScope.ServiceProvider.GetRequiredService<IOptions<AppConfig>>().Value;

                    var hetznerRecords = await Tools.HetznerAPI.GetRecordsForZone(config.ZoneName, config.ApiKey, stoppingToken);

                    var recordsToUpdate = hetznerRecords.Where(
                        x => x.FirstEntry != currentIp && 
                        x.Type == "A" &&
                        config.ARecordNames.Any(y => y == x.Name));

                    foreach(var recordToUpdate in recordsToUpdate)
                    {
                        _logger.LogInformation($"updateing record - zone: {config.ZoneName} - record: {recordToUpdate.Name}/{recordToUpdate.Type}");

                        await Tools.HetznerAPI.UpdateRecord(config.ZoneName, config.ApiKey, new Models.Record()
                        {
                            TTL = recordToUpdate.TTL,
                            FirstEntry = currentIp,
                            Name = recordToUpdate.Name,
                            Type = recordToUpdate.Type
                        }, 
                        stoppingToken
                        );
                    }

                    lastIp = currentIp;
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
                finally
                {
                    await Task.Delay(10000, stoppingToken);
                }
            }
        }
    }
}
