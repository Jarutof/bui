using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bui
{
    public static class Version
    {
        public static string Current => $"БУИ 1.11/{PDU}";
        public static string BKN { get; set; }
        public static string PDU { get; set; }
    }
}
