﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MvvmLight1.Model
{
    static class MapNav
    {
      
        //public static string qnnee { get; set; }

        public static string MoveN(string qnnee)
        {
            var ns = Convert.ToInt16(qnnee.Substring(1, 2), 16);
            var ew = Convert.ToInt16(qnnee.Substring(3, 2), 16);
            var q = Convert.ToByte(qnnee.Substring(0, 1), 16);
            var north = (q & 0x02) == 0;
            if (north)
            {
                if (ns < 127)
                    ns++;
            }
            else //if south will move north until zero south
            //then step to zero north at the equator, quadrant change,
            //to draw different boundaries
            {
                ns--;
                if (ns >= 0) return String.Format("{0:x1}{1:x2}{2:x2}", q, ns, ew);
                q = (byte)(q & 0x01);//change quadrant to north 
                ns = 0;
            }
            return String.Format("{0:x1}{1:x2}{2:x2}", q, ns, ew);
        }

        public static string MoveS(string qnnee)//string qnnee)
        {
 byte[] ba = Encoding.UTF8.GetBytes(qnnee);
            char[] ca = qnnee.ToCharArray();
            //var qne = new string();
            var n = qnnee.ToString().Substring(1, 2);
            var ns = Convert.ToInt16(qnnee.Substring(1, 2), 16);
            var ew = Convert.ToInt16(qnnee.Substring(3, 2), 16);
            var q = Convert.ToByte(qnnee.Substring(0, 1), 16);
           

            
            var south = (q & 0x02) == 2;
            if (south)
            {
                if (ns < 127)
                    ns++;
            }
            else //if north will move south until zero north
            //then step to zero south at the equator, quadrant change,
            //to draw different boundaries
            
                ns--; if (ns >= 0)  
                    return String.Format("{0:x1}{1:x2}{2:x2}", q, ns, ew);
                   
                         
                q = (byte)(q | 0x02);//change quadrant to south 
                ns = 0;
            
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
                else q = (byte)(q | 0x01);//set west
            }
            else //first change quadrant to west
            //then step to zero west to east,
            //then repeat change to east quadrant go east to west
            {
                if (ew > 0) ew--;
                else q = (byte)(q & 0x02);//set east
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
            bool isWest;
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

                q = (byte)(ns >= 0 & (ew > 0) | ((ew == 0) & (isEast)) ? 0 :  //00 ne
                           ns >= 0 & (ew < 0) | ((ew == 0) & (isWest)) ? 1 :   //01 nw
                           ns < 0 & (ew < 0) | ((ew == 0) & (isWest)) ? 3 : 2);  //03 sw or SE
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


    }
}