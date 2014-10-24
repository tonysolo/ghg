using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmLight1.Model;
using System.Windows;

namespace MvvmLight1.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    /// 

public class MapVm : ViewModelBase
    {
    //private string _centre; 
   // public string Centre { get; set; }

   // public string height { get; set; }
 
    private void ShowMsg()
        {
       //Centre = "37.000,-122.000";
       
            MessageBox.Show("This is a test");
        }

        public string Centre { get; private set; }
        public string Boundary { get; private set; }
        public string Qnnee { get; private set; }
        public string Qnnneee { get; private set; }




public RelayCommand Search {get; private set;}

     private void RegionNorth()
        { 
            Qnnee = QneUtils.MoveNsew(Qnnee, 'n');
            Centre = Model.QneUtils.CentrePoint(Qnnee);
            Boundary = Model.QneUtils.Boundary(Qnnee); 
            RaisePropertyChanged("Qnnee");
            RaisePropertyChanged("Boundary");
            RaisePropertyChanged("Centre");
            
  
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

 public RelayCommand MoveRegionNorth { get; private set; }
        public RelayCommand MoveRegionEast { get; private set; }
        public RelayCommand MoveRegionWest { get; private set; }
        public RelayCommand MoveRegionSouth { get; private set; }




  private void SetupRelayCommands()
        {
           // EditMap = new RelayCommand(ShowMapDlg);  // (ShowMapDlg);
           // EditEpidemiology = new RelayCommand(ShowEpidemiologyDlg);
           //EditLoader = new RelayCommand(ShowLoaderDlg);
           // EditEhealth = new RelayCommand(ShowEhealthDlg);
            

            MoveRegionNorth = new RelayCommand(RegionNorth);
            MoveRegionEast = new RelayCommand(RegionEast);
            MoveRegionWest = new RelayCommand(RegionWest);
            MoveRegionSouth = new RelayCommand(RegionSouth);
        }



        /// <summary>
        /// Initializes a new instance of the MapVM class.
        /// </summary>
        public MapVm()
        {
           SetupRelayCommands();
           // height = "250";
            Centre = "26.076,-27.972";
            SetupRelayCommands();
        }

      




    }
}