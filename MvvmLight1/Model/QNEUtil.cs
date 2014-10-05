using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmLight1.Model
{
    static class QNE_Utils
    {

        /// <summary>
        /// Converts decimal degrees string to district level (7) qnnneee
        /// </summary>
        /// <param name="deccoords">string</param>
        /// <returns>string qnnneee</returns>
        public static string to_qnnneee(string deccoords) //input -23.56,23.79
        {

            char[] delim = { ',' };
            string[] sarr = deccoords.Split(delim);
            double lat = System.Convert.ToDouble(sarr[0]);
            double lon = System.Convert.ToDouble(sarr[1]);

            int q = ((lat >= 0) && (lon >= 0)) ? 0 :
                     ((lat >= 0) && (lon < 0)) ? 1 :
                     ((lat < 0) && (lon >= 0)) ? 2 : 3;

            int latint = (int)(Math.Abs(lat / 180) * 4096);
            int lonint = (int)(Math.Abs(lon / 180) * 4096);
            return String.Format("{0:x1}{1:x3}{2:x3}", q, latint, lonint);
        }


        /// <summary>
        /// Converts decimal latlon string to hexadecimal
        /// </summary>
        /// <param name="deccoords">csv latlon string eg-23.56,23.79 </param>
        /// <returns>qnnee format latlon string</returns>
        public static string to_qnnee(string deccoords)
        {
            char[] delim = { ',' };
            string[] sarr = deccoords.Split(delim);
            double lat = System.Convert.ToDouble(sarr[0]);
            double lon = System.Convert.ToDouble(sarr[1]);
            int q = ((lat >= 0) && (lon >= 0)) ? 0 :
                     ((lat >= 0) && (lon < 0)) ? 1 :
                     ((lat < 0) && (lon >= 0)) ? 2 : 3;

            int latint = (int)(Math.Abs(lat / 180) * 256);
            int lonint = (int)(Math.Abs(lon / 180) * 256);
            return String.Format("{0:x1}{1:x2}{2:x2}", q, latint, lonint);
        }


        /// <summary>
        /// private method used by the library
        /// latlon string point closest to meridian and equator 
        /// </summary>
        /// <param name="qnnee">5 or 7 character hex </param>
        /// <returns>latlon string  as csv</returns>
        public static string IndexPoint(string qnnee)
        {
            double lat = 0, lon = 0;

            if (qnnee.Length == 5)
            {
                byte _lat = Convert.ToByte(qnnee.Substring(1, 2), 16);
                byte _lon = Convert.ToByte(qnnee.Substring(3, 2), 16);
                lat = (double)(_lat) / 256 * 180;
                lon = (double)(_lon) / 256 * 180;
            }
            else if (qnnee.Length == 7)
            {
                Int16 _lat = Convert.ToInt16(qnnee.Substring(1, 3), 16);
                Int16 _lon = Convert.ToInt16(qnnee.Substring(4, 3), 16);
                lat = (double)(_lat) / 4096 * 180;
                lon = (double)(_lon) / 4096 * 180;
            }
            if ((qnnee[0] == '1') || (qnnee[0] == '3')) lon *= -1;
            if ((qnnee[0] == '2') || (qnnee[0] == '3')) lat *= -1;
            return String.Format("{0:F2},{1:F2}", lat, lon);
        }


        /// <summary>
        /// calculateds the centre point of a qnnee region
        /// </summary>
        /// <param name="qnnee">qnnee as string</param>
        /// <returns>decimal string latlon formar</returns>
        public static string CentrePoint(string qnnee)
        {
            double lat = 0, lon = 0, lat1 = 0, lon1 = 0;

            if (qnnee.Length == 5)
            {
                Int16 _lat = Convert.ToInt16(qnnee.Substring(1, 2), 16);
                Int16 _lon = Convert.ToInt16(qnnee.Substring(3, 2), 16);
                lat = (double)(_lat) / 256 * 180;
                lat1 = (double)(_lat + 1) / 256 * 180;
                lon = (double)(_lon) / 256 * 180;
                lon1 = (double)(_lon + 1) / 256 * 180;
            }
            else if (qnnee.Length == 7)
            {
                Int16 _lat = Convert.ToInt16(qnnee.Substring(1, 3), 16);
                Int16 _lon = Convert.ToInt16(qnnee.Substring(4, 3), 16);
                lat = (double)(_lat) / 4096 * 180;
                lat1 = (double)(_lat + 1) / 4096 * 180;
                lon = (double)(_lon) / 4096 * 180;
                lon1 = (double)(_lon + 1) / 4096 * 180;
            }
            lon = (lon + lon1) / 2;
            lat = (lat + lat1) / 2;

            if ((qnnee[0] == '1') || (qnnee[0] == '3')) lon *= -1;
            if ((qnnee[0] == '2') || (qnnee[0] == '3')) lat *= -1;
            return String.Format(@"{0:F4},{1:F4}", lat, lon); //comma separated
        }


        /// <summary>
        /// constructs a 4 point boundary from 5 or 7 digit qnnee index point and 
        /// incrementing away fron the equator and the prime meridian
        /// for bing map polygon. Handles quadrant swapping for longitude crossing 
        /// 180 degrees. Handles poles by ignoring increments beyond the second last
        /// longitude point.
        /// </summary>
        /// <param name="qnnee"></param>
        /// <returns>4 latlon csv coordinates space separates</returns>
        public static string Boundary(string qnnee)
        {
            if ((qnnee.Length != 5) && (qnnee.Length != 7)) return "";

            string[] saa = new string[4];
            Int16 _lat0, _lon0, _lat1, _lon1;
            Int16 q = Convert.ToInt16(qnnee.Substring(0, 1), 16);
            saa[0] = IndexPoint(qnnee);

            if (qnnee.Length == 5) //a region
            {
                _lat0 = (Int16)Convert.ToInt16(qnnee.Substring(1, 2), 16);
                _lon0 = (Int16)Convert.ToInt16(qnnee.Substring(3, 2), 16);

                if (_lat0 < 255)
                    _lat1 = (Int16)(_lat0 + 1);
                else
                {
                    _lat1 = _lat0;
                    q = (Int16)(3 - q);
                }
                if (_lon0 < 126)
                    _lon1 = (Int16)(_lon0 + 1);

                else _lon1 = _lon0;


                saa[0] = IndexPoint(String.Format("{0:x1}{1:x2}{2:x2}", q, _lat0, _lon0));
                saa[1] = IndexPoint(String.Format("{0:x1}{1:x2}{2:x2}", q, _lat0, _lon1));
                saa[2] = IndexPoint(String.Format("{0:x1}{1:x2}{2:x2}", q, _lat1, _lon1));
                saa[3] = IndexPoint(String.Format("{0:x1}{1:x2}{2:x2}", q, _lat1, _lon0));
            }
            else  //qnnee is 7 charaters (A District)
            {
                _lat0 = (Int16)Convert.ToInt16(qnnee.Substring(1, 3), 16);
                _lon0 = (Int16)Convert.ToInt16(qnnee.Substring(4, 3), 16);

                if (_lat0 < 4095)
                    _lat1 = (Int16)(_lat0 + 1);
                else
                {
                    _lat1 = _lat0;
                    q = (Int16)(3 - q);
                }
                if (_lon0 < 2046)
                    _lon1 = (Int16)(_lon0 + 1);
                else _lon1 = _lon0;

                saa[0] = IndexPoint(String.Format("{0:x1}{1:x3}{2:x3}", q, _lat0, _lon0));
                saa[1] = IndexPoint(String.Format("{0:x1}{1:x3}{2:x3}", q, _lat0, _lon1));
                saa[2] = IndexPoint(String.Format("{0:x1}{1:x3}{2:x3}", q, _lat1, _lon1));
                saa[3] = IndexPoint(String.Format("{0:x1}{1:x3}{2:x3}", q, _lat1, _lon0));
            }
            return String.Join(" ", saa);
        }


        /// <summary>
        /// public method to use a decimal degree point to a
        /// get the region (40 arc minutes 80 x80 km) boundary points. 
        /// </summary>
        /// <param name="decdeg">Decimal Degrees</param>
        /// <returns>A string containing four decimal degree points</returns>
        public static string LatLon_to_qnnee_boundary(string decdeg)
        {
            string qne = to_qnnee(decdeg);
            return Boundary(qne);
        }

        /// <summary>
        /// public method to use a decimal degree point to a
        /// get the region (2.6 arc minutes 5x5 km) boundary points. 
        /// </summary>
        /// <param name="decdeg">Decimal Degrees</param>
        /// <returns>A string containing four decimal degree points</returns>
        public static string LatLon_to_qnnneee_boundary(string decdeg)
        {
            string qne = to_qnnneee(decdeg);
            return Boundary(qne);
        }


        /// Moves coordinate position North South East or West. Takes care of 
        /// hemisphere - moving north in southern hemisphere requires moving towards 
        /// equator while northern hemisphere north moves towards pole.
        /// Handles 40 arcmin regions and 2.6 arc min district
        /// </summary>
        /// <param name="qnnee"></param>
        /// <param name="nsew">direction 'n','s','e','w'</param>
        /// <returns>region coordinates</returns>
        public static string MoveNSEW(string qnnee, char nsew)
        {
            // string s = "";
            if ((qnnee.Length != 5) && (qnnee.Length != 7)) return "";

            if (qnnee.Length == 5) // 5 character qnnee
            {
                byte q = (byte)qnnee[0];
                int ns = Convert.ToInt16(qnnee.Substring(1, 2), 16);
                int ew = Convert.ToInt16(qnnee.Substring(3, 2), 16);
                if (q == '1') ew *= -1;
                if (q == '2') ns *= -1;
                if (q == '3') { ns *= -1; ew *= -1; }

                bool isneg = false;
                switch (nsew)
                {
                    case 'n': if (ns < 127) ns++; break;
                    case 's': if (ns > -127) ns--; break;
                    case 'e': isneg = (ew < 0);
                        ew = ((ew += 1) % 256);
                        if (isneg == true) ew = (Math.Abs(ew)) * -1;
                        break;
                    case 'w': isneg = (ew < 0);
                        ew = ((ew -= 1) % 256);
                        if (isneg == true) ew = (Math.Abs(ew)) * -1;
                        break;
                }

                q = (byte)(((ns >= 0) & (ew >= 0)) ? 0 :  //00 ne
                             ((ns >= 0) & (ew < 0)) ? 1 :   //01 nw
                             ((ns < 0) & (ew < 0)) ? 3 : 2);   //10 se / sw
                // ((ns < 0) & (ew < 0)) ? 3 :

                if (ew == 0xff) q = (byte)(q ^ 0x01);//record the changeover details in 'q'
                if (ew == 0x00) q = (byte)(q ^ 0x01);//record the changeover details in 'q'

                qnnee = String.Format("{0:x1}{1:x2}{2:x2}", q, Math.Abs(ns), Math.Abs(ew));

                return qnnee;
            }

            else
                if (qnnee.Length == 7) // 6 character qnnee
                {
                    byte q = (byte)qnnee[0];
                    int ns = Convert.ToInt16(qnnee.Substring(1, 3), 16);
                    int ew = Convert.ToInt16(qnnee.Substring(4, 3), 16);
                    if (q == '1') ew *= -1;
                    if (q == '2') ns *= -1;
                    if (q == '3') { ns *= -1; ew *= -1; }

                    bool isneg = false;
                    switch (nsew)
                    {
                        case 'n': if (ns < 2047) ns++; break;
                        case 's': if (ns > -2047) ns--; break;
                        case 'e': isneg = (ew < 0);
                            ew = ((ew += 1) % 4096);
                            if (isneg == true) ew = (Math.Abs(ew)) * -1;
                            break;
                        case 'w': isneg = (ew < 0);
                            ew = ((ew -= 1) % 4096);
                            if (isneg == true) ew = (Math.Abs(ew)) * -1;
                            break;
                    }

                    q = (byte)(((ns >= 0) & (ew >= 0)) ? 0 :  //00 ne
                                 ((ns >= 0) & (ew < 0)) ? 1 :   //01 nw
                                 ((ns < 0) & (ew < 0)) ? 3 : 2);   //10 se / sw
                    // ((ns < 0) & (ew < 0)) ? 3 :

                    if (ew == 0xfff) q = (byte)(q ^ 0x01);//record the changeover details in 'q'
                    if (ew == 0x000) q = (byte)(q ^ 0x01);//record the changeover details in 'q'

                    qnnee = String.Format("{0:x1}{1:x3}{2:x3}", q, Math.Abs(ns), Math.Abs(ew));

                    return qnnee;
                }
            return "";
        }

        //--------------------------------------------------------------------------




    }

}
