using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    public static class test
    {
        public static int secstomidnight(string qe)
        {
            DateTime utc = DateTime.UtcNow;
            int x = (utc.Second) + (utc.Minute * 60) + (utc.Hour * 60 * 60);
            int utcsecstomidnight = (86400 - x);
            byte _qe = Convert.ToByte(qe, 16);
            byte longit = (byte)(_qe & 0x1f);
            int timezonesecs = longit * 45 * 60;
            return (timezonesecs - utcsecstomidnight + 86400) % 86400;
        }
    }
}
