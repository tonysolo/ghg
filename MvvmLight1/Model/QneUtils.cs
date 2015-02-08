using System;
using System.Text;

namespace MvvmLight1.Model
{
    
    public static class QneUtils
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
            var lat = Convert.ToDouble(sarr[0]);
            var lon = Convert.ToDouble(sarr[1]);
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
            var q = lat >= 0 && lon >= 0 ? 0 :
                     lat >= 0 && lon < 0 ? 1 :
                     lat < 0 && lon >= 0 ? 2 : 3;

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
            var east = (qnnee[0] == '0');

            switch (qnnee.Length)
            {
                case 5:
                    var lat5 = Convert.ToInt16(qnnee.Substring(1, 2), 16);
                    var lon5 = Convert.ToInt16(qnnee.Substring(3, 2), 16);
                    lat = (double) lat5/256*180;
                    lon = (double) lon5/256*180;
                    break;
                case 7:
                    var lat7 = Convert.ToInt16(qnnee.Substring(1, 3), 16);
                    var lon7 = Convert.ToInt16(qnnee.Substring(4, 3), 16);
                    lat = (double) lat7/4096*180;
                    lon = (double) lon7/4096*180;
                    break;
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

            switch (qnnee.Length)
            {
                case 5:
                {
                    var lat5 = Convert.ToInt16(qnnee.Substring(1, 2), 16);
                    var lon5 = Convert.ToInt16(qnnee.Substring(3, 2), 16);
                    lat = (double)lat5 / 256 * 180;
                    lat1 = (double)(lat5 + 1) / 256 * 180;
                    lon = (double)lon5 / 256 * 180;
                    lon1 = (double)(lon5 + 1) / 256 * 180;
                }
                    break;
                case 7:
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

                saa[0] = IndexPoint(String.Format("{0:x1}{1:x2}{2:x2}", q, lat0, lon0));
                saa[1] = IndexPoint(String.Format("{0:x1}{1:x2}{2:x2}", q, lat0, lon1));
                saa[2] = IndexPoint(String.Format("{0:x1}{1:x2}{2:x2}", q, lat1, lon1));
                saa[3] = IndexPoint(String.Format("{0:x1}{1:x2}{2:x2}", q, lat1, lon0));
            }
            else  //qnnee is 7 charaters (A District)
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

        public static string MoveN(string qnnee)
        {          
          var ns = Convert.ToInt16(qnnee.Substring(1, 2), 16);
          var ew = Convert.ToInt16(qnnee.Substring(3, 2), 16);
          var q = Convert.ToByte(qnnee.Substring(0, 1), 16);
          var north = (q & 0x02)==0;
            if (north)
            {
                if (ns < 127) 
                ns ++;
            }
            else //if south will move north until zero south
                 //then step to zero north at the equator, quadrant change,
                 //to draw different boundaries
            {
                ns --;
                if (ns >= 0) return String.Format("{0:x1}{1:x2}{2:x2}", q, ns, ew);
                q = (byte) (q & 0x01);//change quadrant to north 
                ns = 0;
            }
            return String.Format("{0:x1}{1:x2}{2:x2}", q, ns,ew);
        }

        public static string MoveS(string qnnee)
        {
            
            var ns = Convert.ToInt32(qnnee.Substring(1, 2),fromBase:16);
            var ew = Convert.ToInt32(qnnee.Substring(3, 2),fromBase:16);
            var q = Convert.ToInt32(qnnee.Substring(0, 1),fromBase:16);

            var south = (q & 0x02) == 2;
            if (south)
            {
                if (ns < 127)
                    ns++;
            }
            else //if north will move south until zero north
            //then step to zero south at the equator, quadrant change,
            //to draw different boundaries
            {
                ns--;
                if (ns >= 0) return String.Format("{0:x1}{1:x2}{2:x2}", q, ns, ew);
                q = (byte)(q | 0x02);//change quadrant to south 
                ns = 0;
            }
            return String.Format("{0:x1}{1:x2}{2:x2}", q, ns, ew);
        }


        public static string MoveE(string qnnee)
        {
            var ns = Convert.ToInt16(qnnee.Substring(1, 2), 16);
            var ew = Convert.ToInt16(qnnee.Substring(3, 2), 16);
            var q = Convert.ToByte(qnnee.Substring(0, 1), 16);
            var east = (q & 0x01) == 0;
            if (east)
            {
                if (ew < 255) ew++;
                else q = (byte) (q | 0x01);//set west
            }
            else //first change quadrant to west
            //then step to zero west to east,
            //then repeat change to east quadrant go east to west
            {
                if (ew>0) ew--;
                else q = (byte) (q & 0x02);//set east
               // if (ns >= 0) return String.Format("{0:x1}{1:x2}{2:x2}", q, ns, ew);
               // q = (byte)(q | 0x02);//change quadrant to south 
               // ns = 0;
            }
            return String.Format("{0:x1}{1:x2}{2:x2}", q, ns, ew);
        }

        public static string MoveW(string qnnee)
        {
            var ns = Convert.ToInt16(qnnee.Substring(1, 2), 16);
            var ew = Convert.ToInt16(qnnee.Substring(3, 2), 16);
            var q = Convert.ToByte(qnnee.Substring(0, 1), 16);
            var west = (q & 0x01) == 1;
            if (west)
            {
                if (ew < 255) ew++;
                else q = (byte)(q & 0x10);//set east
            }
            else //first change quadrant to east
            //then step to zero east to west,
            //then repeat change to west quadrant go west to east
            {
                if (ew > 0) ew--;
                else q = (byte)(q | 0x01);//set west
                // if (ns >= 0) return String.Format("{0:x1}{1:x2}{2:x2}", q, ns, ew);
                // q = (byte)(q | 0x02);//change quadrant to south 
                // ns = 0;
            }
            return String.Format("{0:x1}{1:x2}{2:x2}", q, ns, ew);
        } 

        ///<Summary>
        /// Moves coordinate position North South East or West. Takes care of 
        /// hemisphere - moving north in southern hemisphere requires moving towards 
        /// equator while northern hemisphere north moves towards pole.
        /// Handles 40 arcmin regions and 2.6 arc min district
        /// </summary>
        /// <param name="qnnee"></param>
        /// <param name="nsew">direction 'n','s','e','w'</param>
        /// <returns>region coordinates</returns>
        public static string MoveNsew(string qnnee, char nsew)
        {
            bool  isWest;
            var isEast = isWest = false;
            if ((qnnee.Length != 5) && (qnnee.Length != 7)) return "";

            if (qnnee.Length == 5) // 5 character qnnee
            {
                var q = (byte)qnnee[0];
                int ns = Convert.ToInt16(qnnee.Substring(1, 2), 16);
                int ew = Convert.ToInt16(qnnee.Substring(3, 2), 16);
                if (q == '1') ew *= -1;
                if (q == '2') ns *= -1;
                if (q == '3') { ns *= -1; ew *= -1; }

               //ew = 512 + ew;
               //BUG moving accross the prime meridian skips a cell
               // bool isneg;
                switch (nsew)
                {
                    case 'n': if (ns < 127) ns++; break;
                    case 's': if (ns > -127) ns--; break;
                    case 'e':                    
                        ew++;
                        isWest = (ew == 0);
                        break;
                    case 'w': //isneg = (ew < 0);
                        ew--;
                        isEast = (ew == 0);
                        break;
                        
                }

                q = (byte)(ns >= 0 & (ew > 0) |((ew==0)&(isEast))  ? 0 :  //00 ne
                           ns >= 0 & (ew < 0) |((ew==0)&(isWest)) ?  1 :   //01 nw
                           ns <  0 & (ew < 0) |((ew==0)&(isWest)) ? 3 : 2) ;  //03 sw or SE
                          // ns > 0  & (ew >0 ) |((ew==0)&(isWest))  2); //10 se / sw
                          

                            

               // if (ew == 0xff) q = (byte)(q ^ 0x01);//record the changeover details in 'q'
               // if (ew == 0x00) q = (byte)(q ^ 0x01);//record the changeover details in 'q'

                qnnee = String.Format("{0:x1}{1:x2}{2:x2}", q, Math.Abs(ns), Math.Abs(ew));

                return qnnee;
            }

            if (qnnee.Length == 7) // 6 character qnnee NEEDS SIMPLIFYING
            {
                var q = (byte)qnnee[0];
                int ns = Convert.ToInt16(qnnee.Substring(1, 3), 16);
                int ew = Convert.ToInt16(qnnee.Substring(4, 3), 16);
                if (q == '1') ew *= -1;
                if (q == '2') ns *= -1;
                if (q == '3') { ns *= -1; ew *= -1; }

                bool isneg;
                switch (nsew)
                {
                    case 'n': if (ns < 2047) ns++; break;
                    case 's': if (ns > -2047) ns--; break;
                    case 'e': isneg = (ew < 0);
                        ew = ((ew + 1) % 4096);
                        if (isneg) ew = (Math.Abs(ew)) * -1;
                        break;
                    case 'w': isneg = (ew < 0);
                        ew = ((ew - 1) % 4096);
                        if (isneg) ew = (Math.Abs(ew)) * -1;
                        break;
                }

                q = (byte)(ns >= 0 & ew >= 0 ? 0 :  //00 ne
                    ns >= 0 & ew < 0 ? 1 :   //01 nw
                        ns < 0 & ew < 0 ? 3 : 2);   //10 se / sw
                // ((ns < 0) & (ew < 0)) ? 3 :

                if (ew == 0xfff) q = (byte)(q ^ 0x01);//record the changeover details in 'q'
                if (ew == 0x000) q = (byte)(q ^ 0x01);//record the changeover details in 'q'

                qnnee = String.Format("{0:x1}{1:x3}{2:x3}", q, Math.Abs(ns), Math.Abs(ew));

                return qnnee;
            }
            return "";
        }

        //--------------------------------------------------------------------------
        /// <summary>
        /// Calculates Seconds to midnight for timezones
        /// </summary>
        /// <param name="qe">2 hexadecimal charaster string quadrant and longitide</param>
        /// <returns>secs</returns>
        static int SecsToMidnight(string qe)
        {
            var q = qe[0];
            var e = Convert.ToByte(qe.Substring(1, 1), 16) * 45 * 60;
            var dt = DateTime.UtcNow;
            var secs = dt.Hour * 60 * 60 + dt.Minute * 60 + dt.Second;
            secs += (q == '1') || (q == '3') ? -e : e;   //west                    
            return secs % 0x15180;
        }



    }

}
