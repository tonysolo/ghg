using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Newtonsoft.Json;

static class Jsonutil
{
    public static void Serialize(object value, Stream s)
    {
        StreamWriter writer = new StreamWriter(s);
        JsonTextWriter jsonWriter = new JsonTextWriter(writer);
        JsonSerializer ser = new JsonSerializer();
        ser.Serialize(jsonWriter, value);
        jsonWriter.Flush();
    }

    public static T Deserialize<T>(Stream s)
    {
        StreamReader reader = new StreamReader(s);
        JsonTextReader jsonReader = new JsonTextReader(reader);
        JsonSerializer ser = new JsonSerializer();
        return ser.Deserialize<T>(jsonReader);
    }

    public static byte[] Compress(byte[] data)
    {
        MemoryStream outStream = new MemoryStream();     
        using (GZipStream gzipStream = new GZipStream(outStream, CompressionMode.Compress))
        using (MemoryStream srcStream = new MemoryStream(data))
        { 
            srcStream.CopyTo(gzipStream); 
            return outStream.ToArray();
        }
    }
    

    public static byte[] Decompress(byte[] compressed)
    {
        MemoryStream inStream = new MemoryStream(compressed);
        using (GZipStream gzipStream = new GZipStream(inStream, CompressionMode.Decompress))
        using (MemoryStream outStream = new MemoryStream())
        {
            gzipStream.CopyTo(outStream);
            return outStream.ToArray();
        }
    }


    public static void Resize(ref byte[] b)
    {
        var s = b.Length;
        var adjust = 512 - (s % 512);
        Array.Resize(ref b, s + adjust);
    }

}


static public class Global
{
    static string Accountname { get; set; } //ghg 
    static CloudStorageAccount SelectedAccount { get; set; }

    static public CloudStorageAccount Setcountryaccount(string accname)
    {
        if (accname == Accountname) return SelectedAccount;
        Accountname = accname;
        string accountName = "ghg";
        string accountKey = "38Y8V0konokJ4aNWUJMzKJFrzKPh1t2uLqQRABXA3/oLy0EXPxmApIDJYuiD2gF8sPyH0J2skG/0i1V3GhxMtQ==";
        StorageCredentials creds = new StorageCredentials(accountName, accountKey);
        CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
        var acc = account.CreateCloudBlobClient();
        var c = acc.GetContainerReference("countries");
        var d = c.GetPageBlobReference("global");
        d.FetchAttributes();
        string key = d.Metadata[accname];
        StorageCredentials credentials = new StorageCredentials(accname, key);
        SelectedAccount = new CloudStorageAccount(credentials, useHttps: true);
        return SelectedAccount;
        //AccountName = accname;
    }



    static void UploadProviderToAzure(Provider provider, string countryRegionIndex)
    {
        var login = countryRegionIndex.Split('=');
        var offset = Convert.ToInt32(login[2], 16) << 22;
        var la = Global.Setcountryaccount(login[0]).//"ghza"
                    CreateCloudBlobClient().
                    GetContainerReference(login[1]).   //"22427" container
                    GetPageBlobReference("L");
        using (var ms = new MemoryStream())
        {
            Jsonutil.Serialize(provider, ms);
            //var len = ms.Length;
            var compressedbytes = Jsonutil.Compress(ms.ToArray());
            var len = BitConverter.GetBytes(compressedbytes.Length);
            //var len4 = BitConverter.GetBytes(len).Take(4);
            var final = len.Concat(compressedbytes).ToArray();
            Jsonutil.Resize(ref final);
            using (var ms1 = new MemoryStream(final))
                la.WritePages(ms1, offset);
        }
    }

    static string Registernewprovider(string account, string region)
    {
        var l = Global.Setcountryaccount(account).//"ghza"
        CreateCloudBlobClient().
        GetContainerReference(region).//qnnee eg "22427"
        GetPageBlobReference("L");
        l.FetchAttributes();

        var val = l.Metadata["Next"];
        var tag = l.Properties.ETag;
        var x = (Convert.ToInt32(val, 16) + 1).ToString("x");

        l.Metadata["Next"] = x; //(x + 1).ToString("x");
        l.SetMetadata(AccessCondition.GenerateIfMatchCondition(tag));
        return account + '=' + region + '=' + x;
    }

    static string Registernewpatient(string account, string region)
    {
        var p = Global.Setcountryaccount(account).//"ghza"
        CreateCloudBlobClient().
        GetContainerReference(region).//qnnee eg "22427"
        GetPageBlobReference("P");
        p.FetchAttributes();

        var val = p.Metadata["Next"];
        var tag = p.Properties.ETag;
        var x = (Convert.ToInt32(val, 16) + 1).ToString("x");

        p.Metadata["Next"] = x; //(x + 1).ToString("x");
        p.SetMetadata(AccessCondition.GenerateIfMatchCondition(tag));
        return account + '-' + region + '-' + x;
    }

    //
   static string Registernewimage(string account, string region, int imagesize)
    {
        char s =
        (imagesize < (1 << 11)) ? '0' :
        (imagesize < (1 << 12)) ? '1' :
        (imagesize < (1 << 13)) ? '2' :
        (imagesize < (1 << 14)) ? '3' :
        (imagesize < (1 << 15)) ? '4' :
        (imagesize < (1 << 16)) ? '5' :
        (imagesize < (1 << 17)) ? '6' :
        (imagesize < (1 << 18)) ? '7' :
        (imagesize < (1 << 19)) ? '8' :
        (imagesize < (1 << 20)) ? '9' :
        (imagesize < (1 << 21)) ? 'a' :
        (imagesize < (1 << 22)) ? 'b' : 'x';
        string blobname = "I" + s;

        var I = Global.Setcountryaccount(account).//"ghza"
        CreateCloudBlobClient().
        GetContainerReference(region).//qnnee eg "22427"
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
    public string Name { get { return C[0]; } set { C[0] = value; } }
    [JsonIgnore]
    public string Icd10 { get { return C[1]; } set { C[1] = value; } }
    [JsonIgnore]
    public int Count { get { return Convert.ToInt16(C[2], 16); } set { C[2] = value.ToString(); } }
    //public condition() {}

    public MedCondition(string name, string icd10)
    { Name = name; Icd10 = icd10; Count = 0; }
    //public condition() { count = 0; Name = ""; Icd10 = ""; }
}

public class Person
{

    public string[] Details = new string[8]; //name,address,cel,email,qnnneee
                                             //[JsonIgnore]										 //public string qne_xxx { get { return details[0]; } set { value = details[0]; } }
    [JsonIgnore]
    public string Name { get { return Details[0]; } set { Details[0] = value; } }
    [JsonIgnore]
    public string Cell { get { return Details[1]; } set { Details[1] = value; } }
    [JsonIgnore]
    public string Email { get { return Details[2]; } set { Details[2] = value; } }
    [JsonIgnore]
    public string Qnnneee { get { return Details[3]; } set { Details[3] = value; } }
    [JsonIgnore]
    public string Address1 { get { return Details[4]; } set { Details[4] = value; } }
    [JsonIgnore]
    public string Address2 { get { return Details[5]; } set { Details[5] = value; } }
    [JsonIgnore]
    public string Address3 { get { return Details[6]; } set { Details[6] = value; } }
    [JsonIgnore]
    public string Postalcode { get { return Details[7]; } set { Details[7] = value; } }
}

public class Provider : Person
{
    public string[] Prov = new String[4];
    [JsonIgnore]
    public string PinOffset { get { return Prov[0]; } set { Prov[0] = value; } }
    [JsonIgnore]
    public string RegAuthority { get { return Prov[1]; } set { Prov[1] = value; } }
    [JsonIgnore]
    public string Specialty { get { return Prov[2]; } set { Prov[2] = value; } }
    [JsonIgnore]
    public string Qualification { get { return Prov[3]; } set { Prov[3] = value; } }

    //public List<person> Contacts {get;set;}  -- might be better than obser collection
    public List<string> Top = new List<string>();  //top40 icds
    public List<string> VTop = new List<string>();  //top40 visits
    public List<string> PTop = new List<string>(); //top40 prescriptions
    public ObservableCollection<Patient> Recent = new ObservableCollection<Patient>();
    public ObservableCollection<Person> Contacts = new ObservableCollection<Person>();


    public Provider(string fromazure, string pin)
    {
        if (fromazure == null) return;
        var login = fromazure.Split('=');
        var offset = Convert.ToInt32(login[2], 16) << 22;
        byte[] compressed;
        var ba = new byte[4];

        var la = Global.Setcountryaccount(login[0]).//"ghza"
                CreateCloudBlobClient().
                GetContainerReference(login[1]).   //"22427" container
                GetPageBlobReference("L").OpenRead();
        //if (La.Length > offset)
        {
            la.Seek(offset, SeekOrigin.Begin);
            la.Read(ba, 0, 4);
            //Array.Resize(ref ba,4);
            var size = BitConverter.ToInt32(ba, 0);
            //size = (size & 0xffffff00);
            compressed = new byte[size];
            la.Seek(offset + 4, SeekOrigin.Begin);
            la.Read(compressed, 0, size);
            var decomp = Jsonutil.Decompress(compressed);
            var ms = new MemoryStream(decomp);
            var p = Jsonutil.Deserialize<Provider>(ms);

            this.Prov = p.Prov;
            this.Address1 = p.Address1;
            this.Address2 = p.Address2;
            this.Address3 = p.Address3;
            this.Cell = p.Cell;
            this.Contacts = p.Contacts;
            this.Details = p.Details;
            this.Name = p.Name;
            this.Postalcode = p.Postalcode;
            this.Qnnneee = p.Qnnneee;
            this.Qualification = p.Qualification;
            this.Specialty = p.Specialty;
            this.VTop = p.VTop;
            this.Top = p.Top;
            this.PTop = p.PTop;
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
        var la = Global.Setcountryaccount(login[0]). //"ghza" account
                    CreateCloudBlobClient().
                    GetContainerReference(login[1]). //"22427" container
                    GetPageBlobReference("L");
        using (var ms = new MemoryStream())
        {
            Jsonutil.Serialize(this, ms);
            var len = ms.Length;
            var compressedbytes = Jsonutil.Compress(ms.ToArray());
            var len4 = BitConverter.GetBytes(len).Take(4);
            var final = len4.Concat(compressedbytes).ToArray();
            Jsonutil.Resize(ref final);
            using (var ms1 = new MemoryStream(final))
                la.WritePages(ms1, offset);
        }
    }

}



public class Visit
{ // ?? visit size will be less than 512 bytes
    public string Date { get; set; } //get set to convert to dt	//string date {get;set;}
    public string Referred { get; set; }
    public string Description { get; set; } //top40 visits description choice
    public List<MedCondition> Condition { get; set; } //change <Condition>  to delim string
    public string Prescription { get; set; } //top40 prescription choice
    public string Advice { get; set; }
    public string NextVisit { get; set; } //get set to convert to dt
    public List<string> Images { get; set; } //change to delim string name and image address qnnee/i...    
    int[] Saveimage() { return null; } // saves to azure images and returns offset and length []
                                       //set the expiry months

    public void Addate()
    {
        Date = DateTime.Now.ToShortDateString();
        byte[] ba = new byte[100];
        //images.Add(ba);
    }

}


public class Patient : Person
{
    public string Country { get; set; }
    public string PatientId { get; set; }
    public string Birthday { get; set; }
    public string Sex { get; set; }
    public Person NextOfKin { get; set; }  // next of carer
    public List<MedCondition> Alerts { get; set; }   //List<Condition> Alerts { get; set; }
    public List<Visit> Visits { get; set; } // top40 visits choice
    public List<Person> Dependants { get; set; }  //qnneepxxxx <Patient>
    public string NextVisit { get; set; }
    public string LastVisit { get; set; }
    public UInt16 Riskflags { get; set; } //disability, poverty, chronic disease, neonate,age,
                                          //smoker,
    public byte Version { get; set; }


    public Patient() : base()
    {
        Alerts = new List<MedCondition>();
        Visits = new List<Visit>();
        Visit visit = new Visit();
        visit.Date = "";
        visit.Description = "";
        MedCondition mc = new MedCondition("Cor Art Bypass", "C22.0");
        visit.Condition = new List<MedCondition>();
        visit.Condition.Add(mc);
        Visits.Add(visit);
    }

    public Patient(string healthid) : base()
    {
        PatientId = healthid;
        string[] login = PatientId.Split('-');
        if (login.Length != 2) return;
    }

    public string save_to_azure()
    {
        MemoryStream ms = new MemoryStream();
        Jsonutil.Serialize(this, ms);
        var ba = Jsonutil.Compress(ms.GetBuffer());
        var compressedlen = ba.Length;

        var l = BitConverter.GetBytes(ms.Length).AsEnumerable();
        l = l.Take(2);
        Version++;
        var v = BitConverter.GetBytes(Version);
        l = l.Concat(v);
        var buf = ms.GetBuffer();
        l = l.Concat(buf);
        return "";
        //healthid limit to 64 kb byte[] prepend 3 bytes - 8bit version / 16bit length
    }
}



//void Main()
//{
 //   Provider p = new Provider("ghza=22427=1", "2");
    //var azure_prov_reg = registernewprovider("ghza", "22427");
    //	string s = "ghza=22427=1";
    //	var provider = new Provider(s, "2"); //2 is pin
    //	provider.V_Top.Add("TonyTest");
    //	provider.name = "Tony";
    //	provider.address1 = "173 Blandford Rd, , ";
    //	provider.cell = "0824102620";
    //	provider.email = "tony@turbomed.co.za";
    //	provider.address2 = "North Riding";
    //	provider.address3 = "Randburg";
    //	provider.postalcode = "2040";
    //	provider.qnnneee = "3123456";
    //	provider.RegAuthority = "HPCSA";
    //	provider.Specialty = "Anaestrhetics";
    //	provider.Qualification = "MBCHBFFA";
    //	provider.V_Top.Add("Epidural");
    //	provider.V_Top.Add("Spinal");
    //provider.

    //p.NextV = DateTime.Today.ToShortDateString();

    //	patient _patient = new patient();
    //	_patient.name = "Tony Manicom";
    //	_patient.birthday = new DateTime(1948, 7, 8).ToShortDateString();
    //	_patient.address1 = "Waterford Estate";
    //	_patient.address2 = "fourways";
    //	_patient.address3 = "2040";
    //
    //	//_patient.visits = new List<visit>();//? obserseable collection
    //	//_patient.em
    //	//_patient.visits.Add(value);
    //	visit v = new visit();
    //	v.date = DateTime.Today.ToShortDateString();
    //	v.advice = "do nothing";
    //	v.description = "Nothing";
    //	v.prescription = "Panado";
    //	v.date = DateTime.Today.ToShortDateString();
    //	
    //	_patient.visits.Add(v);
    //	
    //	for (int i = 0; i < 5; i++)
    //		provider.Recent.Add(_patient);
    //
    //	//var reg = registernewprovider("ghza", "22427");
    //
    //	UploadProviderToAzure(provider, s);//string country_region_index)


//provider.UploadToAzure(s);

//Provider prov = new Provider(s,"0");
//prov.D

//MemoryStream ms = new MemoryStream();
//jsonutil.Serialize(provider, ms);

//var compressed = jsonutil.compress(ms.GetBuffer()); //compressed byte[]												//Provider p = jsonutil.Deserialize<Provider>(ms2);
//var l = BitConverter.GetBytes(compressed.Length);
//l.Concat(compressed);


//byte[] PrependLength_and_Version(byte[] ba, byte version)
//{

//var l = BitConverter.GetBytes(ba.Length);
//var list = l.ToList();
//list[3] = version;

//l.Take(2).Concat((IEnumerable)version);
//l[3] = version;
//return l.Concat(ba).ToArray();
//

//	byte[] withlength = PrependLength(compressed); //32bit unit32 used for providers and images

//
//	byte[] withversion = PrependLength_and_Version(compressed, 10); //16bit unit16 used for patients

//	Int16 length = BitConverter.ToInt16(withversion, 0);

//	var L = global.setcountryaccount("ghza").
//	CreateCloudBlobClient().
//	GetContainerReference("22427").
//	GetPageBlobReference("L");
//	L.FetchAttributes();
//
//	var val = L.Metadata["Next"];
//	var tag = L.Properties.ETag;
//	long x = Convert.ToInt32(val, 16);
//	L.Metadata["Next"] = (x + 1).ToString("x");
//	L.SetMetadata(AccessCondition.GenerateIfMatchCondition(tag));
//	//x = 0;
//	x = x << 16;
//	var buf = ms.GetBuffer();
//	var buf1 = PrependLength(ms.GetBuffer());
//	var len = BitConverter.ToInt32(buf1, 0);
//	var before = compressed.Length;
//
//	before = (255 << 16) | before;
//	var ba = BitConverter.GetBytes(before);
//	jsonutil.resize(ref compressed);
//	MemoryStream ms1 = new MemoryStream(compressed);
//	L.WritePages(ms1, x);
//	//L.UploadFromByteArray(compressed,x,compressed.Length);
//	//L.UploadFromByteArray(ms.GetBuffer(),0,512);
//
//
//	ms = new MemoryStream();
//	jsonutil.Serialize(L, ms);
//
//	compressed = jsonutil.compress(ms.GetBuffer());
//	var La = global.setcountryaccount("ghza").
//		CreateCloudBlobClient().
//		GetContainerReference("22427").
//		GetPageBlobReference("L");
//	La.FetchAttributes();
//
//	var va = La.Metadata["Next"];
//	var tg = La.Properties.ETag;
//	long x1 = Convert.ToInt32(val, 16);
//
//	La.Metadata["Next"] = (x1 + 1).ToString("x");
//	La.SetMetadata(AccessCondition.GenerateIfMatchCondition(tg));
//	//if (La.Metadata["Next"] == // ? need to think this thru
//	//x = 0;
//	x1 = x1 << 16;
//	var buff = ms.GetBuffer();
//
//	var before1 = compressed.Length;
//
//	before1 = (255 << 16) | before1;
//	var ba1 = BitConverter.GetBytes(before1);
//	jsonutil.resize(ref compressed);
//	ms = new MemoryStream(compressed);
//	//var ms2 = new MemoryStream();
//	La.WritePages(ms, x);
//	var ms2 = La.OpenRead();
//	byte[] ba2 = new byte[512];
//	ms2.Read(ba2, 0, 512);
//	var ba3 = jsonutil.decompress(ba2);
//	MemoryStream ms4 = new MemoryStream(ba3);
//	var prv = jsonutil.Deserialize<Provider>(ms4);