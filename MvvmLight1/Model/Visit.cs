using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmLight1.Model
{//need to make public access
    class visit
    {
        DateTime date { get; set; }
        string referral { get; set; }
        string description { get; set; }
        string presciption { get; set; }
        List<string> images { get; set; } //change to string[] image address qnnee/i...
        string advise { get; set; }
        string saveimage() { return null; } // saves to azure images and returns offset and length 
                                       // visit size will be less than 512 bytes

        public void addate()
        {
            date = DateTime.Now;
            byte[] ba = new byte[100];
            //images.Add(ba);
        }

       public  visit()
        {
            string str = "tony";
            description = null;
        }

        public void savetoazure()
        {
// bool imageok = saveimages to azure
//if (this.Size =< 512) and imageok save to azure otrherwise warn failure and exit 
        }
    }



}
