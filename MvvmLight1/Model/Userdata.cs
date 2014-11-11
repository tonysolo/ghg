using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using Microsoft.WindowsAzure.Storage.Blob;
using MvvmLight1.Model;

namespace MvvmLight1.Model
{
    public static class Userdata
    {

        public static string SelectedCountryShortName { get; set; } //countrycode eg ZA
        public static string CentreRegion { get; private set; }
        public static string[] Regions { get; private set; }
        public static string SelectedQnnee { get; set; }



        public static string[] GetCountryShortNames()
        {
            //AzureStorage.DevelopmentContainers();
          var cdc = AzureStorage.
              DevelopmentContainers().
              GetEnumerator().
              Current.
              GetPageBlobReference("l");
           
          
            
            using (var reader = File.OpenText(@"c:\azure\countries.txt"))
                return reader.ReadToEnd().Split(',');
        }


        public static void DownloadRegions(string countrycode) //selected countrycode
        {
            if (countrycode.Length != 2) return;
            var s = (countrycode + ".txt").ToLower();
            s = @"c:\azure\" + s;
            using (var reader = File.OpenText(s))
                Regions = reader.ReadToEnd().Split(',');

            Int16 nn = 0;
            Int16 ee = 0;
            var q = 0;
            foreach (var qne in Regions)
            {
                q = Convert.ToInt16(qne.Substring(0, 1), 16);
                nn += Convert.ToInt16(qne.Substring(1, 2), 16);
                ee += Convert.ToInt16(qne.Substring(3, 2), 16);
            }
            nn /= (Int16)Regions.Length;
            ee /= (Int16)Regions.Length;
            CentreRegion = String.Format("{0:x1}{1:x2}{2:x2}", q, nn, ee);
        }


        public static bool Isvalid(string qnnee)
        {
            return Regions.Contains(qnnee);
        }
    }
}