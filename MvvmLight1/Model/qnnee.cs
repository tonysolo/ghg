using System;

namespace MvvmLight1.Model
{

    public class qnnee
    {

        static byte[] _qne;
// need to get a copy of the shareddata qne and update this when the qnnee editor closes;


        public string AzureTrim(string instr)
        {
            string outst = "";
            for (int i = 0; i < instr.Length; i++)
            {
                if (Char.IsLetterOrDigit(instr[i])) outst += instr[i];
            }
            return outst;
        }


        public qnnee(byte[] qne)
        {

            _qne = qne;
        }

        public qnnee(string qne)
        {
            _qne = new byte[3];
             var _qnee = AzureTrim(qne);
            if(_qnee != null)
                if (_qnee.Length == 5) { 
            var x = _qnee.Length; 
           _qne[0] = Convert.ToByte(_qnee.Substring(0,1));
           _qne[1] = Convert.ToByte(_qnee.Substring(1, 2), 16);
           _qne[2] = Convert.ToByte(_qnee.Substring(3, 2), 16);
            //_qne = String.Format("{0:x1}{1:x2}{2:x2}", _qne1, _qne2, _qne3);
}
        }

        bool isnorth() { return (_qne[0] & 0x02) == 0; }
        bool issouth() { return (_qne[0] & 0x02) == 2; }
        bool iseast() { return (_qne[0] & 0x01) == 0; }
        bool iswest() { return (_qne[0] & 0x01) == 1; }


        public void seteast() { _qne[0] &= 0xfe; return; } //clear bit 0
        public void setwest() { _qne[0] |= 0x01; return; } //set bit 0
        public void setsouth() { _qne[0] |= 0x02; return; }   // set bit 1
        public void setnorth() { _qne[0] &= 0xfd; return; }    //clear bit 1

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

        //converts a qnnee string to byte[3]
        //used for qnnee strings
        byte[] getqnebytes(string s)
        {
            if (s.Length != 5) return null;
            byte[] ba = new byte[3];
            ba[0] = Convert.ToByte(s.Substring(0, 1), 16);
            ba[1] = Convert.ToByte(s.Substring(1, 2), 16);
            ba[2] = Convert.ToByte(s.Substring(3, 2), 16);
            return ba;
        }

        //used to get qnnee strings from bytearrays
        //converts byte[3]
        string gethexstring(byte[] barr)
        {
            return String.Format("{0:x1}{1:x2}{2:x2}", barr[0], barr[1], barr[2]);
        }

        public void movN(ref string x)
        {

        }

 public void movS(ref string x)
        {

        }

 public void movE(ref string x)
        {

        }
        public void movW(ref string x)
        {

        }


        public string toString()
        {
            return String.Format("{0:x1}{1:x2}{2:x2}", _qne[0], _qne[1], _qne[2]);
        }
        //centrepoint
        //border
        //qnnneee
        //qnnneee border
        //qnnneee centrepoint
        //secs to midnight
        //tostring qnnee

        //   public class qnnee    //not a static class
        //   {
        //       static byte[] _qne;

        //       public qnnee(string s) //string qnnee constructor
        //       {
        //           _qne = new byte[3];
        //           _qne[0] = Convert.ToByte(s.Substring(0, 1), 16);
        //           _qne[1] = Convert.ToByte(s.Substring(1, 2), 16);
        //           _qne[2] = Convert.ToByte(s.Substring(3, 2), 16);
        //       }

        //       public qnnee(byte[] qne) //byte[3] constructor
        //       {
        //           _qne = qne;
        //       }

        //       bool isnorth() { return (_qne[0] & 0x02) == 0; }
        //       bool issouth() { return (_qne[0] & 0x02) == 2; }
        //       bool iseast() { return (_qne[0] & 0x01) == 0; }
        //       bool iswest() { return (_qne[0] & 0x01) == 1; }


        //       void seteast() { _qne[0] &= 0xfe; return; } //clear bit 0
        //       void setwest() { _qne[0] |= 0x01; return; } //set bit 0
        //       void setsouth() { _qne[0] |= 0x02; return; }   // set bit 1
        //       void setnorth() { _qne[0] &= 0xfd; return; }    //clear bit 1

        //       public byte[] movN()
        //       {
        //           if (isnorth() && (_qne[1] < 127)) { _qne[1]++; } //return _qne; }
        //           else 
        //           if (issouth() && (_qne[1] > 0)) _qne[1]--; else setnorth();// return _qne;
        //           return _qne;
        //       }

        //       public void movN(ref byte[] x)
        //       {
        //           if (isnorth() && (x[1] < 127)) { x[1]++; return; }
        //           if (issouth() && (x[1] > 0)) x[1]--; else setnorth(); return;
        //       }


        //       public byte[] movS()
        //       {
        //           if (issouth() && (_qne[1] < 127)) { _qne[1]++; }
        //           else
        //           if (isnorth() && (_qne[1] > 0)) _qne[1]--; else setsouth();
        //           return _qne;

        //       }
        //       public void movS(ref byte[] x)
        //       {
        //           if (issouth() && (x[1] < 127)) { x[1]++; return; }
        //           if (isnorth() && (x[1] > 0)) x[1]--; else setsouth(); return;
        //       }

        //       public byte[] movE()
        //       {
        //           if (iswest() && (_qne[2] == 0)) { seteast(); return _qne; }
        //           if (iseast() && (_qne[2] == 255)) { setwest(); return _qne; }
        //           if (iseast()) {_qne[2]++; return _qne; }
        //           if (iswest()) {_qne[2]--; return _qne; }
        //           return _qne;
        //       }

        //       public void movE(ref byte[] x)
        //       {
        //           if (iswest() && (x[2] == 0)) { seteast(); return; }
        //           if (iseast() && (x[2] == 255)) { setwest(); return; }
        //           if (iseast()) { x[2]++; return; }
        //           if (iswest()) { x[2]--; return; }
        //       }

        //public void movW()
        //       {
        //           if (iseast() && (x[2] == 0)) { setwest(); return; }
        //           if (iswest() && (x[2] == 255)) { seteast(); return; }
        //           if (iseast()) { x[2]--; return; }
        //           if (iswest()) { x[2]++; return; }
        //       }


        //       public void movW(ref byte[] x)
        //       {
        //           if (iseast() && (x[2] == 0)) { setwest(); return; }
        //           if (iswest() && (x[2] == 255)) { seteast(); return; }
        //           if (iseast()) { x[2]--; return; }
        //           if (iswest()) { x[2]++; return; }
        //       }

        /// <summary>
        ///     private method used by the library
        ///     latlon string point closest to meridian and equator
        /// </summary>
        /// <param name="qnnee">5 or 7 character hex </param>
        /// <returns>latlon string  as csv</returns>
        public static string IndexPoint(string qnnee)
        {
            double lat = 0, lon = 0;
            //    var east = (qnnee[0] == '0');

            switch (_qne.Length)
            {
                case 3:

                    //var lat5 = Convert.ToInt16(qnnee.Substring(1, 2), 16);
                    // var lon5 = Convert.ToInt16(qnnee.Substring(3, 2), 16);
                    lat = (double)_qne[1] / 256 * 180;
                    lon = (double)_qne[2] / 256 * 180;
                    break;
                case 5:   // need this later for 5 byte qne      
                    {
                        break;
                    }
                    //    var lon5 = Convert.ToInt16(qnnee.Substring(3,2), 16);
                    //   lon = (double)lon7 / 4096 * 180;
                    //  break;
            }
            if ((qnnee[0] == '1') || (qnnee[0] == '3')) lon *= -1;
            else if ((qnnee[0] == '2') || (qnnee[0] == '3')) lat *= -1;
            // var s = String.Format("{0} {1:en-US}", lat, lon);
            return lat.ToString("F2") + ',' + lon.ToString("F2");  //"F2",Ci
        }

        //  public string toString()
        //       return string.Format("{0:x1}{1:x2}{2:x2}", _qne[0], _qne[1], _qne[2]);
        //   }
    }
}


