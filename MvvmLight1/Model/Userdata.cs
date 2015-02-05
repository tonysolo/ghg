using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var ms = new MemoryStream();
           // blob.
            blob.DownloadToStream(ms);
            var ba = ms.GetBuffer();
            var str = Encoding.UTF8.GetString(ba, 0, ba.Length);
            str = str.Trim('\0');
            //var s = blob.DownloadText(Encoding.UTF8);
            CountryNames = str.Split(',');
           
            Selectedcountryindex = -1;          
        }

       

        public static void LoadRegions()
        {
            if (Selectedcountryindex <= -1) return;
            var countryblobname = String.Format(@"{0}{1}",
                CountryNames[Selectedcountryindex].ToLower(),".txt");          
            var blob = Container.GetBlockBlobReference(countryblobname);

            var ms = new MemoryStream();
            var ba = ms.GetBuffer();
            var str = Encoding.UTF8.GetString(ba, 0, ba.Length);
            str = str.Trim('\0');     
            Regions = str.Split(',');

        }

        public static bool Isvalid(string qnnee)
        {
            return Regions.Contains(qnnee);
        }
    }
}