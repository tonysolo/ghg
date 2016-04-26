using System;
using System.Globalization;

namespace MvvmLight1.Model
{

    public static class QneNav
        
    {

        public static string AzureTrim(string instr) //trims hidden chars
        {
            var outst = "";
            for (var i = 0; i < instr.Length; i++)
            {
                if (char.IsLetterOrDigit(instr[i])) outst += instr[i];
            }
            return outst;
        }

        static bool Isnorth(byte[] x) { return (x[0] & 0x02) == 0; }
        static bool Issouth(byte[] x) { return (x[0] & 0x02) == 2; }
        static bool Iseast(byte[] x) { return (x[0] & 0x01) == 0; }
        static bool Iswest(byte[] x) { return (x[0] & 0x01) == 1; }


        public static void Seteast(ref byte[] x) { x[0] &= 0xfe;
        } //clear bit 0
        public static void Setwest(ref byte[] x) { x[0] |= 0x01;
        } //set bit 0
        public static void Setsouth(ref byte[] x) { x[0] |= 0x02;
        }   // set bit 1
        public static void Setnorth(ref byte[] x) {x[0] &= 0xfd;
        }    //clear bit 1

        public static void MovN(ref byte[] x)
        {
            if (Isnorth(x) && (x[1] < 127)) { x[1]++; return; }
            if (Issouth(x) && (x[1] > 0)) x[1]--; else Setnorth(ref x);
        }


        public static void MovS(ref byte[] x)
        {
            if (Issouth(x) && (x[1] < 127)) { x[1]++; return; }
            if (Isnorth(x) && (x[1] > 0)) x[1]--; else Setsouth(ref x);
        }


        public static void MovE(ref byte[] x)
        {
            if (Iswest(x) && (x[2] == 0)) { Seteast(ref x); return; }
            if (Iseast(x) && (x[2] == 255)) { Setwest(ref x); return; }
            if (Iseast(x)) { x[2]++; return; }
            if (!Iswest(x)) return;
            x[2]--;
        }


        public static void MovW(ref byte[] x)
        {
            if (Iseast(x) && (x[2] == 0)) { Setwest(ref x); return; }
            if (Iswest(x) && (x[2] == 255)) { Seteast(ref x); return; }
            if (Iseast(x)) { x[2]--; return; }
            if (!Iswest(x)) return;
            x[2]++;
        }

        //converts a qnnee string to byte[3]   
        private static byte[] Getqnebytes(string st)
        {
            var s = AzureTrim(st);
            var ba = new byte[3];
            ba[0] = Convert.ToByte(s.Substring(0, 1), 16);
            ba[1] = Convert.ToByte(s.Substring(1, 2), 16);
            ba[2] = Convert.ToByte(s.Substring(3, 2), 16);
            return ba;
        }

        //used to get qnnee strings from bytearrays
        //converts byte[3] to 5 character string
        private static string Gethexstring(byte[] barr)
        {
            return $"{barr[0]:x1}{barr[1]:x2}{barr[2]:x2}";
        }

        public static void MovN(ref string x)
        {
            var b = Getqnebytes(x);
            MovN(ref b);
            x = Gethexstring(b);
        }

        public static void MovS(ref string x)
        {
            var b = Getqnebytes(x);
            MovS(ref b);
            x = Gethexstring(b);
        }

        public static void MovE(ref string x)
        {
            var b = Getqnebytes(x);
            MovE(ref b);
            x = Gethexstring(b);
        }

        public static void MovW(ref string x)
        {
            var b = Getqnebytes(x);
            MovW(ref b);
            x = Gethexstring(b);
        }

    }


    public static class QneUtils
    {

        //public static CultureInfo Ci = new CultureInfo("en-us");

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
            var s = $"{q:x1}{latint:x3}{lonint:x3}";
            return s.Trim();
        }


        /// <summary>
        ///     Converts decimal latlon string to hexadecimal
        /// </summary>
        /// <param name="deccoords">csv latlon string eg-23.56,23.79 </param>
        /// <returns>qnnee format latlon string</returns>
        public static string to_qnnee(string deccoords)
        {
            //CultureInfo ci = CultureInfo.InvariantCulture;
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
            var s = $"{q:x1}{latint:x2}{lonint:x2}";
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
            return FloatToString(lat) + ',' + FloatToString(lon);
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

            //doubleNumber = 18934.1879;
            //var lonst = lon.ToString("F3", CultureInfo.InvariantCulture);
            //var latst = lat.ToString("F3", CultureInfo.InvariantCulture);


            //var lons = string.Format("{F:3,F:3}", lon, lat); 


            //return lonst + ',' + latst;
            return FloatToString(lat) + ',' + FloatToString(lon);
        }


        public static string FloatToString(double f)
        {
            return f.ToString("F3", CultureInfo.InvariantCulture);
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
            short lat1;
            short lon1;
            var q = Convert.ToInt16(qnnee.Substring(0, 1), 16);
            saa[0] = IndexPoint(qnnee);

            if (qnnee.Length == 5) //a region
            {
                var lat0 = Convert.ToInt16(qnnee.Substring(1, 2), 16);
                var lon0 = Convert.ToInt16(qnnee.Substring(3, 2), 16);

                if (lat0 < 127)
                    lat1 = (short)(lat0 + 1);
                else
                {
                    lat1 = lat0;
                    q = (short)(3 - q);  //? unnecessary
                }
                if (lon0 < 255)
                    lon1 = (short)(lon0 + 1);

                else lon1 = lon0;

                //if (lon0 == 0) lon0 = 1;
                // if (lon1 == 0) lon1 = 1;

                saa[0] = IndexPoint($"{q:x1}{lat0:x2}{lon0:x2}");
                saa[1] = IndexPoint($"{q:x1}{lat0:x2}{lon1:x2}");
                saa[2] = IndexPoint($"{q:x1}{lat1:x2}{lon1:x2}");
                saa[3] = IndexPoint($"{q:x1}{lat1:x2}{lon0:x2}");
            }
            else //qnnee is 7 charaters (A District)
            {
                var lat0 = Convert.ToInt16(qnnee.Substring(1, 3), 16);
                var lon0 = Convert.ToInt16(qnnee.Substring(4, 3), 16);

                if (lat0 < 2047)
                    lat1 = (short)(lat0 + 1);
                else
                {
                    lat1 = lat0;
                    q = (short)(3 - q);
                }
                if (lon0 < 4095)
                    lon1 = (short)(lon0 + 1);
                else lon1 = lon0;

                //if (lon0 == 0) lon0 = 1;
                //if (lon1 == 0) lon1 = 1;

                saa[0] = IndexPoint($"{q:x1}{lat0:x:3}{lon0:x:3}");
                saa[1] = IndexPoint($"{q:x1}{lat0:x:3}{lon1:x:3}");
                saa[2] = IndexPoint($"{q:x1}{lat1:x:3}{lon1:x:3}");
                saa[3] = IndexPoint($"{q:x1}{lat1:x:3}{lon0:x:3}");
            }
            var s = string.Join(" ", saa);

            return s;
        }

        //private static string IndexPoint(string v, short q, short lat0, short lon1)
        //{
        //     throw new NotImplementedException();
        // }


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
        ///     Handles 40 arcmin Regions and 2.6 arc min district
        /// </summary>
        /// <param name="qnnee"></param>
        /// <param name="nsew">direction 'n','s','e','w'</param>
        /// <returns>region coordinates</returns>
        public static string MoveNsew(string qnnee, char nsew)
        {
            //qnnee _qne = new Model.qnnee(qnnee);

            // bool isWest;
            // var isEast = isWest = false;
            //  if ((qnnee.Length != 5) && (qnnee.Length != 7)) return "";


            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (nsew)
            {
                case 'n': QneNav.MovN(ref qnnee); break;

                case 's': QneNav.MovS(ref qnnee); break;

                case 'e': QneNav.MovE(ref qnnee); break;

                case 'w': QneNav.MovW(ref qnnee); break;

            }
            return qnnee;
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


