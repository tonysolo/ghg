using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Newtonsoft.Json;

namespace MvvmLight1.Model
{
    //this is the primary class to store and retrieve providers ans all their data - 
    //recent patients
    //storing patients details and their images, 
    //epidemiology etc
    public static class Global
    {
        static string Accountname { get; set; } //ghg 
        static CloudStorageAccount SelectedAccount { get; set; }

        public static CloudStorageAccount Setcountryaccount(string accname)
        {
            if (accname == Accountname) return SelectedAccount;
            Accountname = accname;
            var accountName = "ghg";
            var accountKey =
                "38Y8V0konokJ4aNWUJMzKJFrzKPh1t2uLqQRABXA3/oLy0EXPxmApIDJYuiD2gF8sPyH0J2skG/0i1V3GhxMtQ==";
            StorageCredentials creds = new StorageCredentials(accountName, accountKey);
            CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
            var acc = account.CreateCloudBlobClient();
            var c = acc.GetContainerReference("countries");
            var d = c.GetPageBlobReference("global");
            d.FetchAttributes();
            var key = d.Metadata[accname];
            StorageCredentials credentials = new StorageCredentials(accname, key);
            SelectedAccount = new CloudStorageAccount(credentials, useHttps: true);
            return SelectedAccount;
            //AccountName = accname;
        }

        //public static Task<CloudStorageAccount> setcc(string accname)
        //{
        //    if (accname == Accountname) return SelectedAccount;
        //    Accountname = accname;
        //    var accountName = "ghg";
        //    var accountKey =
        //        "38Y8V0konokJ4aNWUJMzKJFrzKPh1t2uLqQRABXA3/oLy0EXPxmApIDJYuiD2gF8sPyH0J2skG/0i1V3GhxMtQ==";
        //    StorageCredentials creds = new StorageCredentials(accountName, accountKey);
        //    var acc = new CloudStorageAccount(creds, useHttps: true);
        //    Task<CloudStorageAccount> csa = StorageCredentials(StorageCredentials,bool);
        //    //  Task<CloudStorageAccount> account = new Task<CloudStorageAccount>(acc);
        //    return account;
        //}

        private static void UploadProviderToAzure(Provider provider, string countryRegionIndex)
        {
            var login = countryRegionIndex.Split('=');
            var offset = Convert.ToInt32(login[2], 16) << 22;
            var la = Setcountryaccount(login[0]). //"ghza"
                CreateCloudBlobClient().
                GetContainerReference(login[1]). //"22427" container
                GetPageBlobReference("L");
            using (var ms = new MemoryStream())
            {
                Compression.Serialize(provider, ms);
                //var len = ms.Length;
                var compressedbytes = Compression.Compress(ms.ToArray());
                var len = BitConverter.GetBytes((int) compressedbytes.Length);
                //var len4 = BitConverter.GetBytes(len).Take(4);
                var final = len.Concat(compressedbytes).ToArray();
                Compression.Resize(ref final);
                using (var ms1 = new MemoryStream(final))
                    la.WritePages(ms1, offset);
            }
        }

        //Register methods return a unique address to use for storage
        //returns a new address - "account=qnnee=offset" 
        private static string Registernewprovider(string account, string region)
        {
            var l = Setcountryaccount(account). //"ghza"
                CreateCloudBlobClient().
                GetContainerReference(region). //qnnee eg "22427"
                GetPageBlobReference("L");
            l.FetchAttributes();

            var val = l.Metadata["Next"];
            var tag = l.Properties.ETag;
            var x = (Convert.ToInt32(val, 16) + 1).ToString("x");

            l.Metadata["Next"] = x; //(x + 1).ToString("x");
            l.SetMetadata(AccessCondition.GenerateIfMatchCondition(tag));
            return account + '=' + region + '=' + x;
        }

        //returns a new address - "account-qnnee-offset"
        private static string Registernewpatient(string account, string region)
        {
            var p = Setcountryaccount(account). //"ghza"
                CreateCloudBlobClient().
                GetContainerReference(region). //qnnee eg "22427"
                GetPageBlobReference("P");
            p.FetchAttributes();

            var val = p.Metadata["Next"];
            var tag = p.Properties.ETag;
            var x = (Convert.ToInt32(val, 16) + 1).ToString("x");

            p.Metadata["Next"] = x; //(x + 1).ToString("x");
            p.SetMetadata(AccessCondition.GenerateIfMatchCondition(tag));
            return account + '-' + region + '-' + x;
        }

        //returns a new address - "account|qnnee|blobsize|offset" eg ghza|32222|5|20
        private static string Registernewimage(string account, string region, int imagesize)
        {
            var s =
                (imagesize < (1 << 11))
                    ? '0'
                    : (imagesize < (1 << 12))
                        ? '1'
                        : (imagesize < (1 << 13))
                            ? '2'
                            : (imagesize < (1 << 14))
                                ? '3'
                                : (imagesize < (1 << 15))
                                    ? '4'
                                    : (imagesize < (1 << 16))
                                        ? '5'
                                        : (imagesize < (1 << 17))
                                            ? '6'
                                            : (imagesize < (1 << 18))
                                                ? '7'
                                                : (imagesize < (1 << 19))
                                                    ? '8'
                                                    : (imagesize < (1 << 20))
                                                        ? '9'
                                                        : (imagesize < (1 << 21))
                                                            ? 'a'
                                                            : (imagesize < (1 << 22)) ? 'b' : 'x';
            var blobname = "I" + s;

            var I = Setcountryaccount(account). //"ghza"
                CreateCloudBlobClient().
                GetContainerReference(region). //qnnee eg "22427"
                GetPageBlobReference(blobname);
            I.FetchAttributes();

            var val = I.Metadata["Next"];
            //var tag = I.Properties.ETag;
            var x = (Convert.ToInt32(val, 16) + 1).ToString("x");
            I.Metadata["Next"] = x; //(x + 1).ToString("x");
            I.SetMetadata();
            //I.SetMetadata(AccessCondition.GenerateIfMatchCondition(tag));
            return account + '|' + region + '|' + blobname + '|' + x;
        }
    }


    public class MedCondition
    {
        public string[] C = new string[3];

        [JsonIgnore]
        public string Name
        {
            get { return C[0]; }
            set { C[0] = value; }
        }

        [JsonIgnore]
        public string Icd10
        {
            get { return C[1]; }
            set { C[1] = value; }
        }

        [JsonIgnore]
        public int Count
        {
            get { return Convert.ToInt16(C[2], 16); }
            set { C[2] = value.ToString(); }
        }

        //public condition() {}

        public MedCondition(string name, string icd10)
        {
            Name = name;
            Icd10 = icd10;
            Count = 0;
        }

        //public condition() { count = 0; Name = ""; Icd10 = ""; }
    }

    public class Person
    {

        public string[] Details = new string[8]; //name,address,cel,email,qnnneee
        //[JsonIgnore]										 //public string qne_xxx { get { return details[0]; } set { value = details[0]; } }
        [JsonIgnore]
        public string Name
        {
            get { return Details[0]; }
            set { Details[0] = value; }
        }

        [JsonIgnore]
        public string Cell
        {
            get { return Details[1]; }
            set { Details[1] = value; }
        }

        [JsonIgnore]
        public string Email
        {
            get { return Details[2]; }
            set { Details[2] = value; }
        }

        [JsonIgnore]
        public string Qnnneee
        {
            get { return Details[3]; }
            set { Details[3] = value; }
        }

        [JsonIgnore]
        public string Address1
        {
            get { return Details[4]; }
            set { Details[4] = value; }
        }

        [JsonIgnore]
        public string Address2
        {
            get { return Details[5]; }
            set { Details[5] = value; }
        }

        [JsonIgnore]
        public string Address3
        {
            get { return Details[6]; }
            set { Details[6] = value; }
        }

        [JsonIgnore]
        public string Postalcode
        {
            get { return Details[7]; }
            set { Details[7] = value; }
        }
    }

    //todo replace Prov and other public storage arrays with single / short names Pr provider Pt patient 
    //todo similarly make all ofter data more compact - it will be machine read
    public class Provider : Person
    {
        public string[] Prov = new string[4];

        [JsonIgnore]
        public string PinOffset
        {
            get { return Prov[0]; }
            set { Prov[0] = value; }
        }

        [JsonIgnore]
        public string RegAuthority
        {
            get { return Prov[1]; }
            set { Prov[1] = value; }
        }

        [JsonIgnore]
        public string Specialty
        {
            get { return Prov[2]; }
            set { Prov[2] = value; }
        }

        [JsonIgnore]
        public string Qualification
        {
            get { return Prov[3]; }
            set { Prov[3] = value; }
        }

        //public string[] Prov => _prov;
        //public List<person> Contacts {get;set;}  -- might be better than obser collection
        public List<string> I_Top = new List<string>(); //top40 icds
        public List<string> V_Top = new List<string>(); //top40 visits
        public List<string> P_Top = new List<string>(); //top40 prescriptions
        public ObservableCollection<Patient> Recent = new ObservableCollection<Patient>();
        public ObservableCollection<Person> Contacts = new ObservableCollection<Person>();

        //static void Readlength(AsyncCallback ac)
        //{

        //}



        /// <summary>
        /// reads provider from azure 
        /// format fromazure = account=container=index
        /// pin = provider login auth (not used yet) 
        /// </summary>
        /// <param name="fromazure"></param>
        /// <param name="pin"></param>
        public Provider(string fromazure, string pin) //still need to check pin for auth
        {

            if (fromazure == null) return;
            var login = fromazure.Split('=');
            var offset = Convert.ToInt32(login[2], 16) << 22;


            var blob = Global.Setcountryaccount(login[0]). //"ghza"
                CreateCloudBlobClient().
                GetContainerReference(login[1]). //"22427" container
                GetPageBlobReference("L");
            blob.OpenRead();

            var bytes = new byte[4];
            blob.DownloadRangeToByteArray(bytes, 0, offset, 4); //BitConverter.ToInt32(ba, 0);


            var length = BitConverter.ToInt32(bytes, 0);
            var compressed = new byte[length];

            blob.DownloadRangeToByteArray(compressed, 0, offset + 4, compressed.Length); //provider blob


            var prov = Compression.Decompress(compressed);

            using (var ms = new MemoryStream(prov))
            { 
            var p = Compression.Deserialize<Provider>(ms);
                
                Prov = p.Prov;
                Address1 = p.Address1;
                Address2 = p.Address2;
                Address3 = p.Address3;
                Cell = p.Cell;
                Contacts = p.Contacts;
                Details = p.Details;
                Name = p.Name;
                Postalcode = p.Postalcode;
                Qnnneee = p.Qnnneee;
                Qualification = p.Qualification;
                Specialty = p.Specialty;
                
                V_Top = p.V_Top;
                P_Top = p.P_Top;
                PinOffset = p.PinOffset;
                Recent = p.Recent;

                this.Recent = p.Recent;
                
                if (this.PinOffset != pin) this.PinOffset = null;
            }
        }

        //private void Readlength(IAsyncResult ar)
        //{
        //    throw new NotImplementedException();
        //}
    }

    //uploads provider
    //    public void UploadToAzure(string storagereference)
    //    {
    //        var login = storagereference.Split('=');
    //        var offset = Convert.ToInt32(login[2], 16) << 22; // data byte offset in blob
    //        var la = Global.Setcountryaccount(login[0]). //"ghza" account
    //            CreateCloudBlobClient().
    //            GetContainerReference(login[1]). //"22427" container
    //            GetPageBlobReference("L");
    //        using (var ms = new MemoryStream())
    //        {
    //            Compression.Serialize(this, ms);
    //            var len = ms.Length;
    //            var compressedbytes = Compression.Compress(ms.ToArray());
    //            var len4 = BitConverter.GetBytes(len).Take(4);
    //            var final = len4.Concat(compressedbytes).ToArray();
    //            Compression.Resize(ref final);
    //            using (var ms1 = new MemoryStream(final))
    //                la.WritePages(ms1, offset);
    //        }
    //    }
    //}



    public class Visit
    {
        // ?? visit size will be less than 512 bytes
        public string Date { get; set; } //get set to convert to dt	//string date {get;set;}
        public string Referred { get; set; }
        public string Description { get; set; } //top40 visits description choice
        public List<MedCondition> Condition { get; set; } //change <Condition>  to delim string
        public string Prescription { get; set; } //top40 prescription choice
        public string Advice { get; set; }
        public string NextVisit { get; set; } //get set to convert to dt
        public List<string> Images { get; set; } //change to delim string name and image address qnnee/i...    

        int[] Saveimage()
        {
            return null;
        } // saves to azure images and returns offset and length []
        //set the expiry months

        public void Addate()
        {
            Date = DateTime.Now.ToShortDateString();
            var ba = new byte[100];
            //images.Add(ba);
        }
    }


    public class Patient : Person
    {
       
        public string Country { get; set; }
        private string PatientId { get; set; }
        public string Birthday { get; set; }
        public string Sex { get; set; }
        public Person NextOfKin { get; set; } // next of carer
        public List<MedCondition> Alerts { get; set; } //List<Condition> Alerts { get; set; }
        public List<Visit> Visits { get; set; } // top40 visits choice
        public List<Person> Dependants { get; set; } //qnneepxxxx <Patient>
        public string NextVisit { get; set; }
        public string LastVisit { get; set; }
        public ushort Riskflags { get; set; } //disability, poverty, chronic disease, neonate,age,
        //smoker,
        public byte Version { get; set; }
        //patientid wont be saved to azure - it will be used to generate the 'account/region/page blob' storage address
        //ie temporary stored in patient data during editing and persisted in the blob name.
        public Patient() : base()
        {
            Alerts = new List<MedCondition>();
            Visits = new List<Visit>();
            var visit = new Visit
            {
                Date = "",
                Description = ""
            };
            var mc = new MedCondition("Cor Art Bypass", "C22.0");
            visit.Condition = new List<MedCondition> {mc};
            Visits.Add(visit);
        }

        public Patient(string healthid) : base()
        {
            PatientId = healthid;
            string[] login = PatientId.Split('-');
            if (login.Length != 2) return;
        }

        public void save_to_azure(string healthid)
        {
            var ms = new MemoryStream();
            Compression.Serialize(this, ms);
            var ba = Compression.Compress(ms.GetBuffer());
            var compressedlen = ba.Length;
            var l = BitConverter.GetBytes(ms.Length).AsEnumerable();
            l = l.Take(2);
            Version++;
            var v = BitConverter.GetBytes(Version);

            l = l.Take(2).Concat(v.Take(1)).Concat(ba);
            var buf = ms.GetBuffer();
            var fin = l.Concat(buf).ToArray();
            Compression.Resize(ref fin);
            //save to azure ********

            //length 2bytes, version 1byte, compressed data -   expanded modulo 512
        }

    }
}
    


