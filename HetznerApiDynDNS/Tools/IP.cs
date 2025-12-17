using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HetznerApiDynDNS.Tools
{
    public static class IP
    {
        public const string API_URL4 = "https://api.ipify.org";
        public async static Task<string> GetCurrentIP4(CancellationToken cancellationToken)
        {
            using var httpClient = new HttpClient();
            return await httpClient.GetStringAsync(API_URL4, cancellationToken);
        }
    }
}
