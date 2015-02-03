using System;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MvvmLight1.Model
{
    public static class Userdata
    {
        public static CloudStorageAccount GhgAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("GHGConnectionString"));
        public static string[] CountryNames { get; set; }
        public static string Country { get; set; }
        public static string CentreRegion { get; set; }
        public static string[] Regions { get; set; }
        public static string SelectedQnnee { get; set; }
        public static int Selectedcountryindex;
        public static int Selectedregionindex;
        public static CloudBlobContainer Container;
       // public static string Txt;

        public static void LoadCountryNames()
        {
            Container = GhgAccount.CreateCloudBlobClient().GetContainerReference("countries");
            
            var blob = Container.GetBlockBlobReference("countries.txt");
            var countries = blob.DownloadText(Encoding.UTF8);
            CountryNames = countries.Split(',');
            Selectedcountryindex = -1;
           
        }

        public static void LoadRegions()
        {
            if (Selectedcountryindex <= -1) return;
            var countryblobname = String.Format(@"{0}{1}",
                CountryNames[Selectedcountryindex].ToLower(),".txt");          
            var blob = Container.GetBlockBlobReference(countryblobname);
            var txt = blob.DownloadText(Encoding.UTF8);
            Regions = txt.Split(',');
            Selectedregionindex = 0;
        }

        public static bool Isvalid(string qnnee)
        {
            return Regions.Contains(qnnee);
        }
    }
}