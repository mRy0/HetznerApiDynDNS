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
                    var currentIp = await Tools.IP.GetCurrentIP4();
                    if (string.IsNullOrWhiteSpace(currentIp) || !IPAddress.TryParse(currentIp, out IPAddress addr))
                        throw new Exception($"current ip is null or wrong: {currentIp}");

                    if (lastIp == currentIp)
                        continue;

                    _logger.LogInformation($"ip has changed from {lastIp} to {currentIp}");

                    var serviceScope = _serviceProvider.CreateScope();
                    var config = serviceScope.ServiceProvider.GetRequiredService<IOptions<AppConfig>>().Value;

                    foreach(var zone in config.Zones)
                    {
                        if (string.IsNullOrWhiteSpace(zone.Name))
                        {
                            _logger.LogWarning("empty zone name");
                            continue;
                        }

                        var records = await Tools.HetznerAPI.GetRecordsForZone(zone.Name, config.Hetzner.ApiKey);
                        var matchedRecords = records.Where(x => zone.Records.Any(y => y.Name == x.Name && y.Type == x.Type) && x.FirstEntry != currentIp);

                        foreach(var recordToUpdate in matchedRecords)
                        {
                            _logger.LogInformation($"updateing record - zone: {zone.Name} - record: {recordToUpdate.Name}/{recordToUpdate.Type}");
                            await Tools.HetznerAPI.UpdateRecord(zone.Name, config.Hetzner.ApiKey, new Models.Record()
                            {
                                TTL = 60,
                                FirstEntry = currentIp,
                                Name = recordToUpdate.Name,
                                Type = recordToUpdate.Type
                            });
                        }
                    }

                    lastIp = currentIp;
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
                finally
                {
                    await Task.Delay(10000, stoppingToken);
                }
            }
        }
    }
}
