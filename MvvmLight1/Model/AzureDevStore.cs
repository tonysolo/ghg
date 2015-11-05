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
        private static readonly CloudStorageAccount CldStoreAcc = CloudStorageAccount.DevelopmentStorageAccount;
        public static string[] CurrentCountry; //placeholder for all current qnnee regions
        public static string Centreregion = ""; //centre of Region placeholder qnnee
        public static string Loaderid { get; set; }

        public static string[] QueueNames
        {
            get
            {
                IEnumerable<CloudQueue> cql = CloudStorageAccount.DevelopmentStorageAccount.
                    CreateCloudQueueClient().ListQueues();
                CloudQueue[] cqarr = cql.ToArray();
                var sarr = new string[cqarr.Length];
                for (int i = 0; i < sarr.Length; i++) sarr[i] = cqarr[i].Name;
                return sarr;
            }
        }

        public static string[] BlobContainerNames
        {
            get
            {
                IEnumerable<CloudBlobContainer> cbc = CloudStorageAccount.DevelopmentStorageAccount.
                    CreateCloudBlobClient().ListContainers();
                CloudBlobContainer[] ccarr = cbc.ToArray();
                var sarr = new string[ccarr.Length];
                for (int i = 0; i < sarr.Length; i++) sarr[i] = ccarr[i].Name;
                return sarr;
            }
        }

        public static string[] DownloadData()
        {
            string qne = Loaderid.Substring(0, 5);
            short pagenumber = Convert.ToInt16(Loaderid.Substring(5, Loaderid.Length - 5));
            int startoffset = pagenumber << 9;
            int endoffset = ((pagenumber + 1) << 9) - 1;
            return new string[1];
        }


        public static IEnumerable<CloudBlobContainer> DevelopmentContainers()
        {
            CloudStorageAccount dev =
                CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            //"StorageConnectionString"
            CloudBlobClient blobClient = dev.CreateCloudBlobClient();
            return blobClient.ListContainers();
            //ToArray<
            //CloudBlobContainer container = blobClient.GetContainerReference("2aabb");
            // IEnumerable<CloudBlobContainer> cbc = blobClient.ListContainers();
            //string s = container.Name;        
            //return s; 
        }


        /// <summary>
        ///     Gets all the name names for the GHG management account
        /// </summary>
        /// <returns></returns>
        public static string[] CountryNames()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("GHGConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("countries");
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("countries.txt");
            string text;
            using (var memoryStream = new MemoryStream())
            {
                blockBlob.DownloadToStream(memoryStream);
                text = Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            return text.Split(',');
        }


        /// <summary>
        ///     Downloads csv strings containing names of regions from blob storage
        /// </summary>
        /// <param name="country">string</param>
        /// <returns>CSV string</returns>
        public static string DownloadRegions(string country)
        {
            string lower = country.ToLower();
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("GHGConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("countries");
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(lower);
            string text;
            using (var memoryStream = new MemoryStream())
            {
                blockBlob.DownloadToStream(memoryStream);
                text = Encoding.UTF8.GetString(memoryStream.ToArray());
            }

            return text;
        }


        public static bool InRange(string region)
        {
            return CurrentCountry.Contains(region);
        }


        public static int Secstomidnight(string queue) //needs checking
        {
            DateTime utc = DateTime.UtcNow;
            int x = (utc.Second) + (utc.Minute*60) + (utc.Hour*60*60);
            int utcsecstomidnight = (86400 - x);
            byte qe = Convert.ToByte(queue, 16);
            var longit = (byte) (qe & 0x1f);
            int timezonesecs = longit*45*60;
            return (timezonesecs - utcsecstomidnight + 86400)%86400;
        }


        public static bool PageBlobExists(CloudBlobClient client, string container, string key)
        {
            return client.GetContainerReference(container).
                GetPageBlobReference(key).Exists();
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


        public static void LoadtoQueue(CloudQueueMessage msg, int queue, int waitsecs)
        {
            string qname = "t-" + queue;
            TimeSpan ts = TimeSpan.FromSeconds(waitsecs);
            CloudQueueClient cqc = CloudStorageAccount.
                DevelopmentStorageAccount.CreateCloudQueueClient();
            cqc.GetQueueReference(qname).AddMessage(msg, null, ts, null, null);
        }

        public static void RegisterLoader(string[] loaderArray)
        {
            CloudStorageAccount account = CldStoreAcc; //CloudStorageAccount.DevelopmentStorageAccount;          
            CloudBlobContainer cont = account.CreateCloudBlobClient().GetContainerReference(loaderArray[0]); //"2aabb"
            if (cont == null) return;
            cont.CreateIfNotExists();
            CloudPageBlob loader = cont.GetPageBlobReference("l");
            loader.FetchAttributes();
            if (! loader.Metadata.ContainsKey("NextLoader")) loader.Metadata.Add("NextLoader", "0");

            loaderArray[1] = loader.Metadata["NextLoader"]; //hexadecimal next in sequence Id for each loader/Provider
            short ndx = Convert.ToInt16(loaderArray[1], 16);
            loader.Metadata["NextLoader"] = String.Format("{0:x}", ndx + 1);

            string json = JsonConvert.SerializeObject(loaderArray.Skip(1));
                //skips the qnnee field //should check that loaderdata is <= 512 bytes
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            int grow = (512 - bytes.Length%512);
            Array.Resize(ref bytes, bytes.Length + grow);
            loader.UploadFromByteArray(bytes, 0, bytes.Length);
        }

        //edit loader to record epidemiology top 40 choices for loaders to reuse
        //need to truncate this to say 2 pages but plenty room in loader blob for more
        //
        //  public static int GetNextLoaderPos()

        public static void UploadLoaderData(string[] sarr)
        {
            string qne = Loaderid.Substring(0, 5);
            short pagenumber = Convert.ToInt16(Loaderid.Substring(5, Loaderid.Length - 5));
            int startoffset = pagenumber << 9;
            string s = JsonConvert.SerializeObject(sarr);
            var a = JsonConvert.DeserializeObject<string[]>(s);
            //var saa = (string[])a;

            CloudStorageAccount account = CloudStorageAccount.DevelopmentStorageAccount;
            // var en = account.CreateCloudBlobClient().ListContainers();

            account.CreateCloudBlobClient().GetContainerReference("qnnee").CreateIfNotExists();

            CloudBlobContainer container = account.CreateCloudBlobClient().GetContainerReference("qnnee");
            // var container = en.ElementAt(0);
            //string blobname = "tony/manicom";
            CloudPageBlob loader = container.GetPageBlobReference("/l");
            loader.Create(8192);
            //const string s1 = "Tony Manicom/n173 blandford road/n north riding, Randburg/n";
            byte[] sb = Encoding.UTF8.GetBytes(s);

            // byte[] ba = new byte[512];
            int grow = 512 - ((sb.Length)%512);
            Array.Resize(ref sb, sb.Length + grow);
            //  for (int i = 0; i < sb.Length; i++) ba[i] = sb[i];      
            //ba.
            // byte[] x = new byte[512];
            // for (int i = 0; i < 512; i++) x[i] = (byte)i;
            loader.UploadFromByteArray(sb, 0, 512);

            CloudPageBlob readerblob = container.GetPageBlobReference("/l");
            Stream stream = readerblob.OpenRead();
            var buffer = new byte[512];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(buffer, 0, 512);


            string text = Encoding.UTF8.GetString(buffer);
            //Model.AzureStorage.devListContainers();    
        }

        public static void SetupAzureAccount()
        {
            // Settings.Registered = true;
            CloudStorageAccount account = CloudStorageAccount.DevelopmentStorageAccount;

            CloudBlobContainer contain = account.CreateCloudBlobClient().GetContainerReference("2aabb");
            CloudPageBlob tony = contain.GetPageBlobReference("blob");
            // var s = container.Name;
            tony.Create(8192);
            const string s1 = "Tony Manicom/n173 blandford road/n north riding, Randburg/n";
            byte[] sb = Encoding.UTF8.GetBytes(s1);

            // byte[] ba = new byte[512];
            int grow = 512 - ((sb.Length)%512);
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