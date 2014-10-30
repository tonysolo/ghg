using GalaSoft.MvvmLight;
using System;
using GalaSoft.MvvmLight.Command;
using MvvmLight1.Model;


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

        public string[] RegionNames { get; set; }

        public int RegionIndex { get; set; }

        public static int CountryIndex { get; set; }

        public static string[] CountryShortNames { get; set; }

        public static string SelectedCountryShortName { get; set; }

        public string Country { get; set; }

        public string[] Qualifications
        {
            get { return Enum.GetNames(typeof(Model.Qualification)); }
        }

        public int SecurityChoice { get; set; }

        public string[] SecurityQuestions
        {
            get { return Model.Settings.Securityquestions; }
        }


        public string SecurityAnswer { get; set; }

        public bool Registered
        {
            get { return Model.Settings.Registered; }
            set { Model.Settings.Registered = value; }
        }

        private void ShowMapDlg()
        {
            Userdata.SelectedQnnee = RegionNames[RegionIndex];
            var v = new MapV();
            v.ShowDialog();
        }

        private void GetRegions()
        {
            Userdata.DownloadRegions(CountryShortNames[CountryIndex]);//(Userdata.SelectedCountryShortName);
            RegionNames = Userdata.Regions;
            RaisePropertyChanged("RegionNames");
            RegionIndex = 0;
            RaisePropertyChanged("RegionIndex");

            //return Userdata.;
        }


        private void SetupRelayCommands()
        {
            EditMap = new RelayCommand(ShowMapDlg); // (ShowMapDlg);        
            LoadRegions = new RelayCommand(GetRegions);
        }


        public RelayCommand EditMap { get; private set; }
        public RelayCommand Submit { get; private set; }
        public RelayCommand LoadRegions { get; private set; }


        /// <summary>
        /// Initializes a new instance of the MvvmViewModel1 class.
        /// </summary>

        public LoaderVm()
        {
            CountryShortNames = Userdata.GetCountryShortNames();
            SetupRelayCommands();
        }
    }
}