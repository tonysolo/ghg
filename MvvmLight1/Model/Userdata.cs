using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using Microsoft.WindowsAzure.Storage.Blob;
using MvvmLight1.Model;

namespace MvvmLight1.Model
{
    public static class Userdata
    {
        public static CloudStorageAccount GhgAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("GHGConnectionString"));
        public static string[] CountryNames { get; private set; }
        public static string Country { get; set; }
        public static string CentreRegion { get; set; }
        public static string[] Regions { get; private set; }
        public static string SelectedQnnee { get; set; }
        public static int Selectedcountryindex;
        public static int Selectedregionindex;
        private static CloudBlobContainer _container;

        public static void LoadCountryNames()
        {
            _container = GhgAccount.CreateCloudBlobClient().GetContainerReference("countries");
            var blob = _container.GetBlockBlobReference("countries.txt");
            var countries = blob.DownloadText();
            CountryNames = countries.Split(',');
            Selectedcountryindex = -1;
           
        }

        public static void LoadRegions()
        {
            if (Selectedcountryindex <= -1) return;
            var blob = _container.GetBlockBlobReference(CountryNames[Selectedcountryindex] + ".txt");
            Regions = blob.DownloadText(Encoding.UTF8).Split(',');
            Selectedregionindex = -1;
        }

        public static bool Isvalid(string qnnee)
        {
            return Regions.Contains(qnnee);
        }
    }
}