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
                 
           //SavedQnnee = SharedData.Region;
           // Qnnee = SharedData.Region;
           // Centre = SharedData.Region;
          //  Centre = QneUtils.IndexPoint(Qnnee);
           // SetupRelayCommands();
            //var cv = new CountryView();
            //bool ok = (bool)cv.ShowDialog();
        }

        public bool IsRegistered
        {
            get { return Settings.Registered; }
            set { Settings.Registered = value; }

        }



       // public string[] CountryNames { get; private set; }
/*        public string Centre { get; private set; }
        public string Zoom { get; private set; }
        public string Boundary { get; private set; }
        public string Qnnee { get; set; }
       // public string Qnnneee { get; private set; }
        public string Fill { get; private set; }
        public string SavedQnnee { get; private set; }


        public RelayCommand Search { get; private set; } //to implement patient search in region
        public RelayCommand MoveRegionNorth { get; private set; }
        public RelayCommand MoveRegionEast { get; private set; }
        public RelayCommand MoveRegionWest { get; private set; }
        public RelayCommand MoveRegionSouth { get; private set; }
        public RelayCommand SetZoom4 { get; private set; }
        public RelayCommand SetZoom6 { get; private set; }
        public RelayCommand SaveQnnee { get; private set; }

        private void RegionNorth()
        {
            Qnnee = QneUtils.MoveN(Qnnee);
            Centre = QneUtils.CentrePoint(Qnnee);
            Boundary = QneUtils.Boundary(Qnnee);
            Fill = SharedData.Isvalid(Qnnee) ? "RoyalBlue" : "";
            RaisePropertyChanged("Qnnee");
            RaisePropertyChanged("Boundary");
            RaisePropertyChanged("Centre");
            RaisePropertyChanged("Fill");
        }

        private void RegionSouth()
        {
            Qnnee = QneUtils.MoveS(Qnnee);
            Centre = QneUtils.CentrePoint(Qnnee);
            Boundary = QneUtils.Boundary(Qnnee);
            Fill = SharedData.Isvalid(Qnnee) ? "RoyalBlue" : "";
            RaisePropertyChanged(()=>Qnnee);
            //RaisePropertyChanged("Qnnee");
            //RaisePropertyChanged("Boundary");
            RaisePropertyChanged(()=>Boundary) ;
           // RaisePropertyChanged("Centre");
            RaisePropertyChanged(()=>Centre);
           // RaisePropertyChanged("Fill");
            RaisePropertyChanged(()=>Fill);
            //var window = Application.Current.Windows[1];
           
        }

        private void RegionEast()
        {
            Qnnee = QneUtils.MoveE(Qnnee);
            Centre = QneUtils.CentrePoint(Qnnee);
            Boundary = QneUtils.Boundary(Qnnee);
            Fill = SharedData.Isvalid(Qnnee) ? "RoyalBlue" : "";
            RaisePropertyChanged("Qnnee");
            RaisePropertyChanged("Boundary");
            RaisePropertyChanged("Centre");
            RaisePropertyChanged("Fill");
        }

        private void RegionWest()
        {
            Qnnee = QneUtils.MoveW(Qnnee);
            Centre = QneUtils.CentrePoint(Qnnee);
            Boundary = QneUtils.Boundary(Qnnee);
            Fill = SharedData.Isvalid(Qnnee) ? "RoyalBlue" : "";
            RaisePropertyChanged("Qnnee");
            RaisePropertyChanged("Boundary");
            RaisePropertyChanged("Centre");
            RaisePropertyChanged("Fill");
        }

        private void Saveqnnee()
        {
            SavedQnnee = Qnnee;
            RaisePropertyChanged("SavedQnnee");
        }

        private void Zoom4()
        {
            Zoom = "10";
            RaisePropertyChanged("Zoom");
           // RaisePropertyChanged("ZoomLevel");
        }

      private void Zoom6()
        {
         
          Zoom = "20";
          RaisePropertyChanged(()=>Zoom);
          //RaisePropertyChanged("ZoomLevel");
        }

     

        private void SetupRelayCommands()
        {
            MoveRegionNorth = new RelayCommand(RegionNorth);
            MoveRegionEast = new RelayCommand(RegionEast);
            MoveRegionWest = new RelayCommand(RegionWest);
            MoveRegionSouth = new RelayCommand(RegionSouth);
            SaveQnnee   =    new RelayCommand(Saveqnnee);
            SetZoom4 = new RelayCommand(Zoom4);
            SetZoom6 = new RelayCommand(Zoom6);
        }

*/



      


        public static string[] CountryNames
        {
            get { return SharedData.CountryNames; }
        }

        public string[] RegionNames
        {
            get
            {
                //SharedData.SelectedCountryIndex = CountryIndex;
                return SharedData.RegionNames;
            }
          //set { Userdata.SelectedCountryIndex = CountryIndex; }
          //      RaisePropertyChanged("CountryIndex");
        }

     //   public int CountryIndex
      //  {
        //    get
       //     {
        //        return SharedData.SelectedCountryIndex;
        //    }
        //    set
       //     {
       //         SharedData.SelectedCountryIndex = value;
        //        RaisePropertyChanged("CountryIndex");
         //   }
      //  }

        //       public string Centre { get; private set; }
        //       public string Boundary { get; private set; }
       
       // public string Qnnee { get; private set; }
       // public string Qnnneee { get; private set; }

        public int CountryIndex {
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

        public RelayCommand EditMap { get; private set; }
        public RelayCommand EditEpidemiology { get; private set; }
        public RelayCommand EditLoader { get; private set; }
        public RelayCommand EditEhealth { get; private set; }

        private void ShowMesg()
        {
            MessageBox.Show("This is a test");
        }


        private static void ShowMapDlg()
        {
            if (SharedData.Region == null) return;
            var v = new MapV();
            v.ShowDialog();
            
        }


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


        private void SetupRelayCommands()
        {
            EditMap = new RelayCommand(ShowMapDlg); // (ShowMapDlg);         
            EditEpidemiology = new RelayCommand(ShowEpidemiologyDlg);
            EditLoader = new RelayCommand(ShowLoaderDlg);
            EditEhealth = new RelayCommand(ShowEhealthDlg);
        }

/*

 public string Centre { get; private set; }
        public string Boundary { get; private set; }
        public string Qnnee { get; private set; }
        public string Qnnneee { get; private set; }
        public string Fill { get; private set; }


        public RelayCommand Search { get; private set; } //to implement patient search in region
        public RelayCommand MoveRegionNorth { get; private set; }
        public RelayCommand MoveRegionEast { get; private set; }
        public RelayCommand MoveRegionWest { get; private set; }
        public RelayCommand MoveRegionSouth { get; private set; }

        private void RegionNorth()
        {
            Qnnee = QneUtils.MoveN(Qnnee);
            Centre = QneUtils.CentrePoint(Qnnee);
            Boundary = QneUtils.Boundary(Qnnee);
            Fill = SharedData.Isvalid(Qnnee) ? "RoyalBlue" : "";
            RaisePropertyChanged("Qnnee");
            RaisePropertyChanged("Boundary");
            RaisePropertyChanged("Centre");
            RaisePropertyChanged("Fill");
        }

        private void RegionSouth()
        {
            Qnnee = QneUtils.MoveS(Qnnee);
            Centre = QneUtils.CentrePoint(Qnnee);
            Boundary = QneUtils.Boundary(Qnnee);
            Fill = SharedData.Isvalid(Qnnee) ? "RoyalBlue" : "";
            RaisePropertyChanged("Qnnee");
            RaisePropertyChanged("Boundary");
            RaisePropertyChanged("Centre");
            RaisePropertyChanged("Fill");
        }

        private void RegionEast()
        {
            Qnnee = QneUtils.MoveE(Qnnee);
            Centre = QneUtils.CentrePoint(Qnnee);
            Boundary = QneUtils.Boundary(Qnnee);
            Fill = SharedData.Isvalid(Qnnee) ? "RoyalBlue" : "";
            RaisePropertyChanged("Qnnee");
            RaisePropertyChanged("Boundary");
            RaisePropertyChanged("Centre");
            RaisePropertyChanged("Fill");
        }

        private void RegionWest()
        {
            Qnnee = QneUtils.MoveW(Qnnee);
            Centre = QneUtils.CentrePoint(Qnnee);
            Boundary = QneUtils.Boundary(Qnnee);
            Fill = SharedData.Isvalid(Qnnee) ? "RoyalBlue" : "";
            RaisePropertyChanged("Qnnee");
            RaisePropertyChanged("Boundary");
            RaisePropertyChanged("Centre");
            RaisePropertyChanged("Fill");
        }
 
        private void SetupRelayCommands()
        {
            MoveRegionNorth = new RelayCommand(RegionNorth);
            MoveRegionEast = new RelayCommand(RegionEast);
            MoveRegionWest = new RelayCommand(RegionWest);
            MoveRegionSouth = new RelayCommand(RegionSouth);
        }
        */
    }
    
}