using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmLight1.Model;

namespace MvvmLight1.ViewModel
{
    public class MainVm : ViewModelBase
    {

        public MainVm()
        {
            IsRegistered = true;
            SetupRelayCommands();
            //   CountryIndex = 0;
            var lvi = new ListViewItem();
        }



        public bool IsRegistered
        {
            get { return Settings.Registered; }
            set { Settings.Registered = value; }
        }

        public static string[] UserAccounts => SharedData.UserAccount;


        public static string[] CountryNames => SharedData.CountryNames;


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


        public static void ShowEpidemiologyDlg()
        {
            var v = new EpidemV();
            v.ShowDialog();
        }

        public static void ShowLoaderDlg()
        {
            //SharedData.SelectedCountryIndex = CountryIndex;
            var v = new LoaderV();
            v.ShowDialog();
        }

        private static void ShowEhealthDlg()
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



        public RelayCommand EditEpidemiology { get; private set; }
        public RelayCommand EditLoader { get; private set; }
        public RelayCommand EditEhealth { get; private set; }
        public RelayCommand AddEhealth { get; private set; }
        public RelayCommand HideEhealth { get; private set; }


        private void SetupRelayCommands()
        {
            // EditMap = new RelayCommand(ShowMapDlg); // (ShowMapDlg);         
            EditEpidemiology = new RelayCommand(ShowEpidemiologyDlg);
            EditLoader = new RelayCommand(ShowLoaderDlg);
            EditEhealth = new RelayCommand(ShowEhealthDlg);
            AddEhealth = new RelayCommand(AddEhealthDlg);
            HideEhealth = new RelayCommand(HideEhealthDlg);
        }

    }

}