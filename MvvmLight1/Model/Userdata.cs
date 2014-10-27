using System.Collections.Generic;
using System.IO;

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

       // public static string[] GetRegions(string countrycode)
       // {
       //     var s = (@"c:\azure\" + countrycode + ".txt").ToLower();
       //     using (var reader = File.OpenText(s))
       //         return reader.ReadToEnd().Split(',');
       // }

        public static string[] GetRegions() //selected countrycode
        {
            if (SelectedCountryShortName.Length != 2) return null;
            var s = (SelectedCountryShortName + ".txt").ToLower();
            using (var reader = File.OpenText(s))
                return reader.ReadToEnd().Split(',');
        }

        public static string[] Regions;

        static public void Loaddata(string locationPin)
        {
            //bool invalid = false;
            //cloudid = location_pin;
            //validate and read prefs and GIS from cloud        
        }


        static Dictionary<string, string> coords;
        static bool _invalid;
        static void AddCoords(string district, string qnnneee)
        {
            coords.Add(district, qnnneee);
            _invalid = true;

        }


    }
}