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
       public static string[] devListContainers()
       {
           string[] saa = new string[3];
           CloudStorageAccount dev = CloudStorageAccount.Parse
               (
               CloudConfigurationManager.
               GetSetting("StorageConnectionString")
               );

           CloudBlobClient blobClient = dev.CreateCloudBlobClient();
           CloudBlobContainer container = blobClient.GetContainerReference("2aabb");
           string s = blobClient.ListContainers().ToString();
          
           return saa; 
       }


    //   static UInt32 LoaderCount(string qnnee) //devstor for dev
    //   { 
    // CloudStorageAccount.
     //  }
    }
}
