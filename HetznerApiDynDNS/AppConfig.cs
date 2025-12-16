using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HetznerApiDynDNS
{
    public class AppConfig
    {
        public HetznerConfig Hetzner { set; get; }
        public ZoneConfig[] Zones { set; get; }

        public sealed class HetznerConfig
        {
            public string ApiKey { set; get; }
        }

        public sealed class ZoneConfig
        {
            public string Name { set; get; }
            public ZoneRecordConfig[] Records { set; get; }
        }

        public sealed class ZoneRecordConfig
        {
            public string Name { set; get; }
            public string Type { set; get; }
        }



    }


}
