using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmLight1.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Maps.MapControl.WPF;
using MvvmLight1.Model;


namespace MvvmLight1.ViewModel
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class MapVm : ViewModelBase
    {

        public MapVm()
        {

            SetupRelayCommands();
            SavedQnnee = SharedData.Region;
            Qnnee = SharedData.Region;
            Centre = SharedData.Region;
            Centre = QneUtils.IndexPoint(Qnnee);
            Zoom = "6";

        }



        public string SavedQnnee { get; set; }
        public string Zoom { get; set; }
        public string Centre { get; set; }
        public string Boundary { get; set; }
        public string Qnnee { get; set; }
        public string Qnnneee { get; set; }
        public string Fill { get; set; }


        public RelayCommand Search { get; private set; } //to implement patient search in region
        public RelayCommand MoveRegionNorth { get; private set; }
        public RelayCommand MoveRegionEast { get; private set; }
        public RelayCommand MoveRegionWest { get; private set; }
        public RelayCommand MoveRegionSouth { get; private set; }
        public RelayCommand SetZoom4 { get; private set; }
        public RelayCommand SetZoom6 { get; private set; }

        private void Zoom4()
        {
            Zoom = QneUtils.Setzoom4();
            RaisePropertyChanged("Zoom");
        }


        private void Zoom6()
        {
            Zoom = QneUtils.Setzoom6();
            RaisePropertyChanged("Zoom");
        }


       

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
            SetZoom4 = new RelayCommand(Zoom4);
            SetZoom6 = new RelayCommand(Zoom6);
        }
    }
}