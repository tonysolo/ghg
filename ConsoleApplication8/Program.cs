using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication8
{

    public static class qnnee
    {
        static bool isnorth, issouth, iseast, iswest;

        public static void movN(ref byte[] x)
        {
            if (issouth && (x[1] == 0)) { isnorth = true; return; } 
            else if ((isnorth) && (x[1] < 127)) { x[1]++; return; }
            else if ((issouth) && (x[1] > 0)) { x[1]--; return; }
            return;
        }

        public static void movS(ref byte[] x)
        {
            if (isnorth && (x[1] == 0)) { issouth = true; return; }
            if ((issouth) && (x[1] < 127)) { x[1]++; return; }
            else if ((isnorth) && (x[1] > 0)) { x[1]--; return; }
            return;
        }

        public static void movE(ref byte[] x)
        {
            if (iswest && (x[2] == 0)) { iseast = true; return; }
            if (iseast && (x[2] < 255)) { x[2]++; return; }
            else if ((iswest) && (x[2] > 0)) { x[2]--; return; }
            return;
        }
        public static void movW(ref byte[] x)
        {
            if (iseast && (x[2] == 0)) { iswest = true; return; }
            if (iswest && (x[2] < 255)) { x[2]++; return; }
            else if ((iseast) && (x[2] > 0)) { x[2]--; return; }
        }





        static void Main()
        {
            byte[] ba = new byte[3];
            ba[0] = 1;  //quadrant byte
            ba[1] = 2;  //longitude byte
            ba[2] = 2;  //latitude byte

qnnee.movN(ref ba);
qnnee.movN(ref ba);
qnnee.movN(ref ba);
qnnee.movN(ref ba);
qnnee.movN(ref ba);
qnnee.movN(ref ba);
qnnee.movN(ref ba);


            qnnee.movN(ref ba);
            Console.WriteLine("{0:x1}{1:x2}{2:x2}", ba[0], ba[1], ba[2]);

            movS(ref ba);
            Console.WriteLine("{0:x1}{1:x2}{2:x2}", ba[0], ba[1], ba[2]);


            movE(ref ba);
            Console.WriteLine("{0:x1}{1:x2}{2:x2}", ba[0], ba[1], ba[2]);

            movW(ref ba);
            Console.WriteLine("{0:x1}{1:x2}{2:x2}", ba[0], ba[1], ba[2]);
            Console.ReadLine();

            return;
        }
        // Define other methods and classes here

    }

   
}
