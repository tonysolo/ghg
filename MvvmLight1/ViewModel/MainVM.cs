using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.WindowsAzure;
using MvvmLight1.Model;
using Microsoft.WindowsAzure.Storage;

namespace MvvmLight1.ViewModel
{
    public class MainVm : ViewModelBase
    {

        public MainVm()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("GHGConnectionString"));
            IsRegistered = true;
            SetupRelayCommands();
            Login = "-provider Id-";
            Pin = "-pin-";
            CountryNames = SharedData.CountryNames;
            LoadEhealth();
            var lvi = new ListViewItem();
        }

        public static string Lastvisit { get; set; }
        public static string Nextvisit { get; set; }

        public static ObservableCollection<Patient> Patients => SharedData.Patients;

        public static int SelectedIndex
        {
            get { return SharedData.Selectedpatientindex; }
            set { SharedData.Selectedpatientindex = value; }
        }

        public static Patient Patient {
            get { return SharedData.Patients[SelectedIndex]; }
            set { SharedData.Patients[SelectedIndex] = value; }
        }
        public static string Login { get; set; }

        public static string Pin { get; set; }

        public bool IsRegistered
        {
            get { return SimpleSettings.Registered; }
            set { SimpleSettings.Registered = value; }
        }

        public static string[] UserAccounts => SharedData.UserAccount;


        public static string[] CountryNames { get; set; }


        public int CountryIndex
        {
            get
            {
                return SharedData.SelectedCountryIndex;
            }
            set
            {
                SharedData.SelectedCountryIndex = value;
                RaisePropertyChanged("CountryIndex");
            }
        }

        // public RelayCommand EditMap { get; private set; }


        public void ShowMesg()
        {
            MessageBox.Show("This is a test");
        }


        // private static void ShowMapDlg()
        //  {
        //      if (SharedData.Region == null) return;
        //     var v = new MapV();
        //     v.ShowDialog();
        // }


        //public static void ShowEpidemiologyDlg()
        //{
        //   // var s = Login;
        //    var v = new EpidemV();
        //    v.ShowDialog();
        //}
       


        public static void ShowLoaderDlg()
        {
            //SharedData.SelectedCountryIndex = CountryIndex;
            var v = new LoaderV();
            v.ShowDialog();

        }

        public static void ShowEhealthDlg()
        {

            var v = new EHealthV();
            MessageBox.Show("Show Ehealth");
            v.ShowDialog();
        }

        private static void AddEhealthDlg()
        {
            var v = new EHealthV();
            MessageBox.Show("If Registered for E-Health, use Patient's E-Health Id or SA ID Number");
            v.ShowDialog();
        }

        private static void HideEhealthDlg()
        {
            var v = new EHealthV();
            MessageBox.Show("Hide Ehealth");
            v.ShowDialog();
        }

        private static void LoadEhealth()
        {
            var pat = new Provider("ghza=22427=1", "2");
            var prov = pat.Prov;
            SharedData.Patients = pat.Recent;
        }

        //public RelayCommand EditEpidemiology { get; private set; }
        public RelayCommand EditLoader { get; private set; }
        public RelayCommand EditEhealth { get; private set; }
        public RelayCommand AddEhealth { get; private set; }
        public RelayCommand HideEhealth { get; private set; }
        public RelayCommand LoadEhealthdata { get; private set; }

        private void SetupRelayCommands()
        {
            // EditMap = new RelayCommand(ShowMapDlg); // (ShowMapDlg);         
            //EditEpidemiology = new RelayCommand(ShowEpidemiologyDlg);
            EditLoader = new RelayCommand(ShowLoaderDlg);
            EditEhealth = new RelayCommand(ShowEhealthDlg);
            AddEhealth = new RelayCommand(AddEhealthDlg);
            HideEhealth = new RelayCommand(HideEhealthDlg);
            LoadEhealthdata = new RelayCommand(LoadEhealth);
        }

    }

}