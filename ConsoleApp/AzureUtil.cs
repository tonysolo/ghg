using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

//http://gauravmantri.com/

namespace ConsoleApp
{

    public static class AzureStorage
    {

        // public static CloudStorageAccount Csa = CloudStorageAccount.DevelopmentStorageAccount;
        public static CloudStorageAccount Csa = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("GHGConnectionString"));

        public static CloudPageBlob Patientblob, Populationblob, Imageblob, Loaderblob, Epidemblob;

        //public static string Container { get; set; }



        public static void SetupGhGstorage(CloudStorageAccount account, string qnnee)
        {
            var container = account.CreateCloudBlobClient().GetContainerReference(qnnee);
            container.CreateIfNotExists();
            Patientblob = container.GetPageBlobReference("p");
            if (!Patientblob.Exists())
            {
                Patientblob.Create(0xa000); //for development need to increase to terabyte for prod        
                Patientblob.FetchAttributes();
                Patientblob.Metadata["nextindex"] = "0x00";
                Patientblob.Properties.ContentEncoding = "application/octet-stream";
                Patientblob.SetMetadata();
                Patientblob.SetProperties();
            }

            Populationblob = container.GetPageBlobReference("m");
            if (!Populationblob.Exists())
            {
                Populationblob.Create(0xa000); //for development need to increase to 0x800000 X number of patient blobs
                Populationblob.FetchAttributes();
                Populationblob.Metadata["nextindex"] = "0x00";
                Populationblob.Properties.ContentEncoding = "application/octet-stream";
                Populationblob.SetMetadata();
                Populationblob.SetProperties();
            }

            Imageblob = container.GetPageBlobReference("i");
            if (!Imageblob.Exists())
            {
                Imageblob.Create(0xa000); //for development need to increase to terabyte
                Imageblob.FetchAttributes();
                Imageblob.Metadata["nextindex"] = "0x00"; //data starts at 0
                Imageblob.Properties.ContentEncoding = "application/octet-stream";
                Imageblob.SetMetadata();
                Imageblob.SetProperties();
            }

            Loaderblob = container.GetPageBlobReference("l");
            if (Loaderblob.Exists()) return;
            Loaderblob.Create(0xa000); //need to increase in production 2^32 4 gigs = 1 million * 4 pages
            Loaderblob.FetchAttributes();
            Loaderblob.Metadata.Add("nextindex", "0x00000");
            Loaderblob.Properties.ContentEncoding = "application/octet-stream";
            Loaderblob.SetMetadata();
            Loaderblob.SetProperties();
        }
        /*
         * epidemiology needs users to upload to a storage queue for processing at midnight for the timezone.
         * It is shared data so its value depends on combining.
         * I might try to only update those users that provide epidemiology data.
         * a worker role does the processing and the user has readonly access to the data for his region
         * this will be updated daily and displayed when the app starts
 
                    public static void SetupEpidemStorage (CloudStorageAccount ghgAccount, string qnnee)
                    {
                        //use global ghg account for epidemiology (global)
                        var cname = qnnee.Substring(0, 4).Remove(2, 1);
                        var container = ghgAccount.CreateCloudBlobClient().GetContainerReference(cname);
                        container.CreateIfNotExists();
                        Epidemblob = container.GetPageBlobReference("e"); //production epidem will go in ghg/epidem/qne
                        if (Epidemblob.Exists()) return;
                        Epidemblob.Create(0xa000); //for development need to increase this to fill whole blob
                        Epidemblob.FetchAttributes();
                        Epidemblob.Metadata.Add("nextindex", "0x00");
                        Epidemblob.Metadata["startoffset"] = "0x120";
                        //constant 100 years =36500 days = 0x120 pages 128 per page.
                        //make epoch 1/1/2015
                        Epidemblob.Properties.ContentEncoding = "application/octet-stream";
                        Epidemblob.SetMetadata();
                        Epidemblob.SetProperties();
                    }
         */

        public static void SaveEpidemiology(CloudStorageAccount ghgAccount, string qnnee)
        {
            //var x = Convert.ToInt32((qnnee.Substring(0,1)+qnnee.Substring(2, 1)),16);
            //timezone(qnnee)
            //loadtoqueuetoqueue(qne)
            //setsecstomidnight
            //data offloaded / saved to ghg/qne/e (global biome blobs) 256 x 2 x (45) x 0.25
        }

        //its only the writing metadata that needs concurrency protection
        //all other reads and writes involve overlapping pages which allows concurrent read / write

        /// <summary>
        /// sets loader index string , loader record is fixed length 2048 bytes
        /// </summary>
        /// <returns>string</returns>
        public static string SetNextLoaderIndex()
        {
            var ndx = "";
            Loaderblob.FetchAttributes();
            var etag = Loaderblob.Properties.ETag;       
            var p = Convert.ToInt32(Loaderblob.Metadata["nextindex"], 16);
            p += 1;
            ndx = String.Format("{0:x6}", p);
            Loaderblob.Metadata["nextindex"] = ndx;    
            try
            {
                Loaderblob.SetMetadata(accessCondition: AccessCondition.GenerateIfMatchCondition(etag));
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode == (int)HttpStatusCode.PreconditionFailed) ndx = "";
            }
            return ndx;
        }

        //population management blob and patient blob share the same index but much longer record in patient blob and 
        //this will have higfher traffic, thererfore the smaller population blob is used for metadata and index allocation
        //and patients blob left free for storage 
        //patient is fixed length record 131072 bytes (128K) (very adequate) but in addition the system provides for storing scans and images
        //for extra storage.
        public static string SetNextPatientIndex()
        {
        var ndx = "";
        Populationblob.FetchAttributes();
        var etag = Populationblob.Properties.ETag;
        var p = Convert.ToInt32(Populationblob.Metadata["nextindex"], 16);
        p += 1;
        ndx = String.Format("{0:x6}", p);
        Populationblob.Metadata["nextindex"] = ndx;
        try
        {
            Populationblob.SetMetadata(accessCondition: AccessCondition.GenerateIfMatchCondition(etag));
        }
        catch (StorageException ex)
        {
            if (ex.RequestInformation.HttpStatusCode == (int)HttpStatusCode.PreconditionFailed) ndx = "";
        }
        return ndx;
        }

        //set epidemiology - index for date offset from 1/1/2015 == 0 spanning 512 pages of 8 bytes per day 
        //storing 32 bit page offset for start and end page data ie 64 bits for each day (89 years)
        //? recycle after that as circular buffer modulo 
        //Epidemilogy is stored per QNE (epidemiolgy) blob in a globally shared storage account, epidem     
        //container accessed daily by a single writer so there is no concurrency problem
        public static string StoreEpidemiology(CloudStorageAccount csa, string qne, DateTime dt,string epidjson)
        {
            CloudBlobClient cbc = csa.CreateCloudBlobClient();
            cbc.GetContainerReference(qne);
            CloudPageBlob cloudPageBlob = Epidemblob;


            var bb = Encoding.UTF8.GetBytes(epidjson);
            var grow = (bb.Length % 512);
            Array.Resize(ref bb,bb.Length+grow);

            var dateoffset = (dt - new DateTime(2015, 1, 1)).Days;
            var datecol = (int)dateoffset/64;
            var daterow = (int)dateoffset%64;
            var ms = new MemoryStream(bb);
            Epidemblob.WritePages(ms,daterow * 512);
            var ndx = "";
            Epidemblob.FetchAttributes();
            var etag = Epidemblob.Properties.ETag;
            var p = Convert.ToInt32(Epidemblob.Metadata["nextindex"], 16);
           // data starts at 1<<18  (262144) byte offset

            p += 1;
            ndx = String.Format("{0:x6}", p);
            Epidemblob.Metadata["nextindex"] = ndx;
            try
            {
                Epidemblob.SetMetadata(accessCondition: AccessCondition.GenerateIfMatchCondition(etag));
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode == (int)HttpStatusCode.PreconditionFailed) ndx = "";
            }
            return ndx;  
        }

        //set population management index / next patient index
        //search / list loaders
        //search / list patients
        //search epidemiolgy date range
        //save image and return the image location to patient blob
        //save epidemiology data and record the location in epidemiology date list.



        /// <summary>
        /// registers a new loader
        /// </summary>
        /// <param name="json"></param>
        /// <returns>string index</returns>
        public static string RegisterNewLoader(string json)
        {
            var bytes = Encoding.UTF8.GetBytes(json);
            var grow = (512 - bytes.Length % 512);
            Array.Resize(ref bytes, bytes.Length + grow);
            var s = SetNextLoaderIndex();
            if (s == "") return s;
            var p = Convert.ToInt32(s, 16);
            var ms = new MemoryStream(bytes);
            var start = p << 10;
            Loaderblob.WritePages(ms, start);
            return s;
        }
    

    public static byte[] Testmem(int start, int length)
    {
        const string s = "TonyManicom";
        var b = Encoding.UTF8.GetBytes(s);
        var page = new byte[200];
        var ms = new MemoryStream(page);
        ms.Seek(100,0);
        ms.Write(b,start,length);
        //var b1 =  ms.ToArray();
        return page;
    }

        public static byte[] Testmem1()
        {
            const ulong x = 0xfecdfecdfecdfecd;     
        var b = BitConverter.GetBytes(x);
       // var c = BitConverter.GetBytes(y);
        var page = new byte[128];
        var ms = new MemoryStream(page);
        ms.Seek(0,SeekOrigin.Begin);
        ms.Write(b,0,8);
            foreach (var y in page.Where(y => y!=0))
            
                Console.WriteLine(y);
            
        //var b1 =  ms.ToArray();
        return page;
       // return c[0];

    }

    }
    public class Test
    {
        public static void Main()
        {
            
//Console.WriteLine(Encoding.UTF8.GetString(AzureStorage.Testmem(4,7)));
Console.Write(AzureStorage.Testmem1().ToString());            
          // AzureStorage.SetupGhGstorage(AzureStorage.Csa, "21f29");

          //  for(var i=0;i<50;i++)
           // Console.WriteLine(AzureStorage.SetNextLoaderIndex());

            Console.ReadLine();

            
            //const string str = "21f29,Tony Manicom,173 Blandford Rd, North Riding,etc";

            //var sarr = str.Split(',');

            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(sarr);

            //var s = AzureStorage.RegisterNewLoader(json);
            //var t = AzureStorage.RegisterNewLoader(json);
            /*
            Console.WriteLine(QneUtils.Timezone("1aa0a"));
            Console.WriteLine(QneUtils.Timezone("1aa1a"));
            Console.WriteLine(QneUtils.Timezone("1aa20"));
            Console.WriteLine(QneUtils.Timezone("1aa31"));
             */ 

        }
    }
}






