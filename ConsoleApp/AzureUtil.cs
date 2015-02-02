using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Markup;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

//http://gauravmantri.com/

namespace ConsoleApp
{
    public static class AzureGhgStorage
    {
        public static CloudStorageAccount Csa = CloudStorageAccount.DevelopmentStorageAccount;
        //public static CloudStorageAccount Csa = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("GHGConnectionString"));

        public static string GetCountryList()
        {
            var ghgAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("GHGConnectionString"));
            var container = ghgAccount.CreateCloudBlobClient().GetContainerReference("countries");
            var cbc = container.GetBlockBlobReference("countries.txt");
            return cbc.DownloadText(Encoding.UTF8);           
        }

        public static string GetCountry(string country)
        {
            var ghgAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("GHGConnectionString"));
            var container = ghgAccount.CreateCloudBlobClient().GetContainerReference("countries");
            var cbc = container.GetBlockBlobReference(country+".txt");
            return cbc.DownloadText(Encoding.UTF8);
        }

        public static CloudPageBlob Epidemblob;
        /*
                 * epidemiology needs users to upload to a storage queue for processing at midnight for the timezone.
                 * It is shared data so its value depends on combining.
                 * I might try to only update those users that provide epidemiology data.
                 * a worker role does the processing and the user has readonly access to the data for his region
                 * this will be updated daily and displayed when the app starts
         */
        public static void SetupEpidemStorage(CloudStorageAccount ghgAccount, string qnnee)
        {
            //use global ghg account for epidemiology (global)
            var cname = qnnee.Substring(0, 4).Remove(2, 1);
            var container = ghgAccount.CreateCloudBlobClient().GetContainerReference(cname);
            container.CreateIfNotExists();
            Epidemblob = container.GetPageBlobReference("e"); //production epidem will go in ghg/epidem/qne
            if (Epidemblob.Exists()) return;
            Epidemblob.Create(0x40000); //256 pages for development, later need to increase this to fill whole blob
            Epidemblob.FetchAttributes();
            Epidemblob.Metadata.Add("nextindex", "0x20000");//start of epidem
            // Epidemblob.Metadata["startoffset"] = "0x120";
            //constant 100 years =36500 days X 4 bytes. 90 years = 256 pages 128 days per page.
            //make epoch 1/1/2015
            Epidemblob.Properties.ContentEncoding = "application/octet-stream";
            Epidemblob.SetMetadata();
            Epidemblob.SetProperties();
        }


        public static void SaveEpidemiology(CloudStorageAccount ghgAccount, string qnnee)
        {
            //var x = Convert.ToInt32((qnnee.Substring(0,1)+qnnee.Substring(2, 1)),16);
            //timezone(qnnee)
            //loadtoqueuetoqueue(qne)
            //setsecstomidnight
            //data offloaded / saved to ghg/qne/e (global biome blobs) 256 x 2 x (45) x 0.25

            //measure the epidemiology data
            const string txt = "Epidemiology testing 1234456567677 testing 123455, 67889899";
            var epiData = Encoding.UTF8.GetBytes(txt);
            var grow = epiData.Length % 512;
            Array.Resize(ref epiData, epiData.Length + grow);
            var epipagesCount = epiData.Length / 512;

            //get the index and update it for future
            Epidemblob.FetchAttributes();
            var pos = Convert.ToInt32(Epidemblob.Metadata["nextindex"], 16);
            Epidemblob.Metadata["nextindex"] = String.Format("{0:x4}", pos + epipagesCount);

            //update the epidemiology blob lookup table 
            //lookup table occupies 256 pages = 256*128 days = 89 years
            //epidemiology update is a scheduled task at midnight so doesnt need special processing requred for manual data
            var days = (DateTime.Now.Subtract(new DateTime(2015, 1, 1))).Days;
            var lookupPage = days / 128;
            var lookupPos = days % 128;
            var lookupBuf = new byte[512];

            var epidDataOffset = 256 * 128 * 4;//bytes (32768 UInt32's)

            var lookupStream = Epidemblob.OpenRead();
            lookupStream.Seek(lookupPage * 512, SeekOrigin.Begin);//go to the lookup page start of page boundary
            lookupStream.Read(lookupBuf, 0, 512); // get the current lookup page
            var memoryStream = new MemoryStream(lookupBuf);//use a memorystream for editing
            memoryStream.Seek(lookupPos, SeekOrigin.Begin);
            //go to byte offset and replace the 4 byte page address to lookup data
            // memoryStream.Write(epiData,0,);

        }

        //set epidemiology - index for date offset from 1/1/2015 == 0 spanning 256 pages of 4 bytes per day 
        //storing 32 bit end page data ie 32 bits for each day (89 years)
        //? recycle after that as circular buffer modulo 32768
        //Epidemilogy is stored per QNE (epidemiolgy) blob in a globally shared storage account, epidem     
        //container accessed daily by a single writer so there is no concurrency problem
        public static string StoreEpidemiology(CloudStorageAccount csa, string qnnee, DateTime dt, string epidjson)
        {
            SetupEpidemStorage(csa, qnnee);
            const int skiplookup = 10 * 512; //(10 testing)  256*512 = 90 years for real
            var bytes = Encoding.UTF8.GetBytes(epidjson);
            var grow = (512 - bytes.Length % 512);
            Array.Resize(ref bytes, bytes.Length + grow);

            var dateoffset = (dt - new DateTime(2015, 1, 1)).Days;

            var lookupByteoffset = (int)dateoffset % 128;
            var lookupPage = (int)dateoffset / 128;



            using (var stream = new MemoryStream(bytes))
                AzureGhgStorage.Epidemblob.WritePages(stream, lookupPage * 512);

            var ndx = "";

            Epidemblob.FetchAttributes();
            var p = Convert.ToInt32(Epidemblob.Metadata["nextindex"], 16);
            var pnext = p + (bytes.Length / 512) + 1;
            Epidemblob.Metadata["nextindex"] = String.Format("{0:x}", pnext);

            using (var ms = new MemoryStream(bytes))
            {
                Epidemblob.WritePages(ms, p * 512 + skiplookup);
            }

            var day = (DateTime.Today - new DateTime(2015, 1, 1)).Days;
            var drow = day / 128;
            var dcol = day % 128;
            var barr = new byte[512];
            const uint n = 326;

            using (var stm = Epidemblob.OpenRead())
            {
                stm.Seek(drow * 512, SeekOrigin.Begin);
                stm.Read(barr, 0, 512);
            }

            using (var ms1 = new MemoryStream(barr))
            {
                ms1.Seek(dcol * 4, SeekOrigin.Begin);
                ms1.Write(BitConverter.GetBytes(n), 0, 4);
                ms1.Seek(0, SeekOrigin.Begin);
                Epidemblob.WritePages(ms1, drow * 512);
            }
            p += 1;
            ndx = String.Format("{0:x6}", p);
            Epidemblob.Metadata["nextindex"] = ndx;
            Epidemblob.SetMetadata();
            return ndx;
        }
  

    }

    public static class AzureEhealth
    {
        public static string Account { get; set; }
        
       // public static CloudStorageAccount Csa = CloudStorageAccount.DevelopmentStorageAccount;
       public static CloudStorageAccount Csa = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(Account));

        public static CloudPageBlob Patientblob, Populationblob, Imageblob, Loaderblob;

        //public static string Container { get; set; }


//one time setup. Must not be changed or data will be lost ("nextindex" must not b reset)
        public static void AdminSetupGhGstorage(CloudStorageAccount account, string qnnee)
        {
            var container = account.CreateCloudBlobClient().GetContainerReference(qnnee);
            container.CreateIfNotExists();
            Patientblob = container.GetPageBlobReference("p");
            if (!Patientblob.Exists())
            {
                Patientblob.Create(0xa000); //for development need to increase to terabyte for prod        
                Patientblob.FetchAttributes();
                Patientblob.Metadata.Add("nextindex",String.Format("{0:x4}", 0));
                Patientblob.Properties.ContentEncoding = "application/octet-stream";
                Patientblob.SetMetadata();
                Patientblob.SetProperties();
            }

            Populationblob = container.GetPageBlobReference("m");
            if (!Populationblob.Exists())
            {
                Populationblob.Create(0xa000); //for development need to increase to 0x800000 X number of patient blobs
                Populationblob.FetchAttributes();
                Populationblob.Metadata.Add("nextindex",String.Format("{0:x4}", 0));
                Populationblob.Properties.ContentEncoding = "application/octet-stream";
                Populationblob.SetMetadata();
                Populationblob.SetProperties();
            }

            Imageblob = container.GetPageBlobReference("i");
            if (!Imageblob.Exists())
            {
                Imageblob.Create(0xa000); //for development need to increase to terabyte for release
                Imageblob.FetchAttributes();
                Imageblob.Metadata.Add("nextindex",String.Format("{0:x4}", 4095));//need millions for release
                Imageblob.Properties.ContentEncoding = "application/octet-stream";
                Imageblob.SetMetadata();
                Imageblob.SetProperties();
            }

            Loaderblob = container.GetPageBlobReference("l");
            if (Loaderblob.Exists()) return;
            Loaderblob.Create(0xa000); //need to increase in production 2^32 4 gigs = 1 million * 4 pages
            Loaderblob.FetchAttributes();
            Loaderblob.Metadata.Add("nextindex", String.Format("{0:x4}", -1));
            Loaderblob.Properties.ContentEncoding = "application/octet-stream";
            Loaderblob.SetMetadata();
            Loaderblob.SetProperties();
        }
        




        /// <summary>
        /// sets loader index string , loader record is fixed length 2048 bytes
        /// </summary>
        /// <returns>int</returns>//-1 == fail
        public static int SetNextLoaderIndex()
        {
            Loaderblob.FetchAttributes();
            var etag = Loaderblob.Properties.ETag;       
            var p = Convert.ToInt32(Loaderblob.Metadata["nextindex"], 16);
            var ndx = String.Format("{0:x6}", p += 1);
            Loaderblob.Metadata["nextindex"] = ndx;    
            try
            {
                Loaderblob.SetMetadata(accessCondition: AccessCondition.GenerateIfMatchCondition(etag));
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode == (int)HttpStatusCode.PreconditionFailed) p = -1;
            }
            return p;
        }

        //population management blob and patient blob share the same index but much longer record in patient blob and 
        //this will have higfher traffic, thererfore the smaller population blob is used for metadata and index allocation
        //and patients blob left free for storage 
        //patient is fixed length record 131072 bytes (128K) (very adequate) but in addition the system provides for storing scans and images
        //for extra storage.
        /// <summary>
        /// Return the next index  and updates the counter (-1 indicates failure)
        /// </summary>
        /// <returns>int</returns>
        public static Int32 GetNextPatientIndex()
        {
        Populationblob.FetchAttributes();     
        var ndx = Convert.ToInt32(Populationblob.Metadata["nextindex"], 16);
        var etag = Populationblob.Properties.ETag;
        var next = String.Format("{0:x}", ndx+1);
        Populationblob.Metadata["nextindex"] = next;
        try
        {
            Populationblob.SetMetadata(accessCondition: AccessCondition.GenerateIfMatchCondition(etag));
        }
        catch (StorageException ex)
        {
            if (ex.RequestInformation.HttpStatusCode == (int)HttpStatusCode.PreconditionFailed) ndx = -1;
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
        public static int RegisterNewLoader(string json)
        {
            Account = "GHGConnetionString";
            var bytes = Encoding.UTF8.GetBytes(json);
            var grow = (512 - bytes.Length % 512);
            Array.Resize(ref bytes, bytes.Length + grow);
            var s = SetNextLoaderIndex();
            if (s == -1) return s;
           // var p = Convert.ToInt32(s, 16);
            var ms = new MemoryStream(bytes);
            var start = s << 10;
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
           

           const UInt32 x = 0xfecdfecd;     
        var b = BitConverter.GetBytes(x);
       // var c = BitConverter.GetBytes(y);
        var page = new byte[512];
        var ms = new MemoryStream(page);
        ms.Seek(4,SeekOrigin.Begin);
        ms.Write(b,0,4);
            foreach (var y in page)//.Where(y => y!=0))          
                Console.Write(String.Format("{0:x2}",y));
            Console.WriteLine();
         Console.WriteLine(page.Length);   
        //var b1 =  ms.ToArray();
        return page;
       // return c[0];

    }


       
    }

    public class Test
    {
        public static void Main()
        { 
           // AzureEhealth.SetupEpidemStorage(AzureEhealth.Csa, "21f29");
           // AzureGhgStorage.StoreEpidemiology(AzureEhealth.Csa, "21f29", DateTime.Today, "Testing testing testing 123444");
            //var s = String.Format("{0:x8}", 268435457);
           // Console.WriteLine();
           var s = AzureGhgStorage.GetCountryList().Split(',');
            Console.WriteLine("Test");
            foreach (var t in s)
                Console.WriteLine(t);
            Console.ReadLine();
            s = AzureGhgStorage.GetCountry("gb").Split(',');
            Console.WriteLine("Test1");
 foreach (var t in s)
                Console.WriteLine(t);
Console.ReadLine();
        }

   
    }

//Console.WriteLine(Encoding.UTF8.GetString(AzureEhealth.Testmem(4,7)));
//Console.Write(AzureEhealth.Testmem1().ToString());            
          // AzureEhealth.AdminSetupGhGstorage(AzureEhealth.Csa, "21f29");

          //  for(var i=0;i<50;i++)
           // Console.WriteLine(AzureEhealth.SetNextLoaderIndex());

          //  Console.ReadLine();

            
            //const string str = "21f29,Tony Manicom,173 Blandford Rd, North Riding,etc";

            //var sarr = str.Split(',');

            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(sarr);

            //var s = AzureEhealth.RegisterNewLoader(json);
            //var t = AzureEhealth.RegisterNewLoader(json);
            /*
            Console.WriteLine(QneUtils.Timezone("1aa0a"));
            Console.WriteLine(QneUtils.Timezone("1aa1a"));
            Console.WriteLine(QneUtils.Timezone("1aa20"));
            Console.WriteLine(QneUtils.Timezone("1aa31"));
             */ 

        }
    







