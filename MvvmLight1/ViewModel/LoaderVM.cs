using System.Text;
using GalaSoft.MvvmLight;
using System;
using GalaSoft.MvvmLight.Command;
using MvvmLight1.Model;
using Newtonsoft.Json;

namespace MvvmLight1.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// Manages things preferences and details for individual loaders / providers that will be stoed in the loader blob
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class LoaderVm : ViewModelBase
    {
        public string Region { get; set; }      //qnnee region
        public string Country { get; set; }     //country short code
        public string ID { get; set; }
        //stored in loader pageblob // pinoffset set in pageblob and recorded by user
        public int RegionIndex { get; set; }     //index    
        public int SecurityChoice { get; set; }
        public string SecurityAnswer { get; set; }
        public int ProviderChoice { get; set; }
        public string Surname { get; set; }
        public string Initials { get; set; }
        public string FirstName { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }
        public string RegAurthority { get; set; }
        public string RegNumber { get; set; }
        public bool Loaded { get; set; }


        public static string[] CountryShortNames
        {
            get { return Userdata.CountryNames; }
        }

        public string[] RegionNames
        {
            get { return Userdata.Regions; }
        }

        public static string[] Providers
        {
            get { return Enum.GetNames(typeof(Model.Providers)); }
        }

        public static string[] SecurityQuestions
        {
            get { return Model.Settings.Securityquestions; }
        }

        public bool Registered
        {
            get { return Model.Settings.Registered; }
            set { Model.Settings.Registered = value; }
        }


        public void SubmitData()
        {
            string[] str =
            {   
                String.Format("{0:x1}", SecurityChoice),
                SecurityAnswer,
                String.Format("{0:x1}", ProviderChoice),
                FirstName, Initials, Surname, Cellphone, Email, RegAurthority, RegNumber
            };

            AzureUtil.RegisterLoader(str);
        }

        public void ShowMapDlg()
        {
           // Userdata.Selectedcountryindex = Country;
            Userdata.SelectedQnnee = Region;
            RaisePropertyChanged("RegionIndex");
            RaisePropertyChanged("CountryIndex");
            var v = new MapV();
            v.ShowDialog();
        }

        public void GetRegions()
        {
           // Userdata.Selectedcountryindex = CountryIndex;
            Loaded = false;          
            Userdata.LoadRegions();//Regio(CountryShortNames[CountryIndex]);
            Loaded = true;                   
            RegionIndex = 1;
            Region = RegionNames[RegionIndex];

            RaisePropertyChanged("Loaded");
            RaisePropertyChanged("SelectedIndex");
            RaisePropertyChanged("RegionNames");
            RaisePropertyChanged("RegionIndex");
            RaisePropertyChanged("CountryIndex");
            //return Userdata.;
        }

        // public void RegisterLoader(string[] sarr)
        //  {
        // AzureUtil.RegisterLoader(sarr, enc: null);
        // }

        public RelayCommand EditMap { get; private set; }
        public RelayCommand Submit { get; private set; }
        public RelayCommand LoadRegions { get; private set; }


        private void SetupRelayCommands()
        {
            EditMap = new RelayCommand(ShowMapDlg); // (ShowMapDlg);        
            LoadRegions = new RelayCommand(GetRegions);
            Submit = new RelayCommand(SubmitData);
            Loaded = true;
        }

        /// <summary>
        /// Initializes a new instance of the MvvmViewModel1 class.
        /// </summary>
        public LoaderVm()
        {
            SetupRelayCommands();

        }
    }
}