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
        private static int _selectedcountryindex;

        private static void LoadCountryNames(string str)
        {
            _selectedcountryindex = 0;
            CountryNames = str.Split(',');
            Country = CountryNames[_selectedcountryindex];
        }

        // public static CloudBlobContainer Container { get; set; }


        //public static CloudStorageAccount Csa = CloudStorageAccount.DevelopmentStorageAccount;


        // public static string[] GetCountryShortNames()
        // {
        // var ghgAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("GHGConnectionString"));

        //  }

        public static void GetRegions(string country)
        {

            // var ghgAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("GHGConnectionString"));
            var container = GhgAccount.CreateCloudBlobClient().GetContainerReference("countries");
            var blob = container.GetBlockBlobReference("za.txt");
            var regions = blob.DownloadText();
            Regions = regions.Split(',');
            //var sarr = str.Split(',');

            //        Int16 nn = 0;
            //        Int16 ee = 0;
            //         var q = 0;
            //        foreach (var qne in sarr)
            //     {
            //           q = Convert.ToInt16(qne.Substring(0, 1), 16);
            //           nn += Convert.ToInt16(qne.Substring(1, 2), 16);
            //          ee += Convert.ToInt16(qne.Substring(3, 2), 16);
            //     }
            //    nn /= (Int16)Regions.Length;
            //    ee /= (Int16)Regions.Length;
            //   CentreRegion = String.Format("{0:x1}{1:x2}{2:x2}", q, nn, ee);
        }


        //       public static string[] GetCountryShortNames()
        //       {
        //using (var reader = File.OpenText(@"c:\azure\countries.txt"))
        //               return reader.ReadToEnd().Split(',');
        //AzureStorage.DevelopmentContainers();
        //var cdc = AzureUtil.CountryNames();
        //   DevelopmentContainers().
        //   GetEnumerator().
        //   Current.
        //  GetPageBlobReference("l");                    
        //        }


        // public static void DownloadRegions(string countrycode) //selected countrycode
        // {
        //      if (countrycode.Length != 2) return;
        //       var s = (countrycode + ".txt").ToLower();
        //       s = @"c:\azure\" + s;
        //       using (var reader = File.OpenText(s))
        //           Regions = reader.ReadToEnd().Split(',');
        //
        //         Int16 nn = 0;
        //        Int16 ee = 0;
        //         var q = 0;
        //        foreach (var qne in Regions)
        //      {
        //           q = Convert.ToInt16(qne.Substring(0, 1), 16);
        //          nn += Convert.ToInt16(qne.Substring(1, 2), 16);
        //        ee += Convert.ToInt16(qne.Substring(3, 2), 16);
        //     }
        //  nn /= (Int16)Regions.Length;
        //    ee /= (Int16)Regions.Length;
        //    CentreRegion = String.Format("{0:x1}{1:x2}{2:x2}", q, nn, ee);
        // }


        public static bool Isvalid(string qnnee)
        {
            return Regions.Contains(qnnee);
        }
    }
}