using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MvvmLight1.Model
{
   public static class AzureStorage
   {
      
 
       //public static string Loaderid { get; set; }

    //   public static void SetupAzureAccount()
   //    {
          // Settings.Registered = true;
  //         var account = CloudStorageAccount.DevelopmentStorageAccount;
          
  //         var contain = account.CreateCloudBlobClient().GetContainerReference("2aabb");
  //         var tony = contain.GetPageBlobReference("blob");
          // var s = container.Name;
//           tony.Create(8192);
 //          const string s1 = "Tony Manicom/n173 blandford road/n north riding, Randburg/n";
//           var sb = Encoding.UTF8.GetBytes(s1);

//           // byte[] ba = new byte[512];
//           var grow = 512 - ((sb.Length) % 512);
//           Array.Resize(ref sb, sb.Length + grow);
           //  for (int i = 0; i < sb.Length; i++) ba[i] = sb[i];      
           //ba.
           // byte[] x = new byte[512];
           // for (int i = 0; i < 512; i++) x[i] = (byte)i;
          // loader.UploadFromByteArray(sb, 0, 512);

         //  var readerblob = container.GetPageBlobReference("l");
         //  var stream = readerblob.OpenRead();
  //         var buffer = new byte[512];
         //  stream.Seek(0, System.IO.SeekOrigin.Begin);
         //  stream.Read(buffer, 0, 512);
           //Model.AzureStorage.devListContainers();     
//       }


       //public static IEnumerable<CloudBlobContainer> DevelopmentContainers()
      // {
       //    var dev = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
     //      var blobClient = dev.CreateCloudBlobClient();
      //     return blobClient.ListContainers();
           //ToArray<
           //CloudBlobContainer container = blobClient.GetContainerReference("2aabb");
          // IEnumerable<CloudBlobContainer> cbc = blobClient.ListContainers();
           //string s = container.Name;        
          //return s; 
     //  }

     //  public static void UploadLoaderData(string[] sarr)
     //  {
     //      var qne = Loaderid.Substring(0, 5);
    //       var pagenumber = Convert.ToInt16(Loaderid.Substring(5, Loaderid.Length - 5));
     //      var startoffset = pagenumber << 9;
    //       var s = JsonConvert.SerializeObject(sarr);
    //       var a = JsonConvert.DeserializeObject<string[]>(s);
           //var saa = (string[])a;
          
  //         var account = CloudStorageAccount.DevelopmentStorageAccount;
           // var en = account.CreateCloudBlobClient().ListContainers();

   //        account.CreateCloudBlobClient().GetContainerReference("qnnee").CreateIfNotExists();

   //        var container = account.CreateCloudBlobClient().GetContainerReference("qnnee");
           // var container = en.ElementAt(0);
           //string blobname = "tony/manicom";
 //          var loader = container.GetPageBlobReference("/l");        
 //          loader.Create(8192);
           //const string s1 = "Tony Manicom/n173 blandford road/n north riding, Randburg/n";
   //        var sb = Encoding.UTF8.GetBytes(s);

           // byte[] ba = new byte[512];
  //         var grow = 512 - ((sb.Length) % 512);
 //          Array.Resize(ref sb, sb.Length + grow);
           //  for (int i = 0; i < sb.Length; i++) ba[i] = sb[i];      
           //ba.
           // byte[] x = new byte[512];
           // for (int i = 0; i < 512; i++) x[i] = (byte)i;
//           loader.UploadFromByteArray(sb, 0, 512);

 //          var readerblob = container.GetPageBlobReference("/l");
 //          var stream = readerblob.OpenRead();
 //          var buffer = new byte[512];
  //         stream.Seek(0, System.IO.SeekOrigin.Begin);
 //          stream.Read(buffer, 0, 512);


 //          var text = Encoding.UTF8.GetString(buffer);
           //Model.AzureStorage.devListContainers();    

//       }

      // public static string[] DownloadData()
      // {
      //     var qne = Loaderid.Substring(0, 5);
       //    var pagenumber = Convert.ToInt16(Loaderid.Substring(5, Loaderid.Length - 5));
       //    var startoffset = pagenumber << 9;
       //    var endoffset = (pagenumber << 10) - 1;
        //   return new string[1];
     //  }


       //   static UInt32 LoaderCount(string qnnee) //devstor for dev
    //   { 
    // CloudStorageAccount.
     }
  }
