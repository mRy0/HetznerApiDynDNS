using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HetznerApiDynDNS.Models.Hetzner
{
    public class Protection
    {
        [JsonPropertyName("change")]
        public bool Change { get; init; }
    }
}
