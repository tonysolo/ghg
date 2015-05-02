using System;
using System.Linq;

namespace ConsoleApplication7
{
    delegate void D(string value);
    class Program
    {

        private static void Test(string t)
        {
            if (t == null) return;
            var lst = t.Where(n => n=='t');
            foreach(var c in lst)
                Console.WriteLine(c);
        }

        //private int x = y => { y; };

        static void Main(string[] args)
        {
            D d = Test;
            d.Invoke("tonytonytony");
            Console.ReadLine();
            // const string str = "tony";
            //  var test1 = str.Substring(0, 2);
            //  Console.WriteLine(test1);
        }
    }
}
