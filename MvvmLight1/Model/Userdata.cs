using System.IO;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MvvmLight1.Model
{
    public static class Userdata
    {
        public static CloudStorageAccount GhgAccount =
            CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("GHGConnectionString"));

        public static int Selectedcountryindex;
        public static CloudBlobContainer Container;
        public static string[] Regions { get; set; }
        public static string SelectedQnnee { get; set; }

        public static string[] CountryNames
        {
            get
            {
                CloudBlobClient cbc = GhgAccount.CreateCloudBlobClient();
                CloudBlobContainer container = cbc.GetContainerReference("countries");
                IListBlobItem[] blobs = container.ListBlobs().ToArray();
                var sarr = new string[blobs.Length];
                for (int i = 0; i < sarr.Length; i++)
                    sarr[i] = blobs[i].Uri.Segments[2].Substring(0, 2).ToUpper();
                return sarr;
            }
        }


        public static void GetRegions()
        {
            CloudBlobClient cbc = GhgAccount.CreateCloudBlobClient();
            CloudBlobContainer container = cbc.GetContainerReference("countries");
            if (Selectedcountryindex < 0) return;
            var sb = new StringBuilder(CountryNames[Selectedcountryindex]);
            sb.Append(".txt");
            string countryblobname = sb.ToString();
            CloudBlockBlob blob = container.GetBlockBlobReference(countryblobname.ToLower());
            var ms = new MemoryStream();
            string txt ="";
            if (blob != null)
            {
                txt= blob.DownloadText(Encoding.UTF8);
                blob.DownloadToStream(ms);
                ms.Position = 0;
            }
            byte[] ba = ms.GetBuffer();
            byte[] uniBytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, ba);


            string str = Encoding.Unicode.GetString(uniBytes);
            Regions = txt.Split(',');
            for (int i = 0; i < Regions.Length; i++) Regions[i].Trim();
            var q = Regions[0].Trim().Substring(0, 2);
            //var str = Encoding.UTF8.GetString(ba, 0, ba.Length);
        }

        public static bool Isvalid(string qnnee)
        {
            return Regions.Contains(qnnee);
        }
    }
}