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
    DateTime.UtcNow.
    byte _qe = Convert.ToByte(qe, 16);    
    bool east = (_qe & 0x10) == 0;
    byte longit = (byte)(_qe & 0x07);
    int timezonesecs = longit * 45 * 60;
    timezonesecs = (east)? timezonesecs: timezonesecs *= -1;
   // DateTime dt = DateTime.UtcNow;
   // DateTime dt1 = dt.AddDays(1); 
   // TimeSpan ts = dt1 - dt;
   
return (timezonesecs);
}
    }
}
