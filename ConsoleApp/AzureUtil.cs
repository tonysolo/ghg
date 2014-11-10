using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

//http://gauravmantri.com/

namespace ConsoleApp
{

    public static class AzureStorage
    {
        public static void SetupAzureAccount()
        {
            CloudStorageAccount.Parse();
            //Settings.Registered = true;
            var account = CloudStorageAccount.DevelopmentStorageAccount;
           // var en = account.CreateCloudBlobClient().ListContainers();
            var container = account.CreateCloudBlobClient().GetContainerReference("2aabb");
            var loader = container.GetPageBlobReference("l");
            if (loader == null) { loader.Create(8192); }
          //  {
                
                // var s = container.Name;
          
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
          //  }

            var readerblob = container.GetPageBlobReference("l");
            var stream = readerblob.OpenRead();
            var buffer = new byte[512];
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            stream.Read(buffer, 0, 512);
            var s = Encoding.UTF8.GetString(buffer).Trim('\0');
            var v = "";
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
    



    //CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
    //    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient(); 
    //    CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName); 
    //    string filePath = "<Full File Path e.g. C:\temp\myblob.txt>"; 
    //    string blobName = "<Blob Name e.g. myblob.txt>"; 
    //   CloudBlockBlob blob = container.GetBlockBlobReference(blobName); 

    

        /// <summary>
        /// Gets all the name names for the GHG management account
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IListBlobItem> DownloadCountryNames()
        {
            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("GHGConnectionString"));
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("countries");
            return container.ListBlobs().AsEnumerable();

        }


        /// <summary>
        /// Downloads Regions from the GHG management account
        /// </summary>
        /// <param name="name">eg "za.txt"</param>,
        /// <returns>CSV string</returns>
        public static string DownloadNames(string name)
        {
            name = name.ToLower() + ".txt";
            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("GHGConnectionString"));
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("countries");
            var blockBlob = container.GetBlockBlobReference(name);
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
            var container = blobClient.GetContainerReference("countries");
            return container.GetBlockBlobReference("countries.txt").DownloadText().ToUpper().Split(',');
        }

    

    private static void Main()
        {
        
            AzureStorage.SetupAzureAccount();
            
        }

    }
}

