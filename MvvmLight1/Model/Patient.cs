using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//patients get 64 kilobytes of data
//visits store links to images (so massive ehealth storage using images is possible) 
//layout
// 0 name,address,cel,email,qnnneee,dob,Gender,nok,relationship,address,cell,email,qnnneee
// 1 Alerts ...
// 2 Visits ... next visit
// 3 Dependants qnneexxx,qnneexxx..



namespace MvvmLight1.Model
{
    public class Patient : Person
    {
        public string Id { get; set; } //qnneepxxxx
        public DateTime Dateofbirth { get; set; }
        public char Gender { get; set; }
        Person Nextofkin { get; set; }  // or carer
        List<Condition> Alerts { get; set; }
        List<visit> Visits { get; set; }
        //List<Patient> patients { get; set; }   //Provider property
        List<Person> Dependants { get; set; }  //qnneepxxxx <Patient>
        public DateTime Nextvisit { get; set; }
        public DateTime Lastvisit { get; set; }
        public int Age => (DateTime.Today - Dateofbirth).Days / 365;


        public string Nextv => Nextvisit.ToString("dd MM yyyy");

        public Patient()
        {
            //name = "Tony M";
            //Gender = 'M';
            //address = "123";
            //Nextofkin.
            //Nextofkin.qnnneee = "1222333";         
        }

        public Patient(string healthid)
        {
            //check Provider is in same county
        }



        public void save_to_azure()
        {
            //limit to 64 kb prepend 3 bytes - 8bit version / 16bit length
        }

    }

}


