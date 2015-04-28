using System.Linq;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MvvmLight1.Model
{
    //static class current user data -- will lookup azure utilities and then keep a session copy
    //of regions and country names to avoid unnecessary repeat access to azure

    public static class Userdata
    {
        public static string[] Countries;
        public static string[] Regions;
        public static int Selectedcountryindex;
        public static int RegionIndex;

        public static CloudBlobContainer Container;

        public static string SelectedCountry
        {
            get { return CountryNames[Selectedcountryindex]; }
        }

        public static string[] CountryNames
        {
            get { return Countries ?? Azure.CountryNames(); }
        }

        public static string[] RegionsNames()
        {
           return Regions ?? Azure.GetRegions(SelectedCountry);
        }

        public static string Region
        {
            get { return ((RegionIndex < Regions.Length)&&(Regions!=null)) ? Regions[RegionIndex] : ""; }
        }

        // public static string Country { get { return SelectedCountry; } }

        public static bool Isvalid(string qnnee)
        {
            return Regions.Contains(qnnee);
        }
    }
}