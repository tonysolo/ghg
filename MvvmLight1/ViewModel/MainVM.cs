using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;

//using System.Configuration;
//using Microsoft.WindowsAzure.Storage;
//using Microsoft.WindowsAzure.Storage.Table;
using MvvmLight1.Model;


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

    public class MainVm : ViewModelBase
    {



        public bool IsRegistered
        {
            get { return Model.Settings.Registered; }
            set { Model.Settings.Registered = value; }
        }




        private void ShowMesg()
        {
            MessageBox.Show("This is a test");
        }




        private static void ShowMapDlg()
        {
            var v = new MapV();
            v.ShowDialog();

        }

        private static void ShowEpidemiologyDlg()
        {
            var v = new EpidemV();
            v.ShowDialog();
        }

        private static void ShowLoaderDlg()
        {
            //Userdata.LoadCountryNames();
            var v = new LoaderV();
            v.ShowDialog();
        }

        private static void ShowEhealthDlg()
        {
            var v = new EHealthV();
            v.ShowDialog();
        }


        //       public string Centre { get; private set; }
        //       public string Boundary { get; private set; }
        public string Qnnee { get; private set; }
        public string Qnnneee { get; private set; }

        /*
                private void RegionNorth()
                { 
                    Qnnee = QneUtils.MoveNsew(Qnnee, 'n');
                   // Centre = Model.QneUtils.CentrePoint(Qnnee);
                   // Boundary = Model.QneUtils.Boundary(Qnnee); 
                 // RaisePropertyChanged("Qnnee");
                   // RaisePropertyChanged("Boundary");
                   //  RaisePropertyChanged("Centre");
            
  
                }

                private void RegionSouth()
                { 
                    Qnnee = QneUtils.MoveNsew(Qnnee, 's');
                    Centre = Model.QneUtils.CentrePoint(Qnnee);
                    Boundary = Model.QneUtils.Boundary(Qnnee);
                    RaisePropertyChanged("Qnnee");
                     RaisePropertyChanged("Boundary");
                   RaisePropertyChanged("Centre");
                }

                private void RegionEast() 
                {
                    Qnnee = QneUtils.MoveNsew(Qnnee, 'e');
                    Centre = Model.QneUtils.CentrePoint(Qnnee);
                    Boundary = Model.QneUtils.Boundary(Qnnee);
                    RaisePropertyChanged("Qnnee");
                    RaisePropertyChanged("Boundary");
                    RaisePropertyChanged("Centre");
            
                }

                private void RegionWest() 
                {
                    Qnnee = QneUtils.MoveNsew(Qnnee, 'w');
                    Centre = Model.QneUtils.CentrePoint(Qnnee);                    
                    Boundary = Model.QneUtils.Boundary(Qnnee);
                    RaisePropertyChanged("Qnnee");
                    RaisePropertyChanged("Boundary");
                    RaisePropertyChanged("Centre");         
         }

        */




        //  public RelayCommand MoveRegionNorth { get; private set; }
        //  public RelayCommand MoveRegionEast { get; private set; }
        //  public RelayCommand MoveRegionWest { get; private set; }
        //  public RelayCommand MoveRegionSouth { get; private set; }

        public RelayCommand EditMap { get; private set; }
        public RelayCommand EditEpidemiology { get; private set; }
        public RelayCommand EditLoader { get; private set; }
        public RelayCommand EditEhealth { get; private set; }


        private void SetupRelayCommands()
        {
            EditMap = new RelayCommand(ShowMapDlg);  // (ShowMapDlg);         
            EditEpidemiology = new RelayCommand(ShowEpidemiologyDlg);
            EditLoader = new RelayCommand(ShowLoaderDlg);
            EditEhealth = new RelayCommand(ShowEhealthDlg);



            //     MoveRegionNorth = new RelayCommand(RegionNorth);
            //    MoveRegionEast = new RelayCommand(RegionEast);
            //      MoveRegionWest = new RelayCommand(RegionWest);
            //     MoveRegionSouth = new RelayCommand(RegionSouth);
        }


        public MainVm()
        {
            SetupRelayCommands();
            IsRegistered = true;
            Userdata.LoadCountryNames();

            Qnnee = Model.QneUtils.to_qnnee("-26.20,28.04");

            //    Centre = Model.QneUtils.CentrePoint(Qnnee);
            //   Boundary = Model.QneUtils.Boundary(Qnnee);
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