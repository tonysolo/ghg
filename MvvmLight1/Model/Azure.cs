using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;

namespace MvvmLight1.Model
{
    public class Azure
    {
        // static string AccountName = "devstoreaccount1";
        //  static string AccountKey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";
        //string accountName = "ghg";
        //string accountKey = "38Y8V0konokJ4aNWUJMzKJFrzKPh1t2uLqQRABXA3/oLy0EXPxmApIDJYuiD2gF8sPyH0J2skG/0i1V3GhxMtQ==";
        // static Microsoft.WindowsAzure.Storage.Auth.StorageCredentials Creds = new StorageCredentials(AccountName, AccountKey);
        // static CloudStorageAccount Account = new CloudStorageAccount(Creds, useHttps: true);
        // public static CloudStorageAccount Account = 
        //  CloudStorageAccount(System.Configuration.AppSettingsReader("GHGConnectionString"));
       
            //private const string AccountName = "ghg";
        //private const string AccountKey = "38Y8V0konokJ4aNWUJMzKJFrzKPh1t2uLqQRABXA3/oLy0EXPxmApIDJYuiD2gF8sPyH0J2skG/0i1V3GhxMtQ==";

        //string AccountName = "devstoreaccount1";
        //string AccountKey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";

       // private static readonly StorageCredentials Creds = new StorageCredentials(AccountName, AccountKey);
       // private static readonly CloudStorageAccount Account = new CloudStorageAccount(Creds, useHttps: true);
        private static readonly CloudStorageAccount Account = Global.Setcountryaccount("ghg");


        /// <summary>
        /// registers a new loader
        /// </summary>
        // <param name="json"></param>
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
        //and seldom changes - records country code | page offset | data length. Perhaps I will put it in azure later.
        //also saved in 
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

        public static int[] Offset_and_Length(string country)
        {
            var ol = new int[2];
            var sarr = new string[CountryData.Length];
            for (var i = 0; i < sarr.Length; i++)
            {
                var dat = CountryData[i].Split('|');
                if (dat[0] == country)
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
            if (chars.Select(c => ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F'))).All(isHex => isHex)) return true;
            return false;
        }

//this is slow - need to store this as static data - ie a separate program for each country
        public static string[] GetRegions(string countryname)
        {
            if (countryname == null) return null;
            var x = Offset_and_Length(countryname);
            var cbc = Account.CreateCloudBlobClient();
            var container = cbc.GetContainerReference("countries");
            var blob = container.GetPageBlobReference("global");
            var ba = new byte[x[1]];
            blob.DownloadRangeToByteArray(ba, 0, x[0], x[1]);
            var str = Encoding.UTF8.GetString(ba);
            var sarr = str.Split(',');
            var sarr1 = new string[sarr.Length];
            for (var i = 0; i < sarr.Length; i++) //remove extra characters     
                sarr1[i] = (sarr.Length == 6) ? sarr[i].Remove(0, 1) : sarr[i];
            return sarr1;
        }
    }
}