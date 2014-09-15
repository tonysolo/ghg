using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string s2= "-26.076,27.972";
            string s = tester.to_qnnee("-26.076,27.972");
            string s1 = tester.IndexPoint(s);


            string b = tester.Boundary(s);
            string c = tester.CentrePoint(s);

            string s4 = "";

        }
    }
}
