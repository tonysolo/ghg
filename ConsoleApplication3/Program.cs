using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Program
    {
        static public string[] tst ={"00","01","02","03","04","05","06","07","08","09","0A","0B","0C","0D","0E","0F",
                                    "10","11","12","13","14","15","16","17","18","19","1A","1B","1C","1D","1E","1F"};
       
        static void Main(string[] args)
        {

         for (int i = 0;i<32;i++)
          Console.WriteLine("Zone {0:x}  {1}  ", i ,test.secstomidnight(tst[i])/60);
          Console.ReadLine();

        }
    }
}
