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


        /// <summary>
        /// Converts decimal latlon string to hexadecimal
        /// </summary>
        /// <param name="deccoords">csv latlon string eg-23.56,23.79 </param>
        /// <returns>qnnee format latlon string</returns>
        static string to_qnnee(string deccoords)
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


        /// <summary>
        /// latlon string point closest to meridian and equator 
        /// </summary>
        /// <param name="qnnee">5 or 7 character hex </param>
        /// <returns>latlon string  as csv</returns>
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
            return String.Format("{0:F2},{1:F2}", lat, lon);

        }
        /// <summary>
        /// calculateds the centre point of a qnnee region
        /// </summary>
        /// <param name="qnnee">qnnee</param>
        /// <returns>decimal string latlon formar</returns>
        public static string CentrePoint(string qnnee)
        {
            double lat = 0, lon = 0, lat1 = 0, lon1 = 0;

            if (qnnee.Length == 5)
            {
                Int16 _lat = Convert.ToInt16(qnnee.Substring(1, 2), 16);
                Int16 _lon = Convert.ToInt16(qnnee.Substring(3, 2), 16);
                lat = ((double)_lat / 256 * 180);
                lat1 = ((double)(_lat + 1) / 256 * 180);
                lon = ((double)_lon / 256 * 180);
                lon1 = ((double)(_lon + 1) / 256 * 180);

            }
            else if (qnnee.Length == 7)
            {
                Int16 _lat = Convert.ToInt16(qnnee.Substring(1, 3), 16);
                Int16 _lon = Convert.ToInt16(qnnee.Substring(4, 3), 16);
                lat = (_lat / 4096 * 180);
                lat1 = ((double)(_lat + 1) / 256 * 180);
                lon = (_lon / 4096 * 180);
                lon1 = ((double)(_lon + 1) / 256 * 180);
            }
            lon = (lon + lon1) / 2;
            lat = (lat + lat1) / 2;
            if ((qnnee[0] == '1') || (qnnee[0] == '3')) lon *= -1;
            if ((qnnee[0] == '2') || (qnnee[0] == '3')) lat *= -1;
            return String.Format(@"{0:F4},{1:F4}", lat, lon); //comma separated
        }

        /// <summary>
        /// constructs a 3 point boundary from 5 or 7 digit qnnee index point for bing map polygon
        /// </summary>
        /// <param name="qnnee"></param>
        /// <returns>3 latlon csv coordinates space separates</returns>
        public string Boundary(string qnnee)
        {
            int len = qnnee.Length;
            if (!((len == 5) || (len == 7))) return "error";

            UInt16 lat, lat1, lon, lon1, q, q1;

            lat = lat1 = lon = lon1 = q = q1 = 0;

            q = Convert.ToUInt16(qnnee.Substring(0, 1), 16);

            if (len == 5)
            {
                lat = Convert.ToUInt16(qnnee.Substring(1, 2), 16);
                lon = Convert.ToUInt16(qnnee.Substring(3, 2), 16);
                if (lat < 127)
                {
                    lat1 = (UInt16)(lat + 1);
                    if (lon < 255) lon1 = (UInt16)(lon + 1);
                    else q = (UInt16)(3 - q);  //swap East West
                }
            }
            else //len==7
            {
                lat = Convert.ToUInt16(qnnee.Substring(1, 3), 16);
                lon = Convert.ToUInt16(qnnee.Substring(4, 3), 16);
                if (lat < 2047)
                {
                    lat1 = (UInt16)(lat + 1);
                    if (lon < 4095) lon1 = (UInt16)(lon + 1);
                    else q = (UInt16)(3 - q);
                }
            }

            bool digt5 = (qnnee.Length == 5);


            float latf = (digt5) ? 128 : 2048;
            float lonf = (digt5) ? 521 : 4096;
            float latf1 = (digt5) ? 128 : 2048;
            float lonf1 = (digt5) ? 521 : 4096;




            return String.Format(@"{0:F4},{1:F4}", lat, lon);
        }
    }


}
