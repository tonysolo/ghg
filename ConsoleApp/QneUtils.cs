using System;
using System.Globalization;

namespace ConsoleApp
{
    static class QneUtils
    {
        
        /// <summary>
        /// Converts decimal degrees string to district level (7) qnnneee
        /// </summary>
        /// <param name="deccoords">string</param>
        /// <returns>string qnnneee</returns>
        public static string to_qnnneee(string deccoords) //input -23.56,23.79
        {

            char[] delim = { ',' };
            var sarr = deccoords.Split(delim);
            var lat = System.Convert.ToDouble(sarr[0]);
            var lon = System.Convert.ToDouble(sarr[1]);

            var q = ((lat >= 0) && (lon >= 0)) ? 0 :
                     ((lat >= 0) && (lon < 0)) ? 1 :
                     ((lat < 0) && (lon >= 0)) ? 2 : 3;

            var latint = (int)(Math.Abs(lat / 180) * 4096);
            var lonint = (int)(Math.Abs(lon / 180) * 4096);
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
            var sarr = deccoords.Split(delim);
            var lat = Convert.ToDouble(sarr[0]);
            var lon = Convert.ToDouble(sarr[1]);
            var q = ((lat >= 0) && (lon >= 0)) ? 0 :
                     ((lat >= 0) && (lon < 0)) ? 1 :
                     ((lat < 0) && (lon >= 0)) ? 2 : 3;

            var latint = (int)(Math.Abs(lat / 180) * 256);
            var lonint = (int)(Math.Abs(lon / 180) * 256);
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
                var llat = Convert.ToByte(qnnee.Substring(1, 2), 16);
                var llon = Convert.ToByte(qnnee.Substring(3, 2), 16);
                lat = (double)(llat) / 256 * 180;
                lon = (double)(llon) / 256 * 180;
            }
            else if (qnnee.Length == 7)
            {
                var llat = Convert.ToInt16(qnnee.Substring(1, 3),16);
                var llon = Convert.ToInt16(qnnee.Substring(4, 3),16);
                lat = (double)(llat) / 4096 * 180;
                lon = (double)(llon) / 4096 * 180;
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
                            var llat = Convert.ToInt16(qnnee.Substring(1, 2), 16);
                            var llon = Convert.ToInt16(qnnee.Substring(3, 2), 16);
                            lat = llat / 256 * 180;
                            lat1 = (double)(llat + 1) / 256 * 180;
                            lon  = (double)(llon) / 256 * 180;
                            lon1 = (double)(llon + 1) / 256 * 180;
                        }
                        else if (qnnee.Length == 7)
                        {
                            var llat = Convert.ToInt16(qnnee.Substring(1, 3), 16);
                            var llon = Convert.ToInt16(qnnee.Substring(4, 3), 16);
                            lat  = (double)(llat) / 4096 * 180;
                            lat1 = (double)(llat + 1) / 4096 * 180;
                            lon  = (double)(llon) / 4096 * 180;
                            lon1 = (double)(llon + 1) / 4096 * 180;
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
                    /// 180 degrees. Handle poles by ignoring increments beyond the second last
                    /// longitude point.
                    /// </summary>
                    /// <param name="qnnee"></param>
                    /// <returns>4 latlon csv coordinates space separates</returns>
                    public static string Boundary(string qnnee)
                    {
                        if ((qnnee.Length != 5) && (qnnee.Length != 7)) return "";

                        var saa = new string[4];
                        Int16 lat0, lon0, lat1, lon1;
                        var q = Convert.ToInt16(qnnee.Substring(0, 1), 16);
                        saa[0] = IndexPoint(qnnee);

                        if (qnnee.Length == 5) //a region
                        {
                            lat0 = (Int16)Convert.ToInt16(qnnee.Substring(1, 2), 16);
                            lon0 = (Int16)Convert.ToInt16(qnnee.Substring(3, 2), 16);

                            if (lat0 < 255)
                                lat1 = (Int16)(lat0 + 1);
                            else
                            {
                                lat1 = lat0;
                                q = (Int16)(3 - q);
                            }
                            if (lon0 < 126)
                                lon1 = (Int16)(lon0 + 1);

                            else lon1 = lon0;


                            saa[0] = IndexPoint(String.Format("{0:x1}{1:x2}{2:x2}", q, lat0, lon0));
                            saa[1] = IndexPoint(String.Format("{0:x1}{1:x2}{2:x2}", q, lat0, lon1));
                            saa[2] = IndexPoint(String.Format("{0:x1}{1:x2}{2:x2}", q, lat1, lon1));
                            saa[3] = IndexPoint(String.Format("{0:x1}{1:x2}{2:x2}", q, lat1, lon0));
                        }
                        else  //qnnee is 7 charaters (A District)
                        {
                            lat0 = (Int16)Convert.ToInt16(qnnee.Substring(1, 3), 16);
                            lon0 = (Int16)Convert.ToInt16(qnnee.Substring(4, 3), 16);

                            if (lat0 < 4095)
                                lat1 = (Int16)(lat0 + 1);
                            else
                            {
                                lat1 = lat0;
                                q = (Int16)(3 - q);
                            }
                            if (lon0 < 2046) 
                                lon1 = (Int16)(lon0 + 1); 

                            else lon1 = lon0;

                            saa[0] = IndexPoint(String.Format("{0:x1}{1:x3}{2:x3}", q, lat0, lon0));
                            saa[1] = IndexPoint(String.Format("{0:x1}{1:x3}{2:x3}", q, lat0, lon1));
                            saa[2] = IndexPoint(String.Format("{0:x1}{1:x3}{2:x3}", q, lat1, lon1));
                            saa[3] = IndexPoint(String.Format("{0:x1}{1:x3}{2:x3}", q, lat1, lon0));
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
                        var qne = to_qnnee(decdeg);
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
                        var qne = to_qnnneee(decdeg);
                        return Boundary(qne);
                    }


        public static sbyte Timezone(string qnnee) //or qnnneee
        {
            if ((qnnee.Length != 5) && (qnnee.Length != 7)) return sbyte.MaxValue;//error
            var q = (qnnee[0] - '0');
            q = (q & 0x01)<<4 ;//set quadrant if west add 16
            var e = (qnnee.Length == 5) ? qnnee[3]:qnnee[4];
            var f = Convert.ToByte(e.ToString(CultureInfo.InvariantCulture), 16);
            return (sbyte)(q|f);         
        }   // 0 to  +15 is east of meridian. 0 to -15 is west of meridian

    }
    }
