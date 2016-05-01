using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Globalization;

namespace MvvmLight1.Model
{
    //static class current user data -- will lookup azure utilities and then keep a session copy
    //of Regions and country names to avoid unnecessary repeat access to azure

    public static class SharedData
    {
        //public static CultureInfo Ci = new CultureInfo("en-us");
        private static string[] _regionnames;
        private static string[] _countrynames;
        private static int _selectedcountryindex;

        public static string CountryName => CountryNames[SelectedCountryIndex];

        public static string SelectedCountry { get; set; }

        public static string[] UserAccount
        { //need to set and get these from isolated storage - qnnee1234 accounts and pin codes
            get;
            set;
        }
        public static int SelectedCountryIndex
        {
            get { return _selectedcountryindex; }
            set { _selectedcountryindex = value; _regionnames = null; }
        }

        public static int SelectedRegionIndex
        {
            get;
            set;
        }

        public static string[] CountryNames
        {
            get
            {
                _countrynames = _countrynames ?? CountriesRegions.CountryNames();
                return _countrynames;
            }
        }

        public static String[] RegionNames
        {
            get
            {
                return _regionnames = _regionnames ?? CountriesRegions.GetRegions(CountryName);
            }
        }

        public static string Region
        {
            get
            {
                var s = RegionNames[SelectedRegionIndex];
                if (s.Length == 6) s = s.Substring(1);         
                return s;
            }
        }


        public static bool Isvalid(string qnnee)
        {
            var b = RegionNames.Contains(qnnee);
            var s = qnnee;
            return b;
        }

        public static ObservableCollection<Patient> Patients { get; set; }

        public static int Selectedpatientindex { get; set; }


    }
}