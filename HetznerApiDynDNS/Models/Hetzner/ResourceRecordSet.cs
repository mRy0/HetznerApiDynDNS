using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HetznerApiDynDNS.Models.Hetzner
{
    public class ResourceRecordSet
    {
        [JsonPropertyName("id")]
        public string Id { get; init; } = string.Empty; // "@ /A", "www/A"

        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty; // "@", "www"

        [JsonPropertyName("type")]
        public string Type { get; init; } = string.Empty; // A, AAAA, NS, SOA

        [JsonPropertyName("ttl")]
        public int? Ttl { get; init; } // null = Zone default

        [JsonPropertyName("labels")]
        public Dictionary<string, string> Labels { get; init; } = new();

        [JsonPropertyName("protection")]
        public Protection Protection { get; init; } = default!;

        [JsonPropertyName("records")]
        public IReadOnlyList<ResourceRecord> Records { get; init; } = Array.Empty<ResourceRecord>();

        [JsonPropertyName("zone")]
        public long Zone { get; init; }

    }
}
