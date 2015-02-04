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


        private void DrawRegion()
        {
            Centre = Model.QneUtils.CentrePoint(Qnnee);
            Boundary = Model.QneUtils.Boundary(Qnnee);
            Fill = Userdata.Isvalid(Qnnee) ? "RoyalBlue" : "";
            RaisePropertyChanged("Qnnee");
            RaisePropertyChanged("Boundary");
            RaisePropertyChanged("Centre");
            RaisePropertyChanged("Fill");
        }


        private void RegionNorth()
        {
            Qnnee =  QneUtils.MoveN(Qnnee);
            DrawRegion();
        }

        private void RegionSouth()
        {
            Qnnee = QneUtils.MoveS(Qnnee);
            DrawRegion();
        }

        private void RegionEast()
        {
            Qnnee = QneUtils.MoveE(Qnnee);
            DrawRegion();
        }

        private void RegionWest()
        {
            Qnnee = QneUtils.MoveW(Qnnee);
            DrawRegion();
        }

        public RelayCommand Search { get; private set; } //to implement patient search in region
        public RelayCommand MoveRegionNorth { get; private set; }
        public RelayCommand MoveRegionEast { get; private set; }
        public RelayCommand MoveRegionWest { get; private set; }
        public RelayCommand MoveRegionSouth { get; private set; }

        private void SetupRelayCommands()
        {            
            MoveRegionNorth = new RelayCommand(RegionNorth);
            MoveRegionEast = new RelayCommand(RegionEast);
            MoveRegionWest = new RelayCommand(RegionWest);
            MoveRegionSouth = new RelayCommand(RegionSouth);
        }

        /// <summary>
        /// Initializes a new instance of the MapVM class.
        /// </summary>
        /// 
        /// 
        public MapVm()
        {  
         Qnnee = Userdata.SelectedQnnee;
            Centre = QneUtils.IndexPoint(Qnnee);
            SetupRelayCommands();
        }
    }
}