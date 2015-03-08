using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;


namespace MvvmLight1.Model
{
    public static class Userdata //static class current usewr data -- will lookup azure utilities and keep a session copy
    {
       // public static CloudStorageAccount GhgAccount =
       //     CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("GHGConnectionString"));

        public static int Selectedcountryindex;

        public static CloudBlobContainer Container;

        public static string SelectedCountry { get { return CountryNames[Selectedcountryindex]; }}

        public static string[] CountryNames { get {return Azure.CountryNames();}}

        public static string Region { get; set; }

       public static string Country
       {
           get { return SelectedCountry; }
        }
    
     

        public static string[] Regions = Azure.GetRegions(Country);
/*
        public static string[] GetRegions()
        {
            var cbc = GhgAccount.CreateCloudBlobClient();
            var container = cbc.GetContainerReference("countries");
            if (Selectedcountryindex < 0) return null;
            var sb = new StringBuilder(CountryNames[Selectedcountryindex]);
            sb.Append(".txt");
            var countryblobname = sb.ToString();
            var blob = container.GetBlockBlobReference(countryblobname.ToLower());
           
            string[] sarr = null;
            if (blob == null) return null;
            var ms = new MemoryStream();
            blob.DownloadToStream(ms);
            var s = ms.GetBuffer();
            var str = Encoding.UTF8.GetString(s);
            str = str.Trim('\0');
            sarr = str.Split(',');
            for (var i=0;i<sarr.Length;i++)
            {
                var carr = sarr[i].ToCharArray();
                carr = Array.FindAll<char>(carr, (c => (char.IsLetterOrDigit(c))));
                sarr[i] = new string(carr);
            }
            return sarr;
            
        }
*/
        public static bool Isvalid(string qnnee)
        {
            return Regions.Contains(qnnee);
        }


    }
}