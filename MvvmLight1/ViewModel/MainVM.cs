using System;
using System.Collections;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Maps.MapControl.WPF;
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
        }

        public bool IsRegistered
        {
            get { return Settings.Registered; }
            set { Settings.Registered = value; }
        }



        public static string[] CountryNames
        {
            get { return SharedData.CountryNames; }
        }

   

        public int CountryIndex {
            get
            {
                return SharedData.SelectedCountryIndex;               
            }
            set
            {   
                SharedData.SelectedCountryIndex = value;
                RaisePropertyChanged("CountryIndex");
                SharedData.SelectedRegionIndex = 0;
            }
        }

       // public RelayCommand EditMap { get; private set; }
       

        private void ShowMesg()
        {
            MessageBox.Show("This is a test");
        }


       // private static void ShowMapDlg()
      //  {
      //      if (SharedData.Region == null) return;
       //     var v = new MapV();
       //     v.ShowDialog();
       // }


        private static void ShowEpidemiologyDlg()
        {
            var v = new EpidemV();
            v.ShowDialog();
        }

        private void ShowLoaderDlg()
        {
            SharedData.SelectedCountryIndex = CountryIndex;
            var v = new LoaderV();
            v.ShowDialog();
        }

        private static void ShowEhealthDlg()
        {
            var v = new EHealthV();
            v.ShowDialog();
        }


        public RelayCommand EditEpidemiology { get; private set; }
        public RelayCommand EditLoader { get; private set; }
        public RelayCommand EditEhealth { get; private set; }



        private void SetupRelayCommands()
        {
           // EditMap = new RelayCommand(ShowMapDlg); // (ShowMapDlg);         
            EditEpidemiology = new RelayCommand(ShowEpidemiologyDlg);
            EditLoader = new RelayCommand(ShowLoaderDlg);
            EditEhealth = new RelayCommand(ShowEhealthDlg);
        }

    }
    
}