using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmLight1.Model;

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
  

        public string Centre { get; private set; }
        public string Boundary { get; private set; }
        public string Qnnee { get; private set; }
        public string Qnnneee { get; private set; }
        public string Fill { get; private set; }

       public RelayCommand Search { get; private set; } //to implement patient search in region


        private void RegionNorth()
        {
            Qnnee = QneUtils.MoveNsew(Qnnee, 'n');
            Centre = Model.QneUtils.CentrePoint(Qnnee);
            Boundary = Model.QneUtils.Boundary(Qnnee);
            Fill = Userdata.Isvalid(Qnnee) ? "RoyalBlue" : "";
            RaisePropertyChanged("Qnnee");
            RaisePropertyChanged("Boundary");
            RaisePropertyChanged("Centre");
            RaisePropertyChanged("Fill");
        }

        private void RegionSouth()
        {
            Qnnee = QneUtils.MoveNsew(Qnnee, 's');
            Centre = Model.QneUtils.CentrePoint(Qnnee);
            Boundary = Model.QneUtils.Boundary(Qnnee);
            Fill = Userdata.Isvalid(Qnnee) ? "RoyalBlue" : "";
            RaisePropertyChanged("Qnnee");
            RaisePropertyChanged("Boundary");
            RaisePropertyChanged("Centre");
            RaisePropertyChanged("Fill");
        }

        private void RegionEast()
        {
            Qnnee = QneUtils.MoveNsew(Qnnee, 'e');
            Centre = Model.QneUtils.CentrePoint(Qnnee);
            Boundary = Model.QneUtils.Boundary(Qnnee);
            Fill = Userdata.Isvalid(Qnnee) ? "RoyalBlue" : "";
            RaisePropertyChanged("Qnnee");
            RaisePropertyChanged("Boundary");
            RaisePropertyChanged("Centre");
            RaisePropertyChanged("Fill");
        }

        private void RegionWest()
        {
            Qnnee = QneUtils.MoveNsew(Qnnee, 'w');
            Centre = Model.QneUtils.CentrePoint(Qnnee);
            Boundary = Model.QneUtils.Boundary(Qnnee);
            Fill = Userdata.Isvalid(Qnnee) ? "RoyalBlue" : "";
            RaisePropertyChanged("Qnnee");
            RaisePropertyChanged("Boundary");
            RaisePropertyChanged("Centre");
            RaisePropertyChanged("Fill");
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
            Qnnee = Userdata.SelectedQnnee;
            Centre = QneUtils.IndexPoint(Qnnee);
            MoveRegionNorth = new RelayCommand(RegionNorth);
            MoveRegionEast = new RelayCommand(RegionEast);
            MoveRegionWest = new RelayCommand(RegionWest);
            MoveRegionSouth = new RelayCommand(RegionSouth);
            RegionSouth();
            RaisePropertyChanged("Qnnee");
            RaisePropertyChanged("Centre");
            RaisePropertyChanged("Boundary");
            RaisePropertyChanged("Fill");
        }

        /// <summary>
        /// Initializes a new instance of the MapVM class.
        /// </summary>
        /// 
        /// 
        public MapVm()
        {           
            SetupRelayCommands();
        }
    }
}