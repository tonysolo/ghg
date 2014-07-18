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
    public static class AzureAccess
    {
        public static string[] Qnames
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
        
    }
}

