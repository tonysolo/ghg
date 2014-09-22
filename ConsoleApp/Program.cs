using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        /// <summary>
        /// Moves coordinate position North South East or West. Takes care of 
        /// hemisphere eg moving north in southern hemisphere requires moving towards 
        /// equator while nornern hemispher moves towards pole.
        /// Handles 40 arcmin regions and 2.6 arc min district
        /// </summary>
        /// <param name="qnnee"></param>
        /// <param name="nsew">direction 'n','s','e','w'</param>
        /// <returns>region coordinates</returns>
        public static string MoveNSEW(string qnnee, char nsew)
        {
            if ((qnnee.Length != 5) && (qnnee.Length != 7)) return "error";
            int lat, lon;
            byte quad;//0==ne,1==nw,2==se,3==sw
            if (qnnee.Length == 5)
            {  
                quad = Convert.ToByte(qnnee[0]);
                lat = Convert.ToInt16(qnnee.Substring(1, 2), 16);
                lon = Convert.ToInt16(qnnee.Substring(3, 2), 16);


                lat *= (quad & 0x01)==1 ? -1 : 1;

                switch (char.ToLower(nsew))
                {
                    case 'n': if ((quad < 2)&&(lat < 128)) lat++; break;//north hemisphere move north but stop before pole pole
                    case 's': if (lat >= 0) lat--; else  break; //check quadrant for direction
                    case 'e': if (lon < 256) lon++; break;
                    case 'w': if (lon >= 0) lon--; break;
                }

                return String.Format("{0:x1}{1:x2}{2:x2}", quad, lat, lon);
            }

//try to make seemless movement accross equator and meridians by changing directions
//of inc and decrement depending on current position rather than default quadrant handling

            else // qnnneee == 7 characters
            {
                lat = Convert.ToInt16(qnnee.Substring(1, 3), 16);
                lon = Convert.ToInt16(qnnee.Substring(4, 3), 16);
                switch (char.ToLower(nsew))
                {
                    case 'n': if (lat < 2048) lat++; break;//check quadrant for direction
                    case 's': if (lat >= 0) lat--; break;//check quadrant for direction
                    case 'e': if (lon < 4096) lon++; break;
                    case 'w': if (lon >= 0) lon--; break;
                }


                if (quad == 1) lat *= -1;
                if (quad == 2) lon *= -1;
                if (quad == 3)
                {
                    lat *= -1; lon *= -1;
                }

                switch (char.ToLower(nsew))
                {
                    case 'n':
                        {
                            lat += 1;
                           // if (lat >= 0) { quad &= 0x01; }
                            break;
                        }
                    case 's':
                        {
                            lat -= 1;
                          //  if (lat < 0) { quad |= 0x02; }
                            break;
                        }
                    case 'e':
                        {
                            lon += 1;
                           // if (lon >= 0) { quad &= 0x10; }
                            break;
                        }
                    case 'w':
                        {
                            lon -= 1;
                          //  if (lon < 0) { quad |= 0x01; }
                            break;
                        }

                }
                return String.Format("{0:x1}{1:x3}{2:x3}", quad, lat, lon);
            }
        }




        //--------------------------------------------------------------------------
        static void Main(string[] args)
        {
            string qne = "20204";
           Console.WriteLine("press 'n', 's', 'e', 'w', or (q to quit)");
           char k = 'x';
           Console.WriteLine();

            while (k != 'q')
            {
                k = Console.ReadKey().KeyChar;         
                qne = MoveNSEW(qne,k);
                Console.WriteLine(qne);            
           }

            return;
        }
    }
}

/*
 static void Main(string[] args)
        {
            string s = "";
            string s1 = "-26.076,27.972"; 
          
            s = QNE_Utils.to_qnnneee(s1);
            Console.WriteLine(s);
            Console.WriteLine(QNE_Utils.IndexPoint(s));
            Console.WriteLine(QNE_Utils.CentrePoint(s));
 Console.WriteLine();
            Console.WriteLine(QNE_Utils.Boundary(s));

 Console.WriteLine();


            s = QNE_Utils.to_qnnee(s1);
            Console.WriteLine(s);
            Console.WriteLine(QNE_Utils.IndexPoint(s));
            Console.WriteLine(QNE_Utils.CentrePoint(s));
            Console.WriteLine();
            Console.WriteLine(QNE_Utils.Boundary(s));   
            Console.ReadLine();
 * 
 * 
 *  }
            else if (qnnee.Length == 7)
            {
                lat = Convert.ToInt16(qnnee.Substring(1, 3), 16);
                lon = Convert.ToInt16(qnnee.Substring(4, 3), 16);
                if (quad == 1) lat *= -1;
                if (quad == 2) lon *= -1;
                if (quad == 3)
                {
                    lat *= -1; lon *= -1;
                }

 * 
        }
 */
