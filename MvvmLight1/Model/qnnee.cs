using System;

namespace MvvmLight1.Model
{

    public static class qnnee_util
    {

        public static string AzureTrim(string instr) //trims hidden chars
        {
            string outst = "";
            for (int i = 0; i < instr.Length; i++)
            {
                if (Char.IsLetterOrDigit(instr[i])) outst += instr[i];
            }
            return outst;
        }

        static bool isnorth(byte[] x) { return (x[0] & 0x02) == 0; }
        static bool issouth(byte[] x) { return (x[0] & 0x02) == 2; }
        static bool iseast(byte[] x) { return (x[0] & 0x01) == 0; }
        static bool iswest(byte[] x) { return (x[0] & 0x01) == 1; }


        static public void seteast(ref byte[] x) { x[0] &= 0xfe; return; } //clear bit 0
        static public void setwest(ref byte[] x) { x[0] |= 0x01; return; } //set bit 0
        static public void setsouth(ref byte[] x) { x[0] |= 0x02; return; }   // set bit 1
        static public void setnorth(ref byte[] x) {x[0] &= 0xfd; return; }    //clear bit 1

        static public void movN(ref byte[] x)
        {
            if (isnorth(x) && (x[1] < 127)) { x[1]++; return; }
            if (issouth(x) && (x[1] > 0)) x[1]--; else setnorth(ref x); return;
        }


        static public void movS(ref byte[] x)
        {
            if (issouth(x) && (x[1] < 127)) { x[1]++; return; }
            if (isnorth(x) && (x[1] > 0)) x[1]--; else setsouth(ref x); return;
        }


        static public void movE(ref byte[] x)
        {
            if (iswest(x) && (x[2] == 0)) { seteast(ref x); return; }
            if (iseast(x) && (x[2] == 255)) { setwest(ref x); return; }
            if (iseast(x)) { x[2]++; return; }
            if (iswest(x)) { x[2]--; return; }
        }


        static public void movW(ref byte[] x)
        {
            if (iseast(x) && (x[2] == 0)) { setwest(ref x); return; }
            if (iswest(x) && (x[2] == 255)) { seteast(ref x); return; }
            if (iseast(x)) { x[2]--; return; }
            if (iswest(x)) { x[2]++; return; }
        }

        //converts a qnnee string to byte[3]   
        static byte[] getqnebytes(string st)
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
        static string gethexstring(byte[] barr)
        {
            return String.Format("{0:x1}{1:x2}{2:x2}", barr[0], barr[1], barr[2]);
        }

        static public void movN(ref string x)
        {
            var b = getqnebytes(x);
            movN(ref b);
            x = gethexstring(b);
        }

        static public void movS(ref string x)
        {
            var b = getqnebytes(x);
            movS(ref b);
            x = gethexstring(b);
        }

        static public void movE(ref string x)
        {
            var b = getqnebytes(x);
            movE(ref b);
            x = gethexstring(b);
        }

        static public void movW(ref string x)
        {
            var b = getqnebytes(x);
            movW(ref b);
            x = gethexstring(b);
        }

    }
}


