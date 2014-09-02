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

        static string IndexPoint(string qnnee)
        {
            double lat = 0, lon = 0;

            if (qnnee.Length == 5)
            {
                Int16 _lat = Convert.ToInt16(qnnee.Substring(1, 2));
                Int16 _lon = Convert.ToInt16(qnnee.Substring(3, 2));
                lat = _lat / 256 * 180;
                lon = _lon / 256 * 180;
            }
            else if (qnnee.Length == 7)
            {
                Int16 _lat = Convert.ToInt16(qnnee.Substring(1, 3));
                Int16 _lon = Convert.ToInt16(qnnee.Substring(4, 3));
                lat = _lat / 4096 * 180;
                lon = _lon / 4096 * 180;
            }
            if ((qnnee[0] == '1') || (qnnee[0] == '3')) lon *= -1;
            if ((qnnee[0] == '2') || (qnnee[0] == '3')) lat *= -1;
            return String.Format("{0:F2}{1:F2}", lat, lon);

        }
    }


}
