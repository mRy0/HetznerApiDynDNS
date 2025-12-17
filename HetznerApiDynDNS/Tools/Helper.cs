using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HetznerApiDynDNS.Tools
{
    public static class Helper
    {
        public static string MaskString(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            if (input.Length <= 2)
                return input;

            int keep = Math.Min(4, input.Length / 2);

            return input[..keep]
                 + new string('*', input.Length - keep * 2)
                 + input[^keep..];
        }
    }
}
