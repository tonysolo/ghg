﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;


namespace MvvmLight1.Model
{
    public static class AzureDevStorage
    {
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
           CloudQueueClient cqc = Microsoft.WindowsAzure.Storage.CloudStorageAccount.DevelopmentStorageAccount.CreateCloudQueueClient();
            cqc.GetQueueReference("t-0").AddMessage(msg,null,null,null,null);
        }
        
    }
}
