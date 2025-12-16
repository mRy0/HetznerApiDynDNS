using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HetznerApiDynDNS.Models.Hetzner
{
    public class Pagination
    {
        [JsonPropertyName("last_page")]
        public int LastPage { get; init; }

        [JsonPropertyName("next_page")]
        public int? NextPage { get; init; }

        [JsonPropertyName("page")]
        public int Page { get; init; }

        [JsonPropertyName("per_page")]
        public int PerPage { get; init; }

        [JsonPropertyName("previous_page")]
        public int? PreviousPage { get; init; }

        [JsonPropertyName("total_entries")]
        public int TotalEntries { get; init; }
    }
}
