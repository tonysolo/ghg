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

        public static int RegionIndex { get; set; }

        public static int CountryIndex { get; set; }

        public static string[] CountryShortNames
        {
            get { return Userdata.GetCountryShortNames(); }
        }

        public string Country { get; set; }

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
        //stored in loader pageblob // pinoffset set in pageblob and recorded by user
        public int SecurityChoice { get; set; }
        public string SecurityAnswer { get; set; }
        public int ProviderChoice { get; set; }             
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }
        public string RegAurthority { get; set; }
        public string RegNumber { get; set; }

        public void SubmitData()
        {
            string[] str =
            {
                String.Format("{0:x1}", SecurityChoice),
                SecurityAnswer,
                String.Format("{0:x1}", ProviderChoice),
                FirstName, Surname, Cellphone, Email, RegAurthority, RegNumber
            };

        }

        public void ShowMapDlg()
        {
            Userdata.SelectedQnnee = RegionNames[RegionIndex];
            var v = new MapV();
            v.ShowDialog();
        }

        public void GetRegions()
        {
            Userdata.DownloadRegions(CountryShortNames[CountryIndex]);//(Userdata.SelectedCountryShortName);
            RegionNames = Userdata.Regions;
            RaisePropertyChanged("RegionNames");
            RegionIndex = 0;
            RaisePropertyChanged("RegionIndex");
            //return Userdata.;
        }

        public RelayCommand EditMap { get; set; }
        private RelayCommand Submit { get; set; }
        public RelayCommand LoadRegions { get; set; }


        private void SetupRelayCommands()
        {
            EditMap = new RelayCommand(ShowMapDlg); // (ShowMapDlg);        
            LoadRegions = new RelayCommand(GetRegions);
            Submit = new RelayCommand(SubmitData);
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