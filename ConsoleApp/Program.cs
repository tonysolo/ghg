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
        }
    }
}
