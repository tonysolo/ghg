using System.Linq;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MvvmLight1.Model
{
    //static class current user data -- will lookup azure utilities and then keep a session copy
    //of regions and country names to avoid unnecessary repeat access to azure

    public static class Userdata
    {
        private static string[] _countries;
        private static string[] _regions;
        private static int _countryindex;

        public static int Selectedcountryindex { get { return _countryindex; } 
                                                set { _countryindex = value;
                                                    _regions = null;
                                                }}

        public static CloudBlobContainer Container;

        public static string SelectedCountry
        {
            get { return _countries[Selectedcountryindex]; }
        }

        public static string[] CountryNames { get { return _countries ?? (_countries = Azure.CountryNames()); }}

        public static string[] Regions { get { return _regions ?? (_regions = Azure.GetRegions(SelectedCountry)); } }

        public static string Region { get; set; }

       // public static string Country { get { return SelectedCountry; } }

        public static bool Isvalid(string qnnee) { return Regions.Contains(qnnee); }
    }
}