using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {

        public static bool isNorth(string qnnee)
        {
            char c = qnnee[0];
            return ((c == '0') || (c == '1'));
        }

        public static bool isSouth(string qnnee)
        {
            char c = qnnee[0];
            return ((c == '2') || (c == '3'));
        }

        public static bool isEast(string qnnee)
        {
            char c = qnnee[0];
            return ((c == '0') || (c == '2'));
        }

        public static bool isWest(string qnnee)
        {
            char c = qnnee[0];
            return ((c == '1') || (c == '3'));
        }

        public static string setQuadrant(string qnnee, char quad) // ne=0 nw=1 se=2 sw=3
        {
            StringBuilder sb = new StringBuilder(qnnee);
            sb[0] = quad;
            return sb.ToString();
        }

        public static string setQuadrant(string qnnee, byte quad) // ne=0 nw=1 se=2 sw=3
        {
            StringBuilder sb = new StringBuilder(qnnee);
            sb[0] = (char)quad;
            return sb.ToString();
        }


        //char SetQuadrant(char quad,char nsew)



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

            if ((qnnee.Length != 5) && (qnnee.Length != 7)) return "";

            if (qnnee.Length == 5) // 5 character qnnee
            {
                int q = Convert.ToInt16(qnnee.Substring(0, 1));
                Int32 ns = Convert.ToInt32(qnnee.Substring(1, 2), 16);
                Int32 ew = Convert.ToInt32(qnnee.Substring(3, 2), 16);

                if (q == 1) ew *= -1;
                if (q == 2) ns *= -1;
                if (q == 3) { ns *= -1; ew *= -1; }
                bool dir = true;
                switch (nsew)
                {
                   case 'n':   if (ns <  127)    ns = ns + 1; break;                                                      
                   case 's':   if (ns > - 127 )  ns = ns - 1; break;
                   case 'e': if (dir == true) { ew = ew + 1; if (ew <= 255) dir = false; }
                                else { ew = ew - 1; if (ew == 0)   dir = true; } break; 
                   //else ew = ew - 1; ew = (ew % 256); if (ew == 255) dir = false; break;                                                                                                    
                   case 'w':  ew =  (ew - 1) % 256; break;
                 } 

                        q = ((ns >= 0) && (ew >= 0)) ? 0 :             
                            ((ns >= 0) && (ew <  0)) ? 1 :               
                            ((ns < 0) && (ew  > -1)) ? 2 : 3;                    

                return String.Format("{0:x1}{1:x2}{2:x2}", Math.Abs(q), Math.Abs(ns), Math.Abs(ew));
            }

          else 

            if (qnnee.Length == 7) // 5 character qnnee
            {
                int q = Convert.ToInt16(qnnee.Substring(0, 1));
                Int32 ns = Convert.ToInt32(qnnee.Substring(1, 3), 16);
                Int32 ew = Convert.ToInt32(qnnee.Substring(3, 3), 16);

                if (q == 1) ew *= -1;
                if (q == 2) ns *= -1;
                if (q == 3) { ns *= -1; ew *= -1; }

                switch (nsew)
                {
                    case 'n': if (ns < 2047) ns = ns + 1; break;
                    case 's': if (ns > -2047) ns = ns - 1; break;
                    case 'e': ew = (ew + 1) % 4095; break;
                    case 'w': ew = (ew - 1) % 4095; break;
                }

                q =  ((ns >= 0) && (ew >= 0)) ? 0 :
                     ((ns >= 0) && (ew  < 0)) ? 1 :
                     ((ns < 0) && (ew  > -1)) ? 2 : 3;

                return String.Format("{0:x1}{1:x3}{2:x3}", Math.Abs(q), Math.Abs(ns), Math.Abs(ew));
            }
            return "";
        }

        //--------------------------------------------------------------------------
        static void Main(string[] args)
        {
            string _qnnee = "022f0";
            Console.WriteLine("press 'n', 's', 'e', 'w', or (q to quit)");
            char k = 'x';
            Console.WriteLine(k);

            while (k != 'q')
            {
                k = Console.ReadKey().KeyChar;
                _qnnee = MoveNSEW(_qnnee, k);
                Console.WriteLine(' ' + _qnnee);
            }
            return;
        }
    }
}
