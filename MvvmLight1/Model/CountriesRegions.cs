using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;

namespace MvvmLight1.Model
{
    public static class CountriesRegions
    {
        // static string AccountName = "devstoreaccount1";
        //  static string AccountKey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";

        //static CloudStorageAccount _storageAccount 

        private static readonly CloudStorageAccount Account = CloudStorageAccount.Parse(
           CloudConfigurationManager.GetSetting("GHGConnectionString"));
     
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
            for (var i = 0; i < sarr.Length; i++)
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