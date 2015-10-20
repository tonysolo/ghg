using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmLight1.Model
{//need to make public access
    class visit
    { // ?? visit size will be less than 512 bytes
        DateTime date { get; set; } //get set to convert to dt
        string referral { get; set; }
        string description { get; set; }
        List<condition> condition { get; set; } //change to delim string
        string presciption { get; set; }
        string advise { get; set; }
        DateTime nextvisit { get; set; } //get set to convert to dt
        List<string> images { get; set; } //change to delim string image address qnnee/i...    
        int[] saveimage() { return null; } // saves to azure images and returns offset and length []
                               

        public void addate()
        {
            date = DateTime.Now;
            byte[] ba = new byte[100];
            //images.Add(ba);
        }

       public  visit()
        {
            
            description = null;
        }

        public void savetoazure()
        {
//save to storage queue ...
// bool imageok = saveimages to azure
//if (this.Size =< 512) and imageok save to azure otrherwise warn failure and exit 
        }

        public void checkandupdate()
        {

        }

    }



}
