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
                
                char q = qnnee[0];
                int ns = Convert.ToUInt16(qnnee.Substring(1, 2), 16);
                int ew = Convert.ToUInt16(qnnee.Substring(3, 2), 16);
                if (q == '1') { ew *= -1;}
                if (q == '2') ns *= -1;
                if (q == '3') { ns *= -1; ew *= -1; }
                if (ew < 0) ew += 512;

                switch (nsew)
                {
                    case 'n': {if (ns < 127) ns++; break; }
                    case 's': {if (ns > -127) ns--; break; }
                    case 'e': { ew = (ew + 1) ; break; }
                    case 'w': { ew = (ew -1); break; }                                             
                }
                ew = ew % 512;
                if (ew > 256) ew = ew - 512;

                q = (char) (((ns > -1) && ((ew & 0xff) <  0)) ? 0 :                       //ne  q0 = ne+ n 0..128    ew  0 ..255
                            ((ns > -1) && ((ew & 0xff) >  0)) ? 1:                        //nw  q1 = nw+ n 0..128    ew  256 ..511
                            ((ns <  0) && ((ew & 0xff) == 0)) ? 2 : 3);                                           //se  q2 = se- en 0..-128  ew  0..-255
                                                                                                                  //sw  q3 = sw- en 0..-128  ew -256..-511

                return String.Format("{0:x1}{1:x2}{2:x2}", Math.Abs(q), Math.Abs(ns),Math.Abs(ew));
            }
            return "";
        }

               // int _nn, _ee;
               // _nn = Convert.ToInt16(_qnnee.Substring(1, 2), 16); //convert to integers
               // _ee = Convert.ToInt16(_qnnee.Substring(3, 2), 16);//and keep in range
               // if (_nn > 127) _nn = 127; 
                //00 == north east == default 
               // if (q == '1') _ee *= -1;                      //01 == north west                            
               // else if (q == '2') _nn *= -1;                 //10 == south east
               // else if (q == '3') { _ee *= -1; _nn *= -1; } //11 == south west            


              //  switch (char.ToLower(nsew))
               // {
              //      case 'n': _nn += 1; if (_nn > 127) _nn = 127; break;     //inc N latitude and longitude truncatw at poles           
               //     case 's': _nn -= 1; if (_nn < -127) _nn = -127; break;   //inc S latitude - equator = 127
                 //   case 'e': if (_ee < 255) _ee += 1; else _ee -= 1; break;
                  //  case 'w': if (_ee > -255) _ee -= 1; else _ee += 1; break;  // dec latitude  change 
             //       case 'e': _ee += 1; _ee = (_ee % 256); if (_ee == 0) { _ee = 255; } break;  //toggle ew and reverse   
            //        case 'w': _ee -= 1; _ee = (_ee % 256); break;                                                                                                                         
           //     }
                // reform the qnnee string
            //    int x = ((_nn >= 0) && (_ee >= 0)) ? 0 :   //  north east  0,0 (0)
            //            ((_nn >= 0) && (_ee < 0)) ? 1 :    //  north west  0,1 (1)
            //            ((_nn < 0) && (_ee >= 0)) ? 2 : 3; //  southeast   1,0 (2) southwest 1,1  (3)
     //
            //    string s = String.Format("{0:x1}{1:x2}{2:x2}", Math.Abs((int)x), Math.Abs(_nn), Math.Abs(_ee));
            //    return s;
         /*   }



            else if (qnnee.Length == 7)  //7 character qnnneee
            {

                int nnn, eee;
                nnn = Convert.ToInt16(_qnnee.Substring(1, 3), 16);
                eee = Convert.ToInt16(_qnnee.Substring(4, 3), 16);
                if (q == 1) eee *= -1; //01 == north west
                else if (q == 2) nnn *= -1;//10 == south east
                else if (q == 3) { eee *= -1; nnn *= -1; }//south west
                switch (char.ToLower(nsew))
                {
                    case 'n': nnn += 1; break;
                    case 's': nnn -= 1; break;
                    case 'e': eee += 1; break;
                    case 'w': eee -= 1; break;
                }
                int x = ((nnn >= 0) && (eee >= 0)) ? 0 :
                         ((nnn >= 0) && (eee < 0)) ? 1 :
                         ((nnn < 0) && (eee >= 0)) ? 2 : 3;   //0==ne,1==nw,2==se,3==sw
                if (eee == 255) ; //dir = !dir; ;
                return String.Format("{0:x1}{1:x3}{2:x3}", q, nnn, eee);
            }
            else return "";
        }
*/
        //--------------------------------------------------------------------------
        static void Main(string[] args)
        {
            string _qnnee = "22222";
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
