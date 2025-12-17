using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HetznerApiDynDNS
{
    public class AppConfig
    {
        public string ApiKey { set; get; }
        public string ZoneName { set; get; }
        public string[] ARecordNames { set; get; }
    }


}
