using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace MvvmLight1.Model
{
    public class Person
    {
        public string[] details = new string[8]; //name,address,cel,email,qnnneee                                                //[JsonIgnore]										 //public string qne_xxx { get { return details[0]; } set { value = details[0]; } }
        [JsonIgnore]
        public string name { get { return details[0]; } set { details[0] = value; } }
        [JsonIgnore]
        public string cell { get { return details[1]; } set { details[1] = value; } }
        [JsonIgnore]
        public string email { get { return details[2]; } set { details[2] = value; } }
        [JsonIgnore]
        public string qnnneee { get { return details[3]; } set { details[3] = value; } }
        [JsonIgnore]
        public string address1 { get { return details[4]; } set { details[4] = value; } }
        [JsonIgnore]
        public string address2 { get { return details[5]; } set { details[5] = value; } }
        [JsonIgnore]
        public string address3 { get { return details[6]; } set { details[6] = value; } }
        [JsonIgnore]
        public string postalcode { get { return details[7]; } set { details[7] = value; } }
    }


    public class med_condition
    {
        public string[] c = new string[3];

        [JsonIgnore]
        public string Name { get { return c[0]; } set { c[0] = value; } }
        [JsonIgnore]
        public string Icd10 { get { return c[1]; } set { c[1] = value; } }
        [JsonIgnore]
        public int Count { get { return Convert.ToInt16(c[2], 16); } set { c[2] = value.ToString(); } }
        //public condition() {}

        public med_condition(string name, string icd10)
        { Name = name; Icd10 = icd10; Count = 0; }
        //public condition() { count = 0; Name = ""; Icd10 = ""; }
    }


    public class visit
    { // ?? visit size will be less than 512 bytes
        public string date { get; set; } //get set to convert to dt	//string date {get;set;}
        public string referred { get; set; }
        public string description { get; set; } //top40 visits description choice
        public List<med_condition> condition { get; set; } //change <Condition>  to delim string
        public string prescription { get; set; } //top40 prescription choice
        public string advice { get; set; }
        public string nextVisit { get; set; } //get set to convert to dt
        public List<string> images { get; set; } //change to delim string name and image address qnnee/i...    
        int[] saveimage() { return null; } // saves to azure images and returns offset and length []
                                           //set the expiry months

        public void addate()
        {
            date = DateTime.Now.ToShortDateString();
            byte[] ba = new byte[100];
            //images.Add(ba);
        }

    }


    public class patient : Person
    {
        public string country { get; set; }
        public string patientId { get; set; }
        public string birthday { get; set; }
        public string sex { get; set; }
        public Person nextOfKin { get; set; }  // next of carer
        public List<med_condition> Alerts { get; set; }   //List<Condition> Alerts { get; set; }
        public List<visit> visits { get; set; } // top40 visits choice
        public List<Person> Dependants { get; set; }  //qnneepxxxx <Patient>
        public string nextVisit { get; set; }
        public string lastVisit { get; set; }
        public UInt16 riskflags { get; set; } //disability, poverty, chronic disease, neonate,age,
                                              //smoker,
        public byte version { get; set; }


        public patient() : base()
        {
            Alerts = new List<med_condition>();
            visits = new List<visit>();
            visit visit = new visit();
            visit.date = "";
            visit.description = "";
            med_condition mc = new med_condition("Cor Art Bypass", "C22.0");
            visit.condition = new List<med_condition>();
            visit.condition.Add(mc);
            visits.Add(visit);
        }

        public patient(string healthid) : base()
        {
            patientId = healthid;
            string[] login = patientId.Split('-');
            if (login.Length != 2) return;
        }

        public string save_to_azure()
        {
            MemoryStream ms = new MemoryStream();
            jsonutil.Serialize(this, ms);
            var ba = jsonutil.compress(ms.GetBuffer());
            var compressedlen = ba.Length;

            var l = BitConverter.GetBytes(ms.Length).AsEnumerable();
            l = l.Take(2);
            version++;
            var v = BitConverter.GetBytes(version);
            l = l.Concat(v);
            var buf = ms.GetBuffer();
            l = l.Concat(buf);
            return "";
            //healthid limit to 64 kb byte[] prepend 3 bytes - 8bit version / 16bit length
        }
    }


    public class Provider : Person
    {
        public string[] _prov = new String[4];
        [JsonIgnore]
        public string PinOffset { get { return _prov[0]; } set { _prov[0] = value; } }
        [JsonIgnore]
        public string RegAuthority { get { return _prov[1]; } set { _prov[1] = value; } }
        [JsonIgnore]
        public string Specialty { get { return _prov[2]; } set { _prov[2] = value; } }
        [JsonIgnore]
        public string Qualification { get { return _prov[3]; } set { _prov[3] = value; } }

        //public List<person> Contacts {get;set;}  -- might be better than obser collection
        public List<string> I_Top = new List<string>();  //top40 icds
        public List<string> V_Top = new List<string>();  //top40 visits
        public List<string> P_Top = new List<string>(); //top40 prescriptions
        public ObservableCollection<patient> Recent = new ObservableCollection<patient>();
        public ObservableCollection<Person> Contacts = new ObservableCollection<Person>();


        public Provider(string fromazure, string pin)
        {
            if (fromazure == null) return;
            var login = fromazure.Split('=');
            var offset = Convert.ToInt32(login[2], 16) << 22;
            byte[] compressed;
            var ba = new byte[4];

            var La = global.setcountryaccount(login[0]).//"ghza"
                    CreateCloudBlobClient().
                    GetContainerReference(login[1]).   //"22427" container
                    GetPageBlobReference("L").OpenRead();
            //if (La.Length > offset)
            {
                La.Seek(offset, SeekOrigin.Begin);
                La.Read(ba, 0, 4);
                //Array.Resize(ref ba,4);
                var size = BitConverter.ToInt32(ba, 0);
                //size = (size & 0xffffff00);
                compressed = new byte[size];
                La.Seek(offset + 4, SeekOrigin.Begin);
                La.Read(compressed, 0, size);
                var decomp = jsonutil.decompress(compressed);
                var ms = new MemoryStream(decomp);
                var p = jsonutil.Deserialize<Provider>(ms);

                this._prov = p._prov;
                this.address1 = p.address1;
                this.address2 = p.address2;
                this.address3 = p.address3;
                this.cell = p.cell;
                this.Contacts = p.Contacts;
                this.details = p.details;
                this.name = p.name;
                this.postalcode = p.postalcode;
                this.qnnneee = p.qnnneee;
                this.Qualification = p.Qualification;
                this.Specialty = p.Specialty;
                this.V_Top = p.V_Top;
                this.I_Top = p.I_Top;
                this.P_Top = p.P_Top;
                this.PinOffset = p.PinOffset;
                this.Recent = p.Recent;
                if (this.PinOffset != pin) this.PinOffset = null;
            }
        }


        //uploads provider
        public void UploadToAzure(string storagereference)
        {
            var login = storagereference.Split('=');
            var offset = Convert.ToInt32(login[2], 16) << 22;  // data byte offset in blob
            var La = global.setcountryaccount(login[0]). //"ghza" account
                        CreateCloudBlobClient().
                        GetContainerReference(login[1]). //"22427" container
                        GetPageBlobReference("L");
            using (var ms = new MemoryStream())
            {
                jsonutil.Serialize(this, ms);
                var len = ms.Length;
                var compressedbytes = jsonutil.compress(ms.ToArray());
                var len4 = BitConverter.GetBytes(len).Take(4);
                var final = len4.Concat(compressedbytes).ToArray();
                jsonutil.resize(ref final);
                using (var ms1 = new MemoryStream(final))
                    La.WritePages(ms1, offset);
            }
        }

    }













    //public class Provider : Person
    //{
    //    Provider() { }
    //    Provider(string csv) { }  //?use json
    //    //string name { get; set; }
    //    //string Gender { get; set; }
    //    //public string qnnneee { get; set; }
    //    string RegAuthority { get; set; }
    //    string RegNumber { get; set; }
    //    string Specialty { get; set; }
    //    string Qualification { get; set; }
    //    string[] Commonicds { get; set; }  //top40
    //    string[] Commonvisits { get; set; }  //top40
    //    string[] Commonprescripions { get; set; } //top40
    //    private readonly List<Patient> _recentpatients = new List<Patient>();//was observablecollection
    //    string Tocsv() { return ""; }  //?tojson

    //    byte[] ToUtf8Data() { return null; } //put this in shared library

    //    //gets patient from azure storage and saves it locally to recentpatient list

    //    bool Getpatient(string qnexxx)
    //    {
    //        var p = new Patient
    //        {
    //            name = "Tony Manicom",
    //            Dateofbirth = new DateTime(1948, 7, 8),
    //            Gender = 'M'
    //        };
    //        _recentpatients.Add(p);
    //        //var p1 = _recentpatients[0];
    //        //_recentpatients.
    //        //check Patient is in same county as Provider
    //        //and check if Patient exists
    //        //Patient p = Getpatient
    //        //_recentpatients.Add(new Patient());
    //        return true;
    //    }

    //    //looks up Id number cross reference to azure qnneexxxx to load Patient
    //    private bool getpatient_id(string countryid)
    //    {
    //        return true;
    //    }  //or gets qnneexxx lookup if passes country check


    //    // opens patreg dialog
    //    // includes country id for access as well
    //    bool Registerpatient()
    //    {
    //        return true;
    //    }

    //    //removes the oldest accessed 
    //    void Trimrecentpatients()
    //    {

    //    }


    //    //checks and updates changes by provide
    //    void Updatepatient(int ndx)
    //    {

    //    }
    //}






    //public class Person
    //{
    //   // private string _name;
    //    public string name { get; set; }
    //    public string address { get; set; }
    //    public string cellphone { get; set; }
    //    public string email { get; set; }
    //    public string qnnneee { get; set; }

    //public void remove_delim(ref string str)
    //{
    //    if ( str != null )
    //    str.Replace('|', ';');
    //}


    //public override string ToString()
    //{
    //    string[] sarr = new string[5];
    //    sarr[0] = name;
    //    sarr[1] = address;
    //    sarr[2] = cellphone;
    //    sarr[3] = email;
    //    sarr[4] = qnnneee;
    //    for (int i = 0; i < 5; i++) { remove_delim(ref sarr[i]); }
    //    return string.Format("{0}|{1}|{2}|{3}|{4}",sarr[0],sarr[1],sarr[2],sarr[3],sarr[4]);
    //}

}


