using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;

//http://gauravmantri.com/

namespace ConsoleApp
{
    //CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
    //    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient(); 
    //    CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName); 
    //    string filePath = "<Full File Path e.g. C:\temp\myblob.txt>"; 
    //    string blobName = "<Blob Name e.g. myblob.txt>"; 
    //   CloudBlockBlob blob = container.GetBlockBlobReference(blobName); 

    public static class AzureUtil
    {
        /// <summary>
        /// Downloads Regions
        /// </summary>
        /// <param name="country">eg "za.txt"</param>,
        /// <returns>CSV string</returns>
        public static string DownloadRegions(string country)
        {
            country = country.ToLower() + ".txt";
            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("GHGConnectionString"));
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("countries");
            var blockBlob = container.GetBlockBlobReference(country);
            string text;
            using (var memoryStream = new MemoryStream())            
            {
                blockBlob.DownloadToStream(memoryStream);
                text = Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            return text;
        }


        private static void Main()
        {
            var s = AzureUtil.DownloadRegions("ZA");
            var sw = AzureUtil.DownloadRegions("SZ");
            var ls = AzureUtil.DownloadRegions("LS");
        }

    }
}

