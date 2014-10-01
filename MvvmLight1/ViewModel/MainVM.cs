﻿using GalaSoft.MvvmLight;
using MvvmLight1.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows;
using MvvmLight1.ViewModel;
//using System.Configuration;
//using Microsoft.WindowsAzure.Storage;
//using Microsoft.WindowsAzure.Storage.Table;


namespace MvvmLight1.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can databind to.
    /// 
    /// The main view is a menu with three items:
    /// 
    /// 1 User registration
    /// 2 User login
    /// 
    /// users need to register and log in they can do anything - view data, or 
    /// add epidemiology or add patient records. 
    /// 
    /// 3 Epidemiology view or edit
    /// 4 Patient e-health records to view or edit
    /// 
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    /// 

    public class MainVM : ViewModelBase
    {
        // private readonly IDataService _dataService;

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        // public const string WelcomeTitlePropertyName = "WelcomeTitle";

        public string Centre { get; private set; }
        public string Boundary { get; private set; }
        public string qnnee { get; private set; }
        public string qnnneee { get; private set; }


        public bool IsRegistered
        {
            get { return Model.settings.registered; }
            set { Model.settings.registered = value; }
        }



        private void ShowMesg()
        {
            MessageBox.Show("This is a test");
        }

        private void ShowMapDlg()
        {
            MapV v = new MapV();
            v.ShowDialog();
            
        }

        private void ShowEpidemiologyDlg()
        {
            EpidemV v = new EpidemV();
            v.ShowDialog();
        }

        private void ShowLoaderDlg()
        {
            LoaderV v = new LoaderV();
            v.ShowDialog();
        }

        private void ShowEhealthDlg()
        {
            eHealthV v = new eHealthV();
            v.ShowDialog();
        }

        private void RegionNorth()
        { 
            qnnee = QNE_Utils.MoveNSEW(qnnee, 'n');
            Centre = Model.QNE_Utils.CentrePoint(qnnee);
            Boundary = Model.QNE_Utils.Boundary(qnnee);
           
            RaisePropertyChanged("Boundary");
             RaisePropertyChanged("Centre");
            
  
        }

        private void RegionSouth()
        { 
            qnnee = QNE_Utils.MoveNSEW(qnnee, 's');
            Centre = Model.QNE_Utils.CentrePoint(qnnee);
            Boundary = Model.QNE_Utils.Boundary(qnnee);
             RaisePropertyChanged("Boundary");
           RaisePropertyChanged("Centre");
        }

        private void RegionEast() 
        {
            qnnee = QNE_Utils.MoveNSEW(qnnee, 'e');
            Centre = Model.QNE_Utils.CentrePoint(qnnee);
            Boundary = Model.QNE_Utils.Boundary(qnnee);       
            RaisePropertyChanged("Boundary");
            RaisePropertyChanged("Centre");
            
        }

        private void RegionWest() 
        {
            qnnee = QNE_Utils.MoveNSEW(qnnee, 'w');
            Centre = Model.QNE_Utils.CentrePoint(qnnee);                    
            Boundary = Model.QNE_Utils.Boundary(qnnee); 
            RaisePropertyChanged("Boundary");
            RaisePropertyChanged("Centre");
 }

        public RelayCommand EditMap { get; private set; }
        public RelayCommand EditEpidemiology { get; private set; }
        public RelayCommand EditLoader { get; private set; }
        public RelayCommand EditEhealth { get; private set; }
        public RelayCommand MoveRegionNorth { get; private set; }
        public RelayCommand MoveRegionEast { get; private set; }
        public RelayCommand MoveRegionWest { get; private set; }
        public RelayCommand MoveRegionSouth { get; private set; }




        private void SetupRelayCommands()
        {
            EditMap = new RelayCommand(ShowMapDlg);  // (ShowMapDlg);
            EditEpidemiology = new RelayCommand(ShowEpidemiologyDlg);
            EditLoader = new RelayCommand(ShowLoaderDlg);
            EditEhealth = new RelayCommand(ShowEhealthDlg);
            MoveRegionNorth = new RelayCommand(RegionNorth);
            MoveRegionEast = new RelayCommand(RegionEast);
            MoveRegionWest = new RelayCommand(RegionWest);
            MoveRegionSouth = new RelayCommand(RegionSouth);
        }

        public MainVM()
        {
            SetupRelayCommands();
            IsRegistered = true;
            qnnee = Model.QNE_Utils.to_qnnee("-26.20,28.04");
            Centre = Model.QNE_Utils.CentrePoint(qnnee);
            Boundary = Model.QNE_Utils.Boundary(qnnee);
        }


        //  public bool isRegistered
        //  {.
        //      get { return Model.settings.registered; }
        //     set { Model.settings.registered = value; }
        //take out set - must be readonly except for loader registration to set it once on login

        // }
    }
}
// isRegistered = true;  //needs to be replaced with loader registration

//object item;
//object error;
// string WelcomeTitle;

// IDataService _dataService = dataService;

// _dataService.GetData((item,error) => 
//     {
//        if (error != null)
//         {
// Report error here
//              return;
//         }

//         WelcomeTitle = item.Title;
//      }); 


/*  
  private string _welcomeTitle = string.Empty;

  /// <summary>
  /// Gets the WelcomeTitle property.
  /// Changes to that property's value raise the PropertyChanged event. 
  /// </summary>
  public string WelcomeTitle
  {
      get
      {
          return _welcomeTitle;
      }

      set
      {
          if (_welcomeTitle == value)
          {
              return;
          }

          _welcomeTitle = value;
          RaisePropertyChanged("Welcome Title");
      }
  }
  */
/// <summary>
/// Initializes a new instance of the MainViewModel class.
/// </summary>
/*        public MainVM(IDataService dataService)
        {
            SetupRelayCommands();


            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    WelcomeTitle = item.Title;
                }); 

        }
       
       //todo  test this code
        
        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
 */