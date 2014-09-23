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
            if ((qnnee.Length != 5) && (qnnee.Length != 7)) return "error";
            int lat, lon;
            byte quad = Convert.ToByte(qnnee.Substring(0, 1), 16);
            //0==ne,1==nw,2==se,3==sw

            if (qnnee.Length == 5)
            {
                lat = Convert.ToInt16(qnnee.Substring(1, 2), 16);
                lon = Convert.ToInt16(qnnee.Substring(3, 2), 16);
                //lat *= (isSouth(qnnee)) ? -1 : 1;
                //lon *= (isWest(qnnee)) ? -1 : 1;

                switch (char.ToLower(nsew))
                {
                    case 'n': if ((isNorth(qnnee)) && (lat < 128)) lat++;
                        //north move north but stop before N pole                 
                        else if ((isSouth(qnnee)) && (lat > 0))
                        {
                            lat--;//if south hemispere move north to equator
                            if (lat == 0)
                            {
                                quad -= 2;
                                qnnee = setQuadrant(qnnee, quad);
                            }//then set quadrant to north                     
                        } break;

                    case 's': if ((isSouth(qnnee)) && (lat < 128)) lat++;//south move south but stop before S pole
                        else if ((isNorth(qnnee)) && (lat > 0)) //if north move south to equator                                 
                        {
                            lat--;
                            if (lat == 0)
                            {
                                quad += 2;
                                qnnee = setQuadrant(qnnee, quad);
                            }//set quadrant to south
                        }
                        break;

                    case 'e': if ((quad & 0x01) == 0)
                            if (lon < 256) lon++; 
                            else { quad += 1; 
                           qnnee = setQuadrant(qnnee, quad); }//change quadrant from west to east
                        break;

                    case 'w': if ((quad & 0x01) == 1) 
                        if (lon > 0) lon--; 
                        else { quad -= 1; 
                        qnnee = setQuadrant(qnnee, quad); } //if lon will be < 0 change quadrant
                        break;
                }
                return String.Format("{0:x1}{1:x2}{2:x2}", quad, lat, lon);
            }

//try to make seemless movement accross equator and meridians by changing directions
            //of inc and decrement depending on current position rather than default quadrant handling

            else // qnnneee == 7 characters
            {
                lat = Convert.ToInt16(qnnee.Substring(1, 3), 16);
                lon = Convert.ToInt16(qnnee.Substring(4, 3), 16);
                lat *= (isSouth(qnnee)) ? -1 : 1;
                lon *= (isWest(qnnee)) ? -1 : 1;

                switch (char.ToLower(nsew))
                {
                    case 'n': if ((isNorth(qnnee)) && (lat < 2048))  lat++;    //check quadrant for direction
                               else if (isSouth(qnnee) && (lat < 2048))  lat--; //if south hemispere move north to equator
                             //if get to equator then change quadrant
                            if (lat == 0)//then set quadrant to north  
                            { quad += 2;  qnnee = setQuadrant(qnnee, quad); }  break;                                                                                          

                    case 's': if ((isSouth(qnnee)) && (lat < 2048)) lat++;                        
                        else if ((isNorth(qnnee)) && (lat < 2048)) lat--;                      
                            break;//check quadrant for direction
                       

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
            string qne = "22222";
            Console.WriteLine("press 'n', 's', 'e', 'w', or (q to quit)");
            char k = 'x';
            Console.WriteLine(k);


            while (k != 'q')
            {
                k = Console.ReadKey().KeyChar;
                //Console.WriteLine();
                qne = MoveNSEW(qne, k);
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
