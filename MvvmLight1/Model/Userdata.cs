using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MvvmLight1.Model
{
    public static class Userdata
    {

        public static string SelectedCountryShortName { get; set; } //countrycode eg ZA


        public static string[] GetCountryShortNames()
        {
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
        }

        public static string[] Regions { get; set; }

        public static string CentreRegion()
        {
            return Regions[Regions.Length/2];
        }

        public static bool Isvalid(string qnnee)
        {
            return Regions.Contains(qnnee);
        }

        //  static public void Loaddata(string locationPin)
      //  {
            //bool invalid = false;
            //cloudid = location_pin;
            //validate and read prefs and GIS from cloud        
     //   }


        //static Dictionary<string, string> coords;
       // static bool _invalid;
       // static void AddCoords(string district, string qnnneee)
       // {
      //      coords.Add(district, qnnneee);
       //     _invalid = true;

      //  }


    }
}