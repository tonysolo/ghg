using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MvvmLight1.Model
{
    public static class Azure
    {
        public static CloudStorageAccount GhgAccount =
            CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("GHGConnectionString"));


        public static string[] CountryNames()
        {
            CloudBlobClient cbc = GhgAccount.CreateCloudBlobClient();
            CloudBlobContainer container = cbc.GetContainerReference("countries");
            IListBlobItem[] blobs = container.ListBlobs().ToArray();
            var sarr = new string[blobs.Length];
            for (int i = 0; i < sarr.Length; i++)
                sarr[i] = blobs[i].Uri.Segments[2].Substring(0, 2).ToUpper();
            return sarr;
        }


        public static string[] GetRegions(string countryname)
        {
            if (countryname == null) return null;
            CloudBlobClient cbc = GhgAccount.CreateCloudBlobClient();
            CloudBlobContainer container = cbc.GetContainerReference("countries");
            //if (Selectedcountryindex < 0) return null;
            // var sb = new StringBuilder(CountryNames[Selectedcountryindex]);
            // sb.Append(".txt");
            string countryblobname = countryname + ".txt";
            CloudBlockBlob blob = container.GetBlockBlobReference(countryblobname.ToLower());

            string[] sarr = null;
            if (blob == null) return null;
            var ms = new MemoryStream();
            blob.DownloadToStream(ms);
            byte[] s = ms.GetBuffer();
            string str = Encoding.UTF8.GetString(s);
            str = str.Trim('\0');
            sarr = str.Split(',');
            for (int i = 0; i < sarr.Length; i++)
            {
                char[] carr = sarr[i].ToCharArray();
                carr = Array.FindAll(carr, (c => (char.IsLetterOrDigit(c))));
                sarr[i] = new string(carr);
            }
            return sarr;
        }
    }
}