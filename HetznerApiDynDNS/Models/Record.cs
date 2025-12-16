using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HetznerApiDynDNS.Models
{
    public class Record
    {
        public string Name { set; get; }
        public string Type { set; get; }
        public int? TTL { set; get; }
        public string FirstEntry { set; get; }
    }
}
