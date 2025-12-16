using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HetznerApiDynDNS.Models.Hetzner
{
    public class ResourceRecord
    {
        [JsonPropertyName("value")]
        public string Value { get; init; } = string.Empty;

        [JsonPropertyName("comment")]
        public string? Comment { get; init; }

    }
}
