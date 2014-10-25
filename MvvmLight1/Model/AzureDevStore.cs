using System;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;


namespace MvvmLight1.Model
{
    public static class AzureUtil
    {
        public static int Secstomidnight(string queue)
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

    }
}

