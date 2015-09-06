using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmLight1.Model
{
  
    public class patient : person
    {//need to make public access
     // person _person = new person();
     //name { get; set; }
        DateTime dateofbirth { get; set; }
        char gender { get; set; }
        //string cellphone { get; set; }
        //string email { get; set; }
        //string address { get; set; }
       // public string qnnneee { get; set; }
        string[] alerts { get; set; }
        person nextofkin { get; set; }
        List<visit> visits { get; set; }
        List<patient> dependants { get; set; }

        public patient()
        {
            name = "tony";
            address = "123";
            nextofkin.qnnneee = "1222333";
           
        }
    }

    //public class provider : person
    //{ }

}
