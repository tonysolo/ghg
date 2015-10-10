using System;
using System.Globalization;



namespace MvvmLight1.Model
{
    public static class QneUtils
    {

        public static CultureInfo Ci = new CultureInfo("en-us");

        public static string Setzoom4()
        {
            return "4";
        }

        public static string Setzoom6()
        {
            return "6";
        }

        /// <summary>
        ///     Converts decimal degrees string to district level (7) qnnneee
        /// </summary>
        /// <param name="deccoords">string</param>
        /// <returns>string qnnneee</returns>
        public static string to_qnnneee(string deccoords) //input -23.56,23.79
        {
            char[] delim = { ',' };
            var sarr = deccoords.Split(delim);
            var lat = Convert.ToDouble(sarr[0]);
            var lon = Convert.ToDouble(sarr[1]);
            var q = ((lat >= 0) && (lon >= 0))
                ? 0
                : ((lat >= 0) && (lon < 0))
                    ? 1
                    : ((lat < 0) && (lon >= 0)) ? 2 : 3;

            var latint = (int)(Math.Abs(lat / 180) * 4096);
            var lonint = (int)(Math.Abs(lon / 180) * 4096);
            var s = String.Format("Ci,{0:x1}{1:x3}{2:x3}", q, latint, lonint);
            return s.Trim();
        }


        /// <summary>
        ///     Converts decimal latlon string to hexadecimal
        /// </summary>
        /// <param name="deccoords">csv latlon string eg-23.56,23.79 </param>
        /// <returns>qnnee format latlon string</returns>
        public static string to_qnnee(string deccoords)
        {
            char[] delim = { ',' };
            var sarr = deccoords.Split(delim);
            var lat = Convert.ToDouble(sarr[0]);
            var lon = Convert.ToDouble(sarr[1]);
            var q = lat >= 0 && lon >= 0
                ? 0
                : lat >= 0 && lon < 0
                    ? 1
                    : lat < 0 && lon >= 0 ? 2 : 3;

            var latint = (int)(Math.Abs(lat / 180) * 256);
            var lonint = (int)(Math.Abs(lon / 180) * 256);
            var s = String.Format(Ci, "{0:x1}{1:x2}{2:x2}", q, latint, lonint);
            return s.Trim();
        }

        //public static byte[] toByteArray(string qnnee)
        //{
        ////    byte[] ba = new byte[3];
        ////    ba[1] = Convert.ToByte(qnnee.Substring(1, 2),16);
        // //   ba[2] = Convert.ToByte(qnnee.Substring(3, 2), 16);
        //    if (ba[0] == 1) ee *= -1;
        //    else if (ba[0] == 2) nn *= -1;
        //    else if (ba[0]) == 3){ ee *= -1;nn *= -1; }
        //    return 

        //}

        /// <summary>
        ///     private method used by the library
        ///     latlon string point closest to meridian and equator
        /// </summary>
        /// <param name="qnnee">5 or 7 character hex </param>
        /// <returns>latlon string  as csv</returns>
        public static string IndexPoint(string qnnee)
        {
            double lat = 0, lon = 0;
            //    var east = (qnnee[0] == '0');

            switch (qnnee.Length)
            {
                case 5:
                case 6:
                    var lat5 = Convert.ToInt16(qnnee.Substring(1, 2), 16);
                    var lon5 = Convert.ToInt16(qnnee.Substring(3, 2), 16);
                    lat = (double)lat5 / 256 * 180;
                    lon = (double)lon5 / 256 * 180;
                    break;
                case 7:
                case 8:
                    var lat7 = Convert.ToInt16(qnnee.Substring(1, 3), 16);
                    var lon7 = Convert.ToInt16(qnnee.Substring(4, 3), 16);
                    lat = (double)lat7 / 4096 * 180;
                    lon = (double)lon7 / 4096 * 180;
                    break;
            }
            if ((qnnee[0] == '1') || (qnnee[0] == '3')) lon *= -1;
            if ((qnnee[0] == '2') || (qnnee[0] == '3')) lat *= -1;
            // var s = String.Format("{0} {1:en-US}", lat, lon);
            return lat.ToString("F2", Ci) + ',' + lon.ToString("F2", Ci);
        }


        /// <summary>
        ///     calculateds the centre point of a qnnee region
        /// </summary>
        /// <param name="qnnee">qnnee as string</param>
        /// <returns>decimal string latlon formar</returns>
        public static string CentrePoint(string qnnee)
        {
            double lat = 0, lon = 0, lat1 = 0, lon1 = 0;
            qnnee = qnnee.Trim();
            switch (qnnee.Length)
            {
                case 5:
                case 6:
                    {
                        for (int i = 0; i < qnnee.Length; i++)
                        {
                            var x = qnnee[i];
                            x = qnnee[i];
                        }

                        var lat5 = Convert.ToInt16(qnnee.Substring(1, 2), 16);
                        var lon5 = Convert.ToInt16(qnnee.Substring(3, 2), 16);
                        lat = (double)lat5 / 256 * 180;
                        lat1 = (double)(lat5 + 1) / 256 * 180;
                        lon = (double)lon5 / 256 * 180;
                        lon1 = (double)(lon5 + 1) / 256 * 180;
                    }
                    break;
                case 7:
                case 8:
                    {
                        var lat7 = Convert.ToInt16(qnnee.Substring(1, 3), 16);
                        var lon7 = Convert.ToInt16(qnnee.Substring(4, 3), 16);
                        lat = (double)lat7 / 4096 * 180;
                        lat1 = (double)(lat7 + 1) / 4096 * 180;
                        lon = (double)lon7 / 4096 * 180;
                        lon1 = (double)(lon7 + 1) / 4096 * 180;
                    }
                    break;
            }
            lon = (lon + lon1) / 2;
            lat = (lat + lat1) / 2;

            if ((qnnee[0] == '1') || (qnnee[0] == '3')) lon *= -1;
            if ((qnnee[0] == '2') || (qnnee[0] == '3')) lat *= -1;
            return String.Format(Ci, "{0:F4},{1:F4}", lat, lon); //comma separated
        }


        /// <summary>
        ///     constructs a 4 point boundary from 5 or 7 digit qnnee index point and
        ///     incrementing away fron the equator and the prime meridian
        ///     for bing map polygon. Handles quadrant swapping for longitude crossing
        ///     180 degrees. Handles poles by ignoring increments beyond the second last
        ///     longitude point.
        /// </summary>
        /// <param name="qnnee"></param>
        /// <returns>4 latlon csv coordinates space separates</returns>
        public static string Boundary(string qnnee)
        {
            if ((qnnee.Length != 5) && (qnnee.Length != 7)) return "";

            var saa = new string[4];
            Int16 lat1;
            Int16 lon1;
            var q = Convert.ToInt16(qnnee.Substring(0, 1), 16);
            saa[0] = IndexPoint(qnnee);

            if (qnnee.Length == 5) //a region
            {
                var lat0 = Convert.ToInt16(qnnee.Substring(1, 2), 16);
                var lon0 = Convert.ToInt16(qnnee.Substring(3, 2), 16);

                if (lat0 < 127)
                    lat1 = (Int16)(lat0 + 1);
                else
                {
                    lat1 = lat0;
                    q = (Int16)(3 - q);
                }
                if (lon0 < 255)
                    lon1 = (Int16)(lon0 + 1);

                else lon1 = lon0;

                //if (lon0 == 0) lon0 = 1;
                // if (lon1 == 0) lon1 = 1;

                saa[0] = IndexPoint(String.Format(Ci, "{0:x1}{1:x2}{2:x2}", q, lat0, lon0));
                saa[1] = IndexPoint(String.Format(Ci, "{0:x1}{1:x2}{2:x2}", q, lat0, lon1));
                saa[2] = IndexPoint(String.Format(Ci, "{0:x1}{1:x2}{2:x2}", q, lat1, lon1));
                saa[3] = IndexPoint(String.Format(Ci, "{0:x1}{1:x2}{2:x2}", q, lat1, lon0));
            }
            else //qnnee is 7 charaters (A District)
            {
                var lat0 = Convert.ToInt16(qnnee.Substring(1, 3), 16);
                var lon0 = Convert.ToInt16(qnnee.Substring(4, 3), 16);

                if (lat0 < 2047)
                    lat1 = (Int16)(lat0 + 1);
                else
                {
                    lat1 = lat0;
                    q = (Int16)(3 - q);
                }
                if (lon0 < 4095)
                    lon1 = (Int16)(lon0 + 1);
                else lon1 = lon0;

                if (lon0 == 0) lon0 = 1;
                if (lon1 == 0) lon1 = 1;

                saa[0] = IndexPoint(String.Format(Ci, "{0:x1}{1:0.0.00}{2:0.00}", q, lat0, lon0));
                saa[1] = IndexPoint(String.Format(Ci, "{0:x1}{1:0.0}{2:0.00}", q, lat0, lon1));
                saa[2] = IndexPoint(String.Format(Ci, "{0:x1}{1:0.00}{2:0.00}", q, lat1, lon1));
                saa[3] = IndexPoint(String.Format(Ci, "{0:x1}{1:0.00,}{2:0.00}", q, lat1, lon0));
            }
            return String.Join(" ", saa);
        }


        /// <summary>
        ///     public method to use a decimal degree point to a
        ///     get the region (40 arc minutes 80 x80 km) boundary points.
        /// </summary>
        /// <param name="decdeg">Decimal Degrees</param>
        /// <returns>A string containing four decimal degree points</returns>
        public static string LatLon_to_qnnee_boundary(string decdeg)
        {
            var qne = to_qnnee(decdeg);
            return Boundary(qne);
        }

        /// <summary>
        ///     public method to use a decimal degree point to a
        ///     get the region (2.6 arc minutes 5x5 km) boundary points.
        /// </summary>
        /// <param name="decdeg">Decimal Degrees</param>
        /// <returns>A string containing four decimal degree points</returns>
        public static string LatLon_to_qnnneee_boundary(string decdeg)
        {
            var qne = to_qnnneee(decdeg);
            return Boundary(qne);
        }



        /// <Summary>
        ///     Moves coordinate position North South East or West. Takes care of
        ///     hemisphere - moving north in southern hemisphere requires moving towards
        ///     equator while northern hemisphere north moves towards pole.
        ///     Handles 40 arcmin regions and 2.6 arc min district
        /// </summary>
        /// <param name="qnnee"></param>
        /// <param name="nsew">direction 'n','s','e','w'</param>
        /// <returns>region coordinates</returns>
        public static string MoveNsew(string qnnee, char nsew)
        {
            qnnee _qne = new Model.qnnee(qnnee);

            bool isWest;
            var isEast = isWest = false;
            if ((qnnee.Length != 5) && (qnnee.Length != 7)) return "";


            if (qnnee.Length == 5) // 5 character qnnee
            {
                var q = qnnee[0];
                var ns = Convert.ToInt16(qnnee.Substring(1, 2), 16);
                var ew = Convert.ToInt16(qnnee.Substring(3, 2), 16);
                if (q == '1') ew *= -1;
                if (q == '2') ns *= -1;
                if (q == '3')
                {
                    ns *= -1;
                    ew *= -1;
                }

                //ew = 512 + ew;
                //BUG moving accross the prime meridian skips a cell - fixed
                // bool isneg;
                switch (nsew)
                {
                    case 'n':
                        if (ns < 127) ns++;
                        break;
                    case 's':
                        if (ns > -127) ns--;
                        break;
                    case 'e':
                        ew++;
                        //isWest = (ew == 0);
                        break;
                    case 'w': //isneg = (ew < 0);
                        ew--;
                        //isEast = (ew == 0);
                        break;
                }

                q = (ns >= 0 & (ew > 0) | ((ew == 0) & (isEast))
                    ? '0'
                    : ns >= 0 & (ew < 0) | ((ew == 0) & (isWest))
                        ? '1'
                        : ns < 0 & (ew < 0) | ((ew == 0) & (isWest)) ? '3' : '2'); //03 sw or SE
                // ns > 0  & (ew >0 ) |((ew==0)&(isWest))  2); //10 se / sw
                // if (ew == 0xff) q = (byte)(q ^ 0x01);//record the changeover details in 'q'
                // if (ew == 0x00) q = (byte)(q ^ 0x01);//record the changeover details in 'q'

                qnnee = String.Format(Ci, "{0:x1}{1:x2}{2:x2}", q, Math.Abs(ns), Math.Abs(ew));

                return qnnee;
            }
            return "";
        }


        /// <summary>
        ///     Calculates Seconds to midnight for timezones
        /// </summary>
        /// <param name="qe">2 hexadecimal charaster string quadrant and longitide</param>
        /// <returns>secs</returns>
        private static
        int SecsToMidnight(string qe)
        {
            var q = qe[0];
            var e = Convert.ToByte(qe.Substring(1, 1), 16) * 45 * 60;
            var dt = DateTime.UtcNow;
            var secs = dt.Hour * 60 * 60 + dt.Minute * 60 + dt.Second;
            secs += (q == '1') || (q == '3') ? -e : e; //west                    
            return secs % 0x15180;
        }

    }
}




