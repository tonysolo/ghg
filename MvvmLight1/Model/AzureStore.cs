using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure;


namespace MvvmLight1.Model
{
   public static class AzureStorage
    {
       public static string devListContainers()
       {
           //string[] saa = new string[10];
           CloudStorageAccount dev = CloudStorageAccount.Parse
               (
               CloudConfigurationManager.
               GetSetting("StorageConnectionString")
               );
           //int i = 0;
           CloudBlobClient blobClient = dev.CreateCloudBlobClient();
           CloudBlobContainer container = blobClient.GetContainerReference("2aabb");
          // IEnumerable<CloudBlobContainer> cbc = blobClient.ListContainers();
           string s = container.Name;
          
          return s; 
       }


    //   static UInt32 LoaderCount(string qnnee) //devstor for dev
    //   { 
    // CloudStorageAccount.
     //  }
    }
}
