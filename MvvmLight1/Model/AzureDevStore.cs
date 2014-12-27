using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;


namespace MvvmLight1.Model
{
    public static class AzureUtil
    {
        public static string Loaderid { get; set; }

        public static string[] DownloadData()
        {
            var qne = Loaderid.Substring(0, 5);
            var pagenumber = Convert.ToInt16(Loaderid.Substring(5, Loaderid.Length - 5));
            var startoffset =   pagenumber << 9;
            var endoffset =   ((pagenumber + 1) << 9) - 1;
            return new string[1];
        }


        public static IEnumerable<CloudBlobContainer> DevelopmentContainers()
        {
            var dev = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));//"StorageConnectionString"
            var blobClient = dev.CreateCloudBlobClient();
            return blobClient.ListContainers();
            //ToArray<
            //CloudBlobContainer container = blobClient.GetContainerReference("2aabb");
            // IEnumerable<CloudBlobContainer> cbc = blobClient.ListContainers();
            //string s = container.Name;        
            //return s; 
        }


        private static readonly CloudStorageAccount CldStoreAcc = CloudStorageAccount.DevelopmentStorageAccount;
        /// <summary>
        /// Gets all the name names for the GHG management account
        /// </summary>
        /// <returns></returns>
        public static string[] CountryNames()
        {
            var storageAccount = CloudStorageAccount.Parse(
            CloudConfigurationManager.GetSetting("GHGConnectionString"));
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("countries");
            var blockBlob = container.GetBlockBlobReference("countries.txt");
            string text;
            using (var memoryStream = new MemoryStream())
            {
                blockBlob.DownloadToStream(memoryStream);
                text = Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            return text.Split(',');
        }


        /// <summary>
        /// Downloads csv strings containing names of regions from blob storage  
        /// </summary>
        /// <param name="country">string</param>
        /// <returns>CSV string</returns>
        public static string DownloadRegions(string country)
        {
            var lower = country.ToLower();
            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("GHGConnectionString"));
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("countries");
            var blockBlob = container.GetBlockBlobReference(lower);
            string text;
            using (var memoryStream = new MemoryStream())            
            {
                blockBlob.DownloadToStream(memoryStream);
                text = Encoding.UTF8.GetString(memoryStream.ToArray());
            }
           
            return text;
        }

   
        public static string[] CurrentCountry; //placeholder for all current qnnee regions


        public static bool InRange(string region)
        {
            return CurrentCountry.Contains(region);
        }

        public static string Centreregion = ""; //centre of Region placeholder qnnee



        public static int Secstomidnight(string queue) //needs checking
        {
            var utc = DateTime.UtcNow;
            var x = (utc.Second) + (utc.Minute * 60) + (utc.Hour * 60 * 60);
            var utcsecstomidnight = (86400 - x);
            var qe = Convert.ToByte(queue, 16);
            var longit = (byte)(qe & 0x1f);
            var timezonesecs = longit * 45 * 60;
            return (timezonesecs - utcsecstomidnight + 86400) % 86400;
        }



        public static bool PageBlobExists(CloudBlobClient client, string container, string key)
        {
            return client.GetContainerReference(container).
                GetPageBlobReference(key). Exists();             
        }


        public static bool BlockBlobExists(CloudBlobClient client, string container, string key)
        {
            return client.GetContainerReference(container).
                GetBlockBlobReference(key).Exists();               
        }

        public static bool ContainerExists(CloudBlobClient client, string container)
        {
            return client.GetContainerReference(container).Exists();
        }


        public static string[] QueueNames
        {
            get
            {
             var cql = CloudStorageAccount.DevelopmentStorageAccount.
                    CreateCloudQueueClient().ListQueues();
                var cqarr = cql.ToArray();
                var sarr = new string[cqarr.Length];
                for (var i = 0; i < sarr.Length; i++) sarr[i] = cqarr[i].Name;
                return sarr;
            }
        }

        public static string[] BlobContainerNames
        {
            get
            {
                var cbc = CloudStorageAccount.DevelopmentStorageAccount.
                    CreateCloudBlobClient().ListContainers();
                var ccarr = cbc.ToArray();
                var sarr = new string[ccarr.Length];
                for (var i = 0; i < sarr.Length; i++) sarr[i] = ccarr[i].Name;
                return sarr;
            }
        }

        public static void LoadtoQueue(CloudQueueMessage msg, int queue, int waitsecs)
        {
            var qname = "t-" + queue;
            var ts = TimeSpan.FromSeconds(waitsecs);
            var cqc = CloudStorageAccount.
                DevelopmentStorageAccount.CreateCloudQueueClient();
            cqc.GetQueueReference(qname).AddMessage(msg, null, ts, null, null);
        }

        public static void RegisterLoader(string[] loaderArray)
        {         
            var account = CldStoreAcc; //CloudStorageAccount.DevelopmentStorageAccount;          
            var cont = account.CreateCloudBlobClient().GetContainerReference(loaderArray[0]); //"2aabb"
            if (cont == null) return;
            cont.CreateIfNotExists();
            var loader = cont.GetPageBlobReference("l");
            loader.FetchAttributes();

            if (! loader.Metadata.ContainsKey("NextLoader")) loader.Metadata.Add("NextLoader","0");
           
            loaderArray[1] = loader.Metadata["NextLoader"];//hexadecimal next in sequence id for each loader/provider
            var ndx = Convert.ToInt16(loaderArray[1],16);
            loader.Metadata["NextLoader"] = String.Format("{0:x}",ndx + 1);

            var json = JsonConvert.SerializeObject(loaderArray.Skip(1));         
            var bytes = Encoding.UTF8.GetBytes(json);
            var grow = (512 - bytes.Length % 512);
            Array.Resize(ref bytes, bytes.Length + grow);
            
            loader.UploadFromByteArray(bytes,0,bytes.Length);
        }

        //edit loader to record epidemiology top 40 choices for loaders to reuse
        //need to truncate this to say 2 pages but plenty room in loader blob for more
        //
       //  public static int GetNextLoaderPos()

        public static void UploadLoaderData(string[] sarr)
        {
            var qne = Loaderid.Substring(0, 5);
            var pagenumber = Convert.ToInt16(Loaderid.Substring(5, Loaderid.Length - 5));
            var startoffset = pagenumber << 9;
            var s = JsonConvert.SerializeObject(sarr);
            var a = JsonConvert.DeserializeObject<string[]>(s);
            //var saa = (string[])a;

            var account = CloudStorageAccount.DevelopmentStorageAccount;
            // var en = account.CreateCloudBlobClient().ListContainers();

            account.CreateCloudBlobClient().GetContainerReference("qnnee").CreateIfNotExists();

            var container = account.CreateCloudBlobClient().GetContainerReference("qnnee");
            // var container = en.ElementAt(0);
            //string blobname = "tony/manicom";
            var loader = container.GetPageBlobReference("/l");
            loader.Create(8192);
            //const string s1 = "Tony Manicom/n173 blandford road/n north riding, Randburg/n";
            var sb = Encoding.UTF8.GetBytes(s);

            // byte[] ba = new byte[512];
            var grow = 512 - ((sb.Length) % 512);
            Array.Resize(ref sb, sb.Length + grow);
            //  for (int i = 0; i < sb.Length; i++) ba[i] = sb[i];      
            //ba.
            // byte[] x = new byte[512];
            // for (int i = 0; i < 512; i++) x[i] = (byte)i;
            loader.UploadFromByteArray(sb, 0, 512);

            var readerblob = container.GetPageBlobReference("/l");
            var stream = readerblob.OpenRead();
            var buffer = new byte[512];
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            stream.Read(buffer, 0, 512);


            var text = Encoding.UTF8.GetString(buffer);
            //Model.AzureStorage.devListContainers();    

        }

        public static void SetupAzureAccount()
        {
            // Settings.Registered = true;
            var account = CloudStorageAccount.DevelopmentStorageAccount;

            var contain = account.CreateCloudBlobClient().GetContainerReference("2aabb");
            var tony = contain.GetPageBlobReference("blob");
            // var s = container.Name;
            tony.Create(8192);
            const string s1 = "Tony Manicom/n173 blandford road/n north riding, Randburg/n";
            var sb = Encoding.UTF8.GetBytes(s1);

            // byte[] ba = new byte[512];
            var grow = 512 - ((sb.Length) % 512);
            Array.Resize(ref sb, sb.Length + grow);
            //  for (int i = 0; i < sb.Length; i++) ba[i] = sb[i];      
            //ba.
            // byte[] x = new byte[512];
            // for (int i = 0; i < 512; i++) x[i] = (byte)i;
            // loader.UploadFromByteArray(sb, 0, 512);

            //  var readerblob = container.GetPageBlobReference("l");
            //  var stream = readerblob.OpenRead();
            var buffer = new byte[512];
            //  stream.Seek(0, System.IO.SeekOrigin.Begin);
            //  stream.Read(buffer, 0, 512);
            //Model.AzureStorage.devListContainers();     
        }

    }
}

