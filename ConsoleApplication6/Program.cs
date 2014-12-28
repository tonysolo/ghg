using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication6
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = String.Format("{0:x6}",-1);
           // Console.WriteLine("{0:x5}",x);
            int y = Convert.ToInt32(x,16);
            y++;
            Console.WriteLine("{0:x6}",y);
            Console.ReadLine();
        }
    }
}
