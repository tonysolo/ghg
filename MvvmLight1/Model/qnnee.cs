using System;

namespace MvvmLight1.Model
{
    public class qnnee    //not a static class
    {
        static byte[] _qne;

        public qnnee(string s) //string qnnee constructor
        {
            _qne = new byte[3];
            _qne[0] = Convert.ToByte(s.Substring(0, 1), 16);
            _qne[1] = Convert.ToByte(s.Substring(1, 2), 16);
            _qne[2] = Convert.ToByte(s.Substring(3, 2), 16);
        }

        public qnnee(byte[] qne) //byte[3] constructor
        {
            _qne = qne;
        }

        bool isnorth() { return (_qne[0] & 0x02) == 0; }
        bool issouth() { return (_qne[0] & 0x02) == 2; }
        bool iseast() { return (_qne[0] & 0x01) == 0; }
        bool iswest() { return (_qne[0] & 0x01) == 1; }


        void seteast() { _qne[0] &= 0xfe; return; } //clear bit 0
        void setwest() { _qne[0] |= 0x01; return; } //set bit 0
        void setsouth() { _qne[0] |= 0x02; return; }   // set bit 1
        void setnorth() { _qne[0] &= 0xfd; return; }    //clear bit 1

        public void movN(ref byte[] x)
        {
            if (isnorth() && (x[1] < 127)) { x[1]++; return; }
            if (issouth() && (x[1] > 0)) x[1]--; else setnorth(); return;
        }


        public void movS(ref byte[] x)
        {
            if (issouth() && (x[1] < 127)) { x[1]++; return; }
            if (isnorth() && (x[1] > 0)) x[1]--; else setsouth(); return;
        }


        public void movE(ref byte[] x)
        {
            if (iswest() && (x[2] == 0)) { seteast(); return; }
            if (iseast() && (x[2] == 255)) { setwest(); return; }
            if (iseast()) { x[2]++; return; }
            if (iswest()) { x[2]--; return; }
        }


        public void movW(ref byte[] x)
        {
            if (iseast() && (x[2] == 0)) { setwest(); return; }
            if (iswest() && (x[2] == 255)) { seteast(); return; }
            if (iseast()) { x[2]--; return; }
            if (iswest()) { x[2]++; return; }
        }

        public string toString()
        {
            return string.Format("{0:x1}{1:x2}{2:x2}", _qne[0], _qne[1], _qne[2]);
        }
    }
}

