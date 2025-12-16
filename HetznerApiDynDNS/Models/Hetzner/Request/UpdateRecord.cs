using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HetznerApiDynDNS.Models.Hetzner.Request
{
    public class UpdateRecord
    {
        [JsonPropertyName("ttl")]
        public int? Ttl { get; init; }

        [JsonPropertyName("records")]
        public IReadOnlyList<ResourceRecord> Records { get; init; } = Array.Empty<ResourceRecord>();

    }
}
