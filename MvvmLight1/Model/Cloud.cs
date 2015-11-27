using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Newtonsoft.Json;

static class jsonutil
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

    public static byte[] compress(byte[] data)
    {
        MemoryStream outStream = new MemoryStream();     
        using (GZipStream gzipStream = new GZipStream(outStream, CompressionMode.Compress))
        using (MemoryStream srcStream = new MemoryStream(data))
        { 
            srcStream.CopyTo(gzipStream); 
            return outStream.ToArray();
        }
    }
    

    public static byte[] decompress(byte[] compressed)
    {
        MemoryStream inStream = new MemoryStream(compressed);
        using (GZipStream gzipStream = new GZipStream(inStream, CompressionMode.Decompress))
        using (MemoryStream outStream = new MemoryStream())
        {
            gzipStream.CopyTo(outStream);
            return outStream.ToArray();
        }
    }


    public static void resize(ref byte[] b)
    {
        var s = b.Length;
        var adjust = 512 - (s % 512);
        Array.Resize(ref b, s + adjust);
    }

}


static public class global
{
    static string _accountname { get; set; } //ghg 
    static CloudStorageAccount _selected_account { get; set; }

    static public CloudStorageAccount setcountryaccount(string accname)
    {
        if (accname == _accountname) return _selected_account;
        _accountname = accname;
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
        _selected_account = new CloudStorageAccount(credentials, useHttps: true);
        return _selected_account;
        //AccountName = accname;
    }



    static void UploadProviderToAzure(Provider provider, string country_region_index)
    {
        var login = country_region_index.Split('=');
        var offset = Convert.ToInt32(login[2], 16) << 22;
        var La = global.setcountryaccount(login[0]).//"ghza"
                    CreateCloudBlobClient().
                    GetContainerReference(login[1]).   //"22427" container
                    GetPageBlobReference("L");
        using (var ms = new MemoryStream())
        {
            jsonutil.Serialize(provider, ms);
            //var len = ms.Length;
            var compressedbytes = jsonutil.compress(ms.ToArray());
            var len = BitConverter.GetBytes(compressedbytes.Length);
            //var len4 = BitConverter.GetBytes(len).Take(4);
            var final = len.Concat(compressedbytes).ToArray();
            jsonutil.resize(ref final);
            using (var ms1 = new MemoryStream(final))
                La.WritePages(ms1, offset);
        }
    }

    static string registernewprovider(string account, string region)
    {
        var L = global.setcountryaccount(account).//"ghza"
        CreateCloudBlobClient().
        GetContainerReference(region).//qnnee eg "22427"
        GetPageBlobReference("L");
        L.FetchAttributes();

        var val = L.Metadata["Next"];
        var tag = L.Properties.ETag;
        var x = (Convert.ToInt32(val, 16) + 1).ToString("x");

        L.Metadata["Next"] = x; //(x + 1).ToString("x");
        L.SetMetadata(AccessCondition.GenerateIfMatchCondition(tag));
        return account + '=' + region + '=' + x;
    }

    static string registernewpatient(string account, string region)
    {
        var P = global.setcountryaccount(account).//"ghza"
        CreateCloudBlobClient().
        GetContainerReference(region).//qnnee eg "22427"
        GetPageBlobReference("P");
        P.FetchAttributes();

        var val = P.Metadata["Next"];
        var tag = P.Properties.ETag;
        var x = (Convert.ToInt32(val, 16) + 1).ToString("x");

        P.Metadata["Next"] = x; //(x + 1).ToString("x");
        P.SetMetadata(AccessCondition.GenerateIfMatchCondition(tag));
        return account + '-' + region + '-' + x;
    }

    //
   static string registernewimage(string account, string region, int imagesize)
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

        var I = global.setcountryaccount(account).//"ghza"
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

public class person
{

    public string[] details = new string[8]; //name,address,cel,email,qnnneee
                                             //[JsonIgnore]										 //public string qne_xxx { get { return details[0]; } set { value = details[0]; } }
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

public class Provider : person
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
    public ObservableCollection<person> Contacts = new ObservableCollection<person>();


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


public class patient : person
{
    public string country { get; set; }
    public string patientId { get; set; }
    public string birthday { get; set; }
    public string sex { get; set; }
    public person nextOfKin { get; set; }  // next of carer
    public List<med_condition> Alerts { get; set; }   //List<Condition> Alerts { get; set; }
    public List<visit> visits { get; set; } // top40 visits choice
    public List<person> Dependants { get; set; }  //qnneepxxxx <Patient>
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