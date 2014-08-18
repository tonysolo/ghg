using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;


namespace MvvmLight1.Model
{
    public static class AzureUtil
    {
        public static int secstomidnight(string qe)
        {
            DateTime utc = DateTime.UtcNow;
            int x = (utc.Second) + (utc.Minute * 60) + (utc.Hour * 60 * 60);
            int utcsecstomidnight = (86400 - x);
            byte _qe = Convert.ToByte(qe, 16);
            byte longit = (byte)(_qe & 0x1f);
            int timezonesecs = longit * 45 * 60;
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
                IEnumerable<CloudQueue> cql =
                    Microsoft.WindowsAzure.Storage.CloudStorageAccount.DevelopmentStorageAccount.
                    CreateCloudQueueClient().ListQueues();
                CloudQueue[] cqarr = cql.ToArray<CloudQueue>();
                string[] sarr = new string[cqarr.Length];
                for (int i = 0; i < sarr.Length; i++) sarr[i] = cqarr[i].Name;
                return sarr;
            }
        }

        public static string[] BlobContainerNames
        {
            get
            {
                IEnumerable<CloudBlobContainer> cbc =
                    Microsoft.WindowsAzure.Storage.CloudStorageAccount.DevelopmentStorageAccount.
                    CreateCloudBlobClient().ListContainers();
                CloudBlobContainer[] ccarr = cbc.ToArray<CloudBlobContainer>();
                string[] sarr = new string[ccarr.Length];
                for (int i = 0; i < sarr.Length; i++) sarr[i] = ccarr[i].Name;
                return sarr;
            }
        }

        public static void LoadtoQueue(CloudQueueMessage msg, int queue, int waitsecs)
        {
            string qname = "t-" + queue.ToString();
            TimeSpan ts = new TimeSpan();
            ts = TimeSpan.FromSeconds(waitsecs);
            CloudQueueClient cqc = Microsoft.WindowsAzure.Storage.CloudStorageAccount.
                DevelopmentStorageAccount.CreateCloudQueueClient();
            cqc.GetQueueReference(qname).AddMessage(msg, null, ts, null, null);
        }

    }
}

