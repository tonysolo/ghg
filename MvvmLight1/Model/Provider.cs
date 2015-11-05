using System.Collections.ObjectModel;


namespace MvvmLight1.Model
{

    //need static utility class to store patients providers and images to azure
    //providers are clinical providers - loaders include clerical to register patients
    //Provider app will list all their current patients next appointments and options to check for updates by other providers
    //providers get 4 megabytes storage so they can store a local copy of their recent patients and other important data

    //data layout:

    // 0 name,address,cell,email,qnnneee,regauth,regnum,specialty,qualif
    // 1  commICDs ,,,
    // 2  commVisits ,,,
    // 3  commVisits presciptions ,,,
    // 4  recentPatients qnneexxxx plus data ,,,


    public class Provider : person
    {
        Provider() { }
        Provider(string csv) { }  //?use json
        //string name { get; set; }
        //string Gender { get; set; }
        //public string qnnneee { get; set; }
        string RegAuthority { get; set; }
        string RegNumber { get; set; }
        string Specialty { get; set; }
        string Qualification { get; set; }
        string[] Commonicds { get; set; }  //top40
        string[] Commonvisits { get; set; }  //top40
        string[] Commonprescripions { get; set; } //top40
        ObservableCollection<Patient> Recentpatients { get; set; }
        string Tocsv() { return ""; }  //?tojson

        byte[] ToUtf8Data() { return null; } //put this in shared library

        bool Getpatient(string qnexxx) //? combine with getpatient_sa_id by checking if starts with valid qnnee
        {
            Patient p = new Patient();
            Recentpatients.Add(p);
            //var p1 = Recentpatients[0];
            //Recentpatients.
            //check Patient is in same county as Provider
            //and check if Patient exists
            //Patient p = Getpatient
            Recentpatients.Add(new Patient());
            return true;
        }

        private bool getpatient_sa_id(string sa_id)//looks up sa Id number cross reference to qnneexxxx to load Patient 
        { return true; }

        bool Registerpatient()
        {  // opens patreg dialog
           //must use saID to register that as well

            return true;
        }

        void Trimrecentpatients() //removes the oldest accessed 
        { }

        void Updatepatient(int ndx) { }     //checks and updates changes by provides 


    }
}
