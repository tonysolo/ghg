using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    static class tester
    {
        public static string to_qnnneee(string deccoords) //input decimals -23.56,23.79
        {                                                 //output qnnneee
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



        public static string to_qnnee(string deccoords) //input -23.56,23.79 output qnnee
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


        public static string IndexPoint(string qnnee)  //takes qnnee or qnnneee result = minpoint/refpoint
        {
            double lat = 0, lon = 0;

            if (qnnee.Length == 5)
            {
                Int16 _lat = Convert.ToInt16(qnnee.Substring(1, 2), 16);
                Int16 _lon = Convert.ToInt16(qnnee.Substring(3, 2), 16);
                lat = ((double)_lat / 256 * 180);
                lon = ((double)_lon / 256 * 180);
            }
            else if (qnnee.Length == 7)
            {
                Int16 _lat = Convert.ToInt16(qnnee.Substring(1, 3), 16);
                Int16 _lon = Convert.ToInt16(qnnee.Substring(4, 3), 16);
                lat = (_lat / 4096 * 180);
                lon = (_lon / 4096 * 180);
            }
            if ((qnnee[0] == '1') || (qnnee[0] == '3')) lon *= -1;
            if ((qnnee[0] == '2') || (qnnee[0] == '3')) lat *= -1;
            return String.Format("{0:F4},{1:F4}", lat, lon); //comma separated
        }


        public static string Boundary(string qnnee)
        {
            if ((qnnee.Length != 5) || (qnnee.Length != 7)) return "";

            string[] saa = new string[4];
            Int16 _lat0, _lon0, _lat1, _lon1;
            Int16 q = Convert.ToInt16(qnnee.Substring(0, 1), 16);
            saa[0] = IndexPoint(qnnee);
            

            if (qnnee.Length == 5)
            {
                _lat0 = Convert.ToInt16(qnnee.Substring(1, 2), 16);
                _lon0 = Convert.ToInt16(qnnee.Substring(3, 2), 16);
                _lat1 = (Int16)(_lat0 + 1);
                _lon1 = (Int16)(_lon0 + 1);
                
                saa[1] = IndexPoint(String.Format("{0:x1}{1:x2}{2:x2}", q, _lat0, _lon1));
                saa[2] = IndexPoint(String.Format("{0:x1}{1:x2}{2:x2}", q, _lat1, _lon1));
                saa[3] = IndexPoint(String.Format("{0:x1}{1:x2}{2:x2}", q, _lat1, _lon0));
            }
            else
            {
                _lat0 = Convert.ToInt16(qnnee.Substring(1, 3), 16);
                _lon0 = Convert.ToInt16(qnnee.Substring(4, 3), 16);
                _lat1 = (Int16)(_lat0 + 1);
                _lon1 = (Int16)(_lon0 + 1);
                
                saa[1] = IndexPoint(String.Format("{0:x1}{1:x3}{2:x3}", q, _lat0, _lon1));
                saa[2] = IndexPoint(String.Format("{0:x1}{1:x3}{2:x3}", q, _lat1, _lon1));
                saa[3] = IndexPoint(String.Format("{0:x1}{1:x3}{2:x3}", q, _lat1, _lon0));
            }

            return String.Join(",", saa);
            // might need to use a different separator or make sure that the decimal point is never a comma
            // coordinate format points

        }
    }
}
