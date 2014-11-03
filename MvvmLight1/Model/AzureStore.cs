using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure;
using Newtonsoft.Json;
using Newtonsoft;
using Newtonsoft.Json.Converters;


namespace MvvmLight1.Model
{
   public static class AzureStorage
   {
      
       

       public static string Loaderid { get; set; }

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

       public static void UploadLoaderData(string[] sarr)
       {
           var qne = Loaderid.Substring(0, 5);
           var pagenumber = Convert.ToInt16(Loaderid.Substring(5, Loaderid.Length - 5));
           var startoffset = pagenumber << 9;
           var s = JsonConvert.SerializeObject(sarr);
           var a = JsonConvert.DeserializeObject<string[]>(s);
           var saa = (string[])a;
           //convert sarr to json string then byte[]
           //grow by size mod 512
           //upload the 512 byte[]
       }

       public static string[] DownloadData()
       {
           var qne = Loaderid.Substring(0, 5);
           var pagenumber = Convert.ToInt16(Loaderid.Substring(5, Loaderid.Length - 5));
           var startoffset = pagenumber << 9;
           var endoffset = (pagenumber << 10) - 1;
           return new string[];
       }


       //   static UInt32 LoaderCount(string qnnee) //devstor for dev
    //   { 
    // CloudStorageAccount.
     //  }
    }
}
