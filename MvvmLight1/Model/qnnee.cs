using System;

namespace MvvmLight1.Model
{

    public static class 
        QnneeUtil
    {

        public static string AzureTrim(string instr) //trims hidden chars
        {
            var outst = "";
            for (int i = 0; i < instr.Length; i++)
            {
                if (Char.IsLetterOrDigit(instr[i])) outst += instr[i];
            }
            return outst;
        }

        static bool Isnorth(byte[] x) { return (x[0] & 0x02) == 0; }
        static bool Issouth(byte[] x) { return (x[0] & 0x02) == 2; }
        static bool Iseast(byte[] x) { return (x[0] & 0x01) == 0; }
        static bool Iswest(byte[] x) { return (x[0] & 0x01) == 1; }


        public static void Seteast(ref byte[] x) { x[0] &= 0xfe; return; } //clear bit 0
        public static void Setwest(ref byte[] x) { x[0] |= 0x01; return; } //set bit 0
        public static void Setsouth(ref byte[] x) { x[0] |= 0x02; return; }   // set bit 1
        public static void Setnorth(ref byte[] x) {x[0] &= 0xfd; return; }    //clear bit 1

        public static void MovN(ref byte[] x)
        {
            if (Isnorth(x) && (x[1] < 127)) { x[1]++; return; }
            if (Issouth(x) && (x[1] > 0)) x[1]--; else Setnorth(ref x); return;
        }


        public static void MovS(ref byte[] x)
        {
            if (Issouth(x) && (x[1] < 127)) { x[1]++; return; }
            if (Isnorth(x) && (x[1] > 0)) x[1]--; else Setsouth(ref x); return;
        }


        public static void MovE(ref byte[] x)
        {
            if (Iswest(x) && (x[2] == 0)) { Seteast(ref x); return; }
            if (Iseast(x) && (x[2] == 255)) { Setwest(ref x); return; }
            if (Iseast(x)) { x[2]++; return; }
            if (Iswest(x)) { x[2]--; return; }
        }


        public static void MovW(ref byte[] x)
        {
            if (Iseast(x) && (x[2] == 0)) { Setwest(ref x); return; }
            if (Iswest(x) && (x[2] == 255)) { Seteast(ref x); return; }
            if (Iseast(x)) { x[2]--; return; }
            if (!Iswest(x)) return;
            x[2]++;
        }

        //converts a qnnee string to byte[3]   
        private static byte[] Getqnebytes(string st)
        {
            var s = AzureTrim(st);
            byte[] ba = new byte[3];
            ba[0] = Convert.ToByte(s.Substring(0, 1), 16);
            ba[1] = Convert.ToByte(s.Substring(1, 2), 16);
            ba[2] = Convert.ToByte(s.Substring(3, 2), 16);
            return ba;
        }

        //used to get qnnee strings from bytearrays
        //converts byte[3] to 5 character string
        private static string Gethexstring(byte[] barr)
        {
            return $"{barr[0]:x1}{barr[1]:x2}{barr[2]:x2}";
        }

        public static void MovN(ref string x)
        {
            var b = Getqnebytes(x);
            MovN(ref b);
            x = Gethexstring(b);
        }

        public static void MovS(ref string x)
        {
            var b = Getqnebytes(x);
            MovS(ref b);
            x = Gethexstring(b);
        }

        public static void MovE(ref string x)
        {
            var b = Getqnebytes(x);
            MovE(ref b);
            x = Gethexstring(b);
        }

        public static void MovW(ref string x)
        {
            var b = Getqnebytes(x);
            MovW(ref b);
            x = Gethexstring(b);
        }

    }
}


