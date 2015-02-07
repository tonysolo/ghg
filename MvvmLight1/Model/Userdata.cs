﻿using System.IO;
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

        public static string Country { get; set; }
        public static string[] Regions { get; set; }
        public static string Region { get; set; }
        public static string CentreRegion { get; set; }
        public static string SelectedQnnee { get; set; }
        public static int Selectedcountryindex;
        public static int Selectedregionindex;
        public static CloudBlobContainer Container;



        public static string[] CountryNames
        {
            get
            {
                var cbc = GhgAccount.CreateCloudBlobClient();
                var container = cbc.GetContainerReference("countries");
                var blobs = container.ListBlobs().ToArray();
                var sarr = new string[blobs.Length];
                for (var i = 0; i < sarr.Length; i++)
                    sarr[i] = blobs[i].Uri.Segments[2].Substring(0, 2).ToUpper();
                return sarr;
            }
        }



        public static void LoadRegions()
        {
            var cbc = GhgAccount.CreateCloudBlobClient();
            var container = cbc.GetContainerReference("countries");
            if (Selectedcountryindex < 0) return;
            var sb = new StringBuilder(CountryNames[Selectedcountryindex]);
            sb.Append(".txt");
            var countryblobname = sb.ToString();
            var blob = container.GetBlockBlobReference(countryblobname.ToLower());
            var ms = new MemoryStream();
            if (blob != null)
            {
                blob.DownloadToStream(ms);
                ms.Position = 0;
            }
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