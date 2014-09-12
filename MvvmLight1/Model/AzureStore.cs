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
       public static void SetupAzureAccount()
       {
           Model.settings.registered = true;
           CloudStorageAccount account = Microsoft.WindowsAzure.Storage.CloudStorageAccount.DevelopmentStorageAccount;
           IEnumerable<CloudBlobContainer> en = account.CreateCloudBlobClient().ListContainers();
           CloudBlobContainer container = en.ElementAt<CloudBlobContainer>(0);
           CloudPageBlob loader = container.GetPageBlobReference("l");
           string s = container.Name;
           loader.Create(8192);
           string s1 = "Tony Manicom/n173 blandford road/n north riding, Randburg/n";
           byte[] sb = Encoding.UTF8.GetBytes(s1);

           // byte[] ba = new byte[512];
           int grow = 512 - ((sb.Length) % 512);
           Array.Resize(ref sb, sb.Length + grow);
           //  for (int i = 0; i < sb.Length; i++) ba[i] = sb[i];      
           //ba.
           // byte[] x = new byte[512];
           // for (int i = 0; i < 512; i++) x[i] = (byte)i;
           loader.UploadFromByteArray(sb, 0, 512);

           CloudPageBlob readerblob = container.GetPageBlobReference("l");
           System.IO.Stream stream = readerblob.OpenRead();
           byte[] buffer = new byte[512];
           stream.Seek(0, System.IO.SeekOrigin.Begin);
           stream.Read(buffer, 0, 512);
           //Model.AzureStorage.devListContainers();     
       }


       public static IEnumerable<CloudBlobContainer> DevelopmentContainers()
       {
           CloudStorageAccount dev = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
           CloudBlobClient blobClient = dev.CreateCloudBlobClient();
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
