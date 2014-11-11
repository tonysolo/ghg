using System;
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

        private static CloudStorageAccount csa = CloudStorageAccount.DevelopmentStorageAccount;
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
        /// Downloads csv strings containing names of regions
        /// from blob storage  
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

        public static void RegisterLoader(string[] ldr)//, Encoding enc)
        {
            var json = JsonConvert.SerializeObject(ldr);
            var account = csa; //CloudStorageAccount.DevelopmentStorageAccount;          
            var cont = account.CreateCloudBlobClient().GetContainerReference(ldr[0]); //"2aabb"
            cont.CreateIfNotExists();
            var loader = cont.GetPageBlobReference("l");
            //var bytes = enc.GetBytes(json);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            var grow = (512 - bytes.Length % 512);
            Array.Resize(ref bytes, bytes.Length + grow);
            loader.UploadFromByteArray(bytes, 0, bytes.Length);
        }

      //  public static int GetNextLoaderPos()
       // {
        //    var account = csa;
       // }

    }
}

