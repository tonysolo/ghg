using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

//http://gauravmantri.com/
using Microsoft.WindowsAzure.Storage.Blob.Protocol;

namespace ConsoleApp
{

    public static class AzureStorage
    {

        private static CloudStorageAccount csa = CloudStorageAccount.DevelopmentStorageAccount;
        //private static CloudStorageAccount csa = CloudStorageAccount.Parse("GHGConnectionString");


        private static int startoffset = 5120;

        //public static string Container { get; set; }

        public static void SetupLoaderBlob(string qnnee) // fixed length records
        {          
            var account = csa; //CloudStorageAccount.DevelopmentStorageAccount;
            // var en = account.CreateCloudBlobClient().ListContainers();
            var cont = account.CreateCloudBlobClient().GetContainerReference(qnnee); //"2aabb"
            var loader = cont.GetPageBlobReference("l");
            if (loader == null) loader.Create(0x40000000); //for development but need to increase in production
            loader.FetchAttributes();
            loader.Metadata.Add("nextindex", "0x00");
            loader.Metadata["nextpage"] = "0x00";
            // loader.Metadata.Add("dataoffset", "5120"); //no index - fixed length record +-2 kilobytes for prefs
            loader.Properties.ContentEncoding = "application/octet-stream";
            loader.SetMetadata();
            loader.SetProperties();
        }



        public static void SetupPatientBlob(string qnnee)
            //variable length records - index/size = 8 bytes in index table
        {
            //CloudStorageAccount.Parse();
            //Settings.Registered = true;
            var account = csa;
            // var en = account.CreateCloudBlobClient().ListContainers();
            var cont = account.CreateCloudBlobClient().GetContainerReference(qnnee); //"2aabb"
            var patient = cont.GetPageBlobReference("p");
            if (patient == null) patient.Create(0x40000000); //for development need to increase this to fill whole blob
            //patient.Metadata.Add("nextindex", "0");
            patient.Metadata["nextindex"] = "0x00";
            patient.Metadata["nextpage"] = "0x00";
            patient.Metadata.Add("startoffset", "0x800000"); //constant
            patient.Properties.ContentEncoding = "application/octet-stream";
            patient.SetMetadata();
            patient.SetProperties();
        }


        public static void SetupImageBlob(string qnnee)
            //variable length records - index/size = 8 bytes in index table
        {
            //CloudStorageAccount.Parse();
            //Settings.Registered = true;
            var account = csa;
            // var en = account.CreateCloudBlobClient().ListContainers();
            var cont = account.CreateCloudBlobClient().GetContainerReference(qnnee); //"2aabb"
            var image = cont.GetPageBlobReference("i");
            if (image == null) image.Create(0x40000000); //for development need to increase this to fill whole blob
            //image.Metadata.Add("nextindex", "0");
            image.Metadata["nextindex"] = "0x00";
            image.Metadata["nextpage"] = "0x00";
            image.Metadata.Add("startoffset", "0x800000"); //constant
            image.Properties.ContentEncoding = "application/octet-stream";
            image.SetMetadata();
            image.SetProperties();
        }


        public static void SetupEpidemiologyBlob(string qnnee)
            //variable length records - index/size = 8 bytes in index table
        {
            //CloudStorageAccount.Parse();
            //Settings.Registered = true;
            var account = csa;
            // var en = account.CreateCloudBlobClient().ListContainers();
            var cont = account.CreateCloudBlobClient().GetContainerReference(qnnee); //"2aabb"
            cont.CreateIfNotExists();
            
            var epidem = cont.GetPageBlobReference("e");
            epidem.Create(0x40000000);        
           // epidem.Create(0xffff); //for development need to increase this to fill whole blob
           // epidem.FetchAttributes();
            //epidem.Metadata.Add("nextindex", "0");
            epidem.Metadata["nextindex"] = "0x00";
            epidem.Metadata["nextpage"] = "0x120";
            epidem.Metadata["startoffset"] = "0x24000"; //constant
            epidem.Properties.ContentEncoding = "application/octet-stream";
           // epidem.SetMetadata();
           // epidem.SetProperties();
        }



        public static void RegisterLoader(string[] ldr, Encoding enc)
        {
            var json = JsonConvert.SerializeObject(ldr);          
            var account = csa; //CloudStorageAccount.DevelopmentStorageAccount;          
            var cont = account.CreateCloudBlobClient().GetContainerReference(ldr[0]); //"2aabb"
            var loader = cont.GetPageBlobReference("l");
            var bytes = enc.GetBytes(json);
            var grow = (bytes.Length % 512);
            Array.Resize(ref bytes,512-grow);
            loader.UploadFromByteArray(bytes,0,bytes.Length);         
        }

    }

    public class test
    {
        public static void  Main()
        {
            AzureStorage.SetupEpidemiologyBlob("2aabb");
        }
    }
}



//   public staticvoid Main()
//{
//}
    /*
                
                // var s = qnnee.Name;
          
                const string s1 = "Tony Manicom/n173 blandford road/n north riding, Randburg/n";
                var sb = Encoding.UTF8.GetBytes(s1);

                // byte[] ba = new byte[512];
                var grow = 512 - (sb.Length) % 512;
                Array.Resize(ref sb, sb.Length + grow);
                //  for (int i = 0; i < sb.Length; i++) ba[i] = sb[i];      
                //ba.
                // byte[] x = new byte[512];
                // for (int i = 0; i < 512; i++) x[i] = (byte)i;
             //   loader.UploadFromByteArray(sb, 0, 512);
            }

           // var readerblob = qnnee.GetPageBlobReference("l");
          // var stream = readerblob.OpenRead();
           // var buffer = new byte[512];
           // stream.Seek(0, System.IO.SeekOrigin.Begin);
          //  stream.Read(buffer, 0, 512);
          //  var s = Encoding.UTF8.GetString(buffer).Trim('\0');
           // var v = "";
            //Model.AzureStorage.devListContainers();     
        }


       // public static IEnumerable<CloudBlobContainer> DevelopmentContainers()
       // {
           // var dev = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
           // var blobClient = dev.CreateCloudBlobClient();
           // return blobClient.ListContainers();
            //ToArray<
            //CloudBlobContainer qnnee = blobClient.GetContainerReference("2aabb");
            // IEnumerable<CloudBlobContainer> cbc = blobClient.ListContainers();
            //string s = qnnee.Name;        
            //return s; 
       // }


        //   static UInt32 LoaderCount(string qnnee) //devstor for dev
        //   { 
        // CloudStorageAccount.
        //  }
    



    //CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
    //    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient(); 
    //    CloudBlobContainer qnnee = cloudBlobClient.GetContainerReference(containerName); 
    //    string filePath = "<Full File Path e.g. C:\temp\myblob.txt>"; 
    //    string blobName = "<Blob Name e.g. myblob.txt>"; 
    //   CloudBlockBlob blob = qnnee.GetBlockBlobReference(blobName); 

    

        /// <summary>
        /// Gets all the name names for the GHG management account
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IListBlobItem> DownloadCountryNames()
        {
            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("GHGConnectionString"));
            var blobClient = storageAccount.CreateCloudBlobClient();
            var qnnee = blobClient.GetContainerReference("countries");
            return qnnee.ListBlobs().AsEnumerable();

        }


        /// <summary>
        /// Downloads Regions from the GHG management account
        /// </summary>
        /// <param name="name">eg "za.txt"</param>,
        /// <returns>CSV string</returns>
      //  public static string DownloadNames(string name)
       // {
            name = name.ToLower() + ".txt";
            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("GHGConnectionString"));
            var blobClient = storageAccount.CreateCloudBlobClient();
            var qnnee = blobClient.GetContainerReference("countries");
            var blockBlob = qnnee.GetBlockBlobReference(name);
            string text;
            using (var memoryStream = new MemoryStream())
            {
                blockBlob.DownloadToStream(memoryStream);
                text = Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            return text;
        }

        public static string[] CountryNames()
        {
            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("GHGConnectionString"));
            var blobClient = storageAccount.CreateCloudBlobClient();
            var qnnee = blobClient.GetContainerReference("countries");
            return qnnee.GetBlockBlobReference("countries.txt").DownloadText().ToUpper().Split(',');
        }

    

   
     */
//}



