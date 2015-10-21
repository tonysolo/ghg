using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmLight1.Model
{
    public class patient : person
    {
        DateTime dateofbirth { get; set; }
        char gender { get; set; }             
        person nextofkin { get; set; }  // or carer
        List<condition> alerts { get; set; }
        List<visit> visits { get; set; }     
        List<patient> dependants { get; set; }
        DateTime nextvisit { get; set; }

        public patient()
        {
            name = "tony";
            address = "123";
            //nextofkin.
            nextofkin.qnnneee = "1222333";         
        }

        public patient(string healthid)
        {
//check provider is in same county
        }

        public void save_to_azure()
        {
//limit to 64 kb prepend 3 bytes - 8bit version / 16bit length
        }

    }

    //public class provider : person
    //{ }

}
