using System;

public class oldtested
{
    internal class Program
    {
        /// Moves coordinate position North South East or West. Takes care of 
        /// hemisphere - moving north in southern hemisphere requires moving towards 
        /// equator while northern hemisphere north moves towards pole.
        /// Handles 40 arcmin regions and 2.6 arc min district
        /// 
        /// <param name="qnnee"></param>
        /// <param name="nsew">direction 'n','s','e','w'</param>
        /// <returns>region coordinates</returns>
        public static string MoveNsew(string qnnee, char nsew)
        {
            // string s = "";
            if ((qnnee.Length != 5) && (qnnee.Length != 7)) return "";

            if (qnnee.Length == 5) // 5 character qnnee
            {
                byte q = (byte)qnnee[0];
                int ns = Convert.ToInt16(qnnee.Substring(1, 2), 16);
                int ew = Convert.ToInt16(qnnee.Substring(3, 2), 16);
                if (q == '1') ew *= -1;
                if (q == '2') ns *= -1;
                if (q == '3')
                {
                    ns *= -1;
                    ew *= -1;
                }

                bool isneg;
                switch (nsew)
                {
                    case 'n':
                        if (ns < 127) ns++;
                        break;
                    case 's':
                        if (ns > -127) ns--;
                        break;
                    case 'e':
                        isneg = (ew < 0);
                        ew = ((ew + 1) % 256);
                        if (isneg) ew = (Math.Abs(ew)) * -1;
                        break;
                    case 'w':
                        isneg = (ew < 0);
                        ew = ((ew - 1) % 256);
                        if (isneg) ew = (Math.Abs(ew)) * -1;
                        break;
                }

                q = (byte)(((ns >= 0) & (ew >= 0))
                    ? 0
                    : //00 ne
                    ((ns >= 0) & (ew < 0))
                        ? 1
                        : //01 nw
                        ((ns < 0) & (ew < 0)) ? 3 : 2); //10 se / sw
                // ((ns < 0) & (ew < 0)) ? 3 :

                if (ew == 0xff) q = (byte)(q ^ 0x01); //record the changeover details in 'q'
                if (ew == 0x00) q = (byte)(q ^ 0x01); //record the changeover details in 'q'

                qnnee = String.Format("{0:x1}{1:x2}{2:x2}", q, Math.Abs(ns), Math.Abs(ew));

                return qnnee;
            }

            if (qnnee.Length == 7) // 6 character qnnee
            {
                byte q = (byte)qnnee[0];
                int ns = Convert.ToInt16(qnnee.Substring(1, 3), 16);
                int ew = Convert.ToInt16(qnnee.Substring(4, 3), 16);
                if (q == '1') ew *= -1;
                if (q == '2') ns *= -1;
                if (q == '3')
                {
                    ns *= -1;
                    ew *= -1;
                }

                bool isneg;
                switch (nsew)
                {
                    case 'n':
                        if (ns < 2047) ns++;
                        break;
                    case 's':
                        if (ns > -2047) ns--;
                        break;
                    case 'e':
                        isneg = (ew < 0);
                        ew = ((ew + 1) % 4096);
                        if (isneg) ew = (Math.Abs(ew)) * -1;
                        break;
                    case 'w':
                        isneg = (ew < 0);
                        ew = ((ew - 1) % 4096);
                        if (isneg) ew = (Math.Abs(ew)) * -1;
                        break;
                }

                q = (byte)(((ns >= 0) & (ew >= 0))
                    ? 0
                    : //00 ne
                    ((ns >= 0) & (ew < 0))
                        ? 1
                        : //01 nw
                        ((ns < 0) & (ew < 0)) ? 3 : 2); //10 se / sw
                // ((ns < 0) & (ew < 0)) ? 3 :

                if (ew == 0xfff) q = (byte)(q ^ 0x01); //record the changeover details in 'q'
                if (ew == 0x000) q = (byte)(q ^ 0x01); //record the changeover details in 'q'

                qnnee = String.Format("{0:x1}{1:x3}{2:x3}", q, Math.Abs(ns), Math.Abs(ew));

                return qnnee;
            }
            return "";
        }

        //--------------------------------------------------------------------------


        private static int SecsToMidnight(string qe)
        {
            var q = qe[0];
            var e = Convert.ToByte(qe.Substring(1, 1), 16) * 45 * 60;
            var dt = DateTime.UtcNow;
            var secs = dt.Hour * 60 * 60 + dt.Minute * 60 + dt.Second;
            secs += (q == '1') || (q == '3') ? -e : e; //west                    
            return secs % 0x15180;
        }





        private static void Main()
        {
            AzureUtil.LoadCountry("ZA", "12345,34565,456567");
            string s = AzureUtil.DownloadCountry("ZA");
        }

        /* {
            int i;
            string[] s = {"20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "2a", "sb", "2c", "2d", "2e", "2f", "3f", "3e", "3d", "3c", "3b", "3a", "39", "38", "37", "36", "35", "34", "33", "32", "31", "30"};

            for (i = 0; i < 32;i++)
            {
 Console.Write(SecsToMidnight(s[i]));
Console.Write(" Timezone ");
Console.WriteLine(s[i]);
            }
           
            Console.ReadLine();
            {
            .
               / {
                    string qnnee = "222fc";
                    char k = Console.ReadKey().KeyChar;
                    while (k != 'q')
                    {
                        qnnee = MoveNsew(qnnee, k);
                        Console.WriteLine("  " + qnnee);
                        k = Console.ReadKey().KeyChar;
                    }
                }
            }*/
    }	
}
