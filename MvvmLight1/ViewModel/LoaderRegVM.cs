using System;
using System.Resources;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmLight1.Model;

namespace MvvmLight1.ViewModel
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     Manages things preferences and details for individual loaders / providers that will be stoed in the loader blob
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class LoaderVm : ViewModelBase
    {
        /// <summary>
        ///     Initializes a new instance of the MvvmViewModel1 class.
        /// </summary>
        public LoaderVm()
        {
            SetupRelayCommands();
            Country = SharedData.CountryName;          
            Regions = SharedData.RegionNames;
            Region = SharedData.Region;
            CountryIndex = SharedData.SelectedCountryIndex;
            //ResetProperties();
        }

        public int CountryIndex { get; set; }
        public string[] Regions { get; set; }
        public string Region { get; set; } //qnnee region
        public string Country { get; set; } //country short code

        public string Id
        {
            get { return ""; }
            set { value = ""; }
        }

        //stored in loader pageblob // pinoffset set in pageblob and recorded by user
        public int RegionIndex { get; set; } //index 
        public int PinOffset;
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


        public static string[] CountryNames => SharedData.CountryNames;


        public static string[] Providers => Enum.GetNames(typeof(Providers));

        public static string[] SecurityQuestions
        {
            get { return SimpleSettings.Securityquestions; }
        }

        public bool Registered
        {
            get { return SimpleSettings.Registered; }
            set { SimpleSettings.Registered = value; }
        }



        public RelayCommand<System.Windows.Window> CloseCommand
        {
            get; private set;
        }

        public RelayCommand EditMap { get; private set; }
        public RelayCommand Submit { get; private set; }
        public RelayCommand LoadRegions { get; private set; }

        private void SubmitData()
        {
            string[] str =
            {
                $"{SecurityChoice:x1}",
                SecurityAnswer,
                $"{ProviderChoice:x1}",
                FirstName, Initials, Surname, Cellphone, Email, RegAurthority, RegNumber
            };
            this.Cleanup();
            // LoaderWindow.
            //AzureUtil.RegisterLoader(str);
        }

        private void GetData()
        {
        }

        public void ShowMapDlg()
        {
            //CountryIndex = 1;
            //SharedData.SelectedCountryIndex = CountryIndex;
            // Userdata.Region = Region;
            // Region = RegionNames[Userdata.SelectedCountryIndex];
            //var i = Region.Length;
            Region = SharedData.Region;
            // SharedData.SelectedCountryIndex = Region;
            QneUtils.IndexPoint(Region);
            QneUtils.CentrePoint(Region);
            RaisePropertyChanged("Region");
            //RaisePropertyChanged("RegionIndex");
            RaisePropertyChanged("Center");
            //RaisePropertyChanged("CountryIndex");
            //Userdata.Region = Region;
            var v = new MapV();
            v.ShowDialog();
        }

        private void ResetProperties()
        {
            RaisePropertyChanged("Regions");
            RaisePropertyChanged("Region");
            RaisePropertyChanged("RegionIndex");
        }

        public void GetRegions()
        {
            SharedData.SelectedCountryIndex = CountryIndex;     
            Loaded = false;
            Regions = SharedData.RegionNames; //Regio(CountryShortNames[CountryIndex]);
            Loaded = true;
            RegionIndex = 0;
            ResetProperties();         
        }


        private void SetupRelayCommands()
        {
            EditMap = new RelayCommand(ShowMapDlg); // (ShowMapDlg);        
            LoadRegions = new RelayCommand(GetRegions);
            Submit = new RelayCommand(SubmitData);
            Loaded = true;
        }
    }
}