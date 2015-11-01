using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmLight1.Model
{
    public class patient : person
    {
        public DateTime dateofbirth { get; set; }
        public char gender { get; set; }             
        person nextofkin { get; set; }  // or carer
        List<condition> alerts { get; set; }
        List<visit> visits { get; set; } 
        //List<patient> patients { get; set; }   
        List<patient> dependants { get; set; }
        public DateTime nextvisit { get; set; }
        public DateTime lastvisit { get; set; }
        public int age { get { return (int)(DateTime.Today - dateofbirth).Days / 365; } }


        public string nextv
        {
            get { return nextvisit.ToString("dd MM yyyy"); }
        }   

        public patient()
        {
            //name = "Tony M";
            //gender = 'M';
            //address = "123";
            //nextofkin.
            //nextofkin.qnnneee = "1222333";         
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

    }

    //public class provider : person
    //{ }


