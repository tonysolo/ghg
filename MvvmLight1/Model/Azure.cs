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


        /// <summary>
        /// registers a new loader
        /// </summary>
        /// <param name="json"></param>
        /// <returns>string index</returns>
     /*   public static int RegisterNewLoader(string json)
        {
            GhgAccount = "GHGConnetionString";
            var bytes = Encoding.UTF8.GetBytes(json);
            var grow = (512 - bytes.Length % 512);
            Array.Resize(ref bytes, bytes.Length + grow);
            var s = SetNextLoaderIndex();
            if (s == -1) return s;
            // var p = Convert.ToInt32(s, 16);
            var ms = new MemoryStream(bytes);
            var start = s << 10;
            Loaderblob.WritePages(ms, start);
            return s;
        }
*/

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
            //if (SelectedCountryIndex < 0) return null;
            // var sb = new StringBuilder(CountryNames[SelectedCountryIndex]);
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