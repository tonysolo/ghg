using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    class Program
    {


        static void Main(string[] args)
        {

            int dat = 0;
            char c;
            bool goup = true;
             max = (0xff | 0x01);
            int min = 0;

            while (true)
            {
                c = Console.ReadKey().KeyChar;
                {
                    switch (c)
                    {
                        case 'e': if ((dat < max)&&(goup)) dat = dat + 1; else () dat = dat - 1; break;
                        case 'w': dat = dat - 1; break;
                    }

                }
                Console.WriteLine(String.Format("  {0:x3}", dat));
            }

        }

    }
}
