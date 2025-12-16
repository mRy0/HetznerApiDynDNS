using HetznerApiDynDNS.Models;
using HetznerApiDynDNS.Models.Hetzner.Request;
using HetznerApiDynDNS.Models.Hetzner.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HetznerApiDynDNS.Tools
{
    public class HetznerAPI
    {
        private const string API_URL = "https://api.hetzner.cloud/v1";
        private static string ReqAllRRset(string zone) => $"{API_URL}/zones/{zone}/rrsets";
        private static string ReqUpdateUrl(string zone, string recordName, string recordType) 
            => $"{API_URL}/zones/{zone}/rrsets/{recordName}/{recordType}/actions/set_records";

        public async static Task <IEnumerable<Record>> GetRecordsForZone(string zoneName, string apiToken)
        {            
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");

            var responseSet = await  httpClient.GetFromJsonAsync<ResponseRRSet>(ReqAllRRset(zoneName));

            if (responseSet is null)
                throw new Exception("bad response");

            return responseSet.RRSets.Select(x => new Record()
            {
                Name = x.Name,
                TTL = x.Ttl,
                Type = x.Type,
                FirstEntry = x.Records.Select(x => x.Value).FirstOrDefault() ?? ""
            });
        }

        public async static Task UpdateRecord (string zoneName, string apiToken, Record record)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");

            var updateReq = new UpdateRecord()
            {
                Ttl = record.TTL,
                Records = new[] {
                    new Models.Hetzner.ResourceRecord()
                        {
                            Value = record.FirstEntry,
                            Comment = ""
                        }
                    }
            };

            var url = ReqUpdateUrl(zoneName, record.Name, record.Type);
            //var data = JsonSerializer.Serialize(updateReq);
            var updateResponse = await httpClient.PostAsJsonAsync(url, updateReq);

            //var info = await updateResponse.Content.ReadAsStringAsync();
            updateResponse.EnsureSuccessStatusCode();

        }
    }
}
