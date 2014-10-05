using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {

        /// Moves coordinate position North South East or West. Takes care of 
        /// hemisphere - moving north in southern hemisphere requires moving towards 
        /// equator while northern hemisphere north moves towards pole.
        /// Handles 40 arcmin regions and 2.6 arc min district
        /// </summary>
        /// <param name="qnnee"></param>
        /// <param name="nsew">direction 'n','s','e','w'</param>
        /// <returns>region coordinates</returns>
        /// 

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
                //bool dir = true;
                switch (nsew)
                {
                    case 'n': if (ns < 127)   ns++;         break;
                    case 's': if (ns > -127)  ns--;         break;
                    case 'e': isneg = (ew < 0);                       
                         ew = ((ew += 1) % 256);
                         if (isneg == true) ew = (Math.Abs(ew)) * -1;
                        break;
                    case 'w': isneg = (ew < 0);
                        ew = ((ew -= 1) % 256);
                        if (isneg == true) ew = (Math.Abs(ew)) * -1;                     
                        break;
                }
                // else { ew = ew - 1; if (ew == 0)   dir = true; } break;
                //else ew = ew - 1; ew = (ew % 256); if (ew == 255) dir = false; break;                                                                                                    
                // case 'w': ew = (ew - 1) % 256; break                   


              q =  (byte) (((ns >= 0) & (ew >= 0)) ? 0 :  //00 ne
                           ((ns >= 0) & (ew <  0)) ? 1 :   //01 nw
                           ((ns < 0)  & (ew < 0)) ? 3 : 2);   //10 se / sw
                     // ((ns < 0) & (ew < 0)) ? 3 :

              if (ew == 0xff)  q = (byte)(q ^ 0x01);//record the changeover details in 'q'
              if (ew == 0x00)  q = (byte)(q ^ 0x01);//record the changeover details in 'q'

                //int nsa = Math.Abs(ns);
               // int ewa = Math.Abs(ew);

                qnnee = String.Format("{0:x1}{1:x2}{2:x2}", q,  Math.Abs(ns), Math.Abs(ew));

                return qnnee;

            }

            else

                if (qnnee.Length == 7) // 6 character qnnee
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

                    q = ((ns >= 0) && (ew >= 0)) ? 0 :
                         ((ns >= 0) && (ew < 0)) ? 1 :
                         ((ns < 0) && (ew > -1)) ? 2 : 3;

                    qnnee = String.Format("{0:x1}{1:x3}{2:x3}", Math.Abs(q), Math.Abs(ns), Math.Abs(ew));


                }
            return qnnee;
        }

        //--------------------------------------------------------------------------
        //static void Main(string[] args)
        //{
        //string _qnnee = "1f5";
        //string s;
        //return;
        //switch (k){
        //case n: test++; 
        //break;
        //case s: testc--;
        //   






        static void Main(string[] args)
        {



            Console.WriteLine("press 'n', 's', 'e', 'w', or (q to quit)");
            {

                {
                    string _qnnee = "222fe";
                    char k = Console.ReadKey().KeyChar;
                    while (k != 'q')
                    {
                        _qnnee = MoveNSEW(_qnnee, k);
                        Console.WriteLine("  " + _qnnee);
                        k = Console.ReadKey().KeyChar;
                    }

                    return;


                }
            }
        }
    }
}

