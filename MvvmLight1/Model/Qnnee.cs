using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmLight1.Model
{
    static class qnnneee
    {
        static string to_qnnneee(string deccoords) //input -23.56,23.79
        {
            char[] delim = { ',' };
            string[] sarr = deccoords.Split(delim);
            double lat = System.Convert.ToDouble(sarr[0]);
            double lon = System.Convert.ToDouble(sarr[1]);

            int q = ((lat >= 0) && (lon >= 0)) ? 0 :
                     ((lat >= 0) && (lon < 0)) ? 1 :
                     ((lat < 0) && (lon >= 0)) ? 2 : 3;

            int latint = (int)Math.Abs(lat / 180 * 4096);
            int lonint = (int)Math.Abs(lon / 180 * 4096);

            return String.Format("{0:x1}{1:x3}{2:x3}", q, latint, lonint);
        }



        static string to_qnnee(string deccoords) //input -23.56,23.79
        {
            char[] delim = { ',' };
            string[] sarr = deccoords.Split(delim);
            double lat = System.Convert.ToDouble(sarr[0]);
            double lon = System.Convert.ToDouble(sarr[1]);

            int q = ((lat >= 0) && (lon >= 0)) ? 0 :
                     ((lat >= 0) && (lon < 0)) ? 1 :
                     ((lat < 0) && (lon >= 0)) ? 2 : 3;

            int latint = (int)Math.Abs(lat / 180 * 256);
            int lonint = (int)Math.Abs(lon / 180 * 256);

            return String.Format("{0:x1}{1:x2}{2:x2}", q, latint, lonint);
        }

    }


}
