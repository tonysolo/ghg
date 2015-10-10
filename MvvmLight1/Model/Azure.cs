using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Shared;

namespace MvvmLight1.Model
{
    public  class Azure
    {
        // static string accountName = "devstoreaccount1";
        //  static string accountKey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";
        // static Microsoft.WindowsAzure.Storage.Auth.StorageCredentials creds = new StorageCredentials(accountName, accountKey);
        // static CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);

        // public static CloudStorageAccount Account = 
        //  CloudStorageAccount(System.Configuration.AppSettingsReader("GHGConnectionString"));
        static string accountName = "ghg";
        static string accountKey = "38Y8V0konokJ4aNWUJMzKJFrzKPh1t2uLqQRABXA3/oLy0EXPxmApIDJYuiD2gF8sPyH0J2skG/0i1V3GhxMtQ==";
        //string accountName = "devstoreaccount1";
        //string accountKey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";

        static StorageCredentials creds = new StorageCredentials(accountName, accountKey);
        static CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);



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
            CloudBlobClient cbc = account.CreateCloudBlobClient();
            CloudBlobContainer container = cbc.GetContainerReference("countries");
            IListBlobItem[] blobs = container.ListBlobs().ToArray();
            var sarr = new string[blobs.Length];
            for (int i = 0; i < sarr.Length; i++)
                sarr[i] = blobs[i].Uri.Segments[2].Substring(0,2).ToUpper();
            return sarr;
        }

        private static bool IsHex(IEnumerable<char> chars)
        {
            bool isHex;
            foreach (var c in chars)
            {
                isHex = ((c >= '0' && c <= '9') ||
                         (c >= 'a' && c <= 'f') ||
                         (c >= 'A' && c <= 'F'));

                if (!isHex)
                    return false;
            }
            return true;
        }


        public static string[] GetRegions(string countryname)
        {
            if (countryname == null) return null;
            CloudBlobClient cbc = account.CreateCloudBlobClient();
            CloudBlobContainer container = cbc.GetContainerReference("countries");

            string countryblobname = countryname + ".txt";
            // string countryblobname = "za.txt";
            CloudBlockBlob blob = container.GetBlockBlobReference(countryblobname.ToLower());
            if (blob == null) return null;
            var ms = new MemoryStream();
            blob.DownloadToStream(ms);
            byte[] s = ms.GetBuffer();
            string str = Encoding.UTF8.GetString(s);
            str = str.Trim('\0');
            string[] sarr = str.Split(',');
            foreach (string st in sarr)
            { if (st.Length == 6) st.Remove(0, 1); }       
            return sarr;
        }
    }
}