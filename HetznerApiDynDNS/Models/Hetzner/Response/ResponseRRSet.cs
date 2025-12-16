using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HetznerApiDynDNS.Models.Hetzner.Response
{
    public class ResponseRRSet
    {
        [JsonPropertyName("meta")]
        public Meta Meta { get; init; } = default!;

        [JsonPropertyName("rrsets")]
        public IReadOnlyList<ResourceRecordSet> RRSets { get; init; } = Array.Empty<ResourceRecordSet>();
    }
}
