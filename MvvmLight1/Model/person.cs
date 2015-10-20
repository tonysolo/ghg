using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmLight1.Model
{
    public class person
    {
        private string _name;

        public string name { get; set; }
        public string address { get; set; }
        public string cellphone { get; set; }
        public string email { get; set; }
        public string qnnneee { get; set; }

        public void remove_delim(ref string str)
        {
            str.Replace('|', ';');
        }


        override public string ToString()
        {
            var sarr = new string[5];
            sarr[0] = name;
            sarr[1] = address;
            sarr[2] = cellphone;
            sarr[3] = email;
            sarr[4] = qnnneee;
            for (int i = 0; i < 5; i++) { remove_delim(ref sarr[i]); }
            return String.Format("{0:sarr[0]}|{1:sarr[1]}|{2:sarr[2]}|{3:sarr[3]}|{4:sarr[4]}");
        }
    }
}


