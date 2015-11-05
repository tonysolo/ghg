using System.IO;
using System.Text;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;

namespace MvvmLight1.Model
{
    public class Azure
    {
        // static string accountName = "devstoreaccount1";
        //  static string accountKey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";
        // static Microsoft.WindowsAzure.Storage.Auth.StorageCredentials creds = new StorageCredentials(accountName, accountKey);
        // static CloudStorageAccount Account = new CloudStorageAccount(creds, useHttps: true);

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
        //18/10/2015 decided to keep this data local to save reading azure data country list thats always needed on starup 
        //and seldom changes - records country code | page offset | data length. Perhaps I will put it in azure later
        public static string[] CountryData =
            {
            "ZA|0|1607",
            "GB|4|940",
            "LS|6|53",
            "SZ|7|47",
            "SL|8|131" };

        public static string[] CountryNames()
        {

            var sarr = new string[CountryData.Length];
            for (int i = 0; i < sarr.Length; i++)
            {
                sarr[i] = CountryData[i].Split('|')[0];
            }
            return sarr;
        }

        public static int[] Offset_and_Length(string Country)
        {
            var ol = new int[2];
            var sarr = new string[CountryData.Length];
            for (int i = 0; i < sarr.Length; i++)
            {
                var dat = CountryData[i].Split('|');
                if (dat[0] == Country)
                {
                    ol[0] = System.Convert.ToInt16(dat[1]) * 512;
                    ol[1] = System.Convert.ToInt16(dat[2]);
                    break;
                }
            }
            return ol;
        }


        /*
                    CloudBlobClient cbc = Account.CreateCloudBlobClient();
                    CloudBlobContainer Container = cbc.GetContainerReference("countries");
                    IListBlobItem[] blobs = Container.ListBlobs().ToArray();
                    var sarr = new string[blobs.Length];
                    for (int i = 0; i < sarr.Length; i++)
                        sarr[i] = blobs[i].Uri.Segments[2].Substring(0,2).ToUpper();
                    return sarr;
                    */


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
            var x = Offset_and_Length(countryname);
            var cbc = account.CreateCloudBlobClient();
            var container = cbc.GetContainerReference("countries");
            var blob = container.GetPageBlobReference("global");
            var ba = new byte[x[1]];
            blob.DownloadRangeToByteArray(ba, 0, x[0], x[1]);
            var str = Encoding.UTF8.GetString(ba);
            var sarr = str.Split(',');
            foreach (var st in sarr)
            { if (st.Length == 6) st.Remove(0, 1); }     //sort this bug 
            return sarr;
        }
    }
}