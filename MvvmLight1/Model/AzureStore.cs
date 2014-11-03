using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure;


namespace MvvmLight1.Model
{
   public static class AzureStorage
    {
       public static void SetupAzureAccount()
       {
           Settings.Registered = true;
           var account = CloudStorageAccount.DevelopmentStorageAccount;
           var en = account.CreateCloudBlobClient().ListContainers();
           var container = en.ElementAt(0);
           var loader = container.GetPageBlobReference("l");
           var s = container.Name;
           loader.Create(8192);
           const string s1 = "Tony Manicom/n173 blandford road/n north riding, Randburg/n";
           var sb = Encoding.UTF8.GetBytes(s1);

           // byte[] ba = new byte[512];
           var grow = 512 - ((sb.Length) % 512);
           Array.Resize(ref sb, sb.Length + grow);
           //  for (int i = 0; i < sb.Length; i++) ba[i] = sb[i];      
           //ba.
           // byte[] x = new byte[512];
           // for (int i = 0; i < 512; i++) x[i] = (byte)i;
           loader.UploadFromByteArray(sb, 0, 512);

           var readerblob = container.GetPageBlobReference("l");
           var stream = readerblob.OpenRead();
           var buffer = new byte[512];
           stream.Seek(0, System.IO.SeekOrigin.Begin);
           stream.Read(buffer, 0, 512);
           //Model.AzureStorage.devListContainers();     
       }


       public static IEnumerable<CloudBlobContainer> DevelopmentContainers()
       {
           var dev = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
           var blobClient = dev.CreateCloudBlobClient();
           return blobClient.ListContainers();
           //ToArray<
           //CloudBlobContainer container = blobClient.GetContainerReference("2aabb");
          // IEnumerable<CloudBlobContainer> cbc = blobClient.ListContainers();
           //string s = container.Name;        
          //return s; 
       }


    //   static UInt32 LoaderCount(string qnnee) //devstor for dev
    //   { 
    // CloudStorageAccount.
     //  }
    }
}
