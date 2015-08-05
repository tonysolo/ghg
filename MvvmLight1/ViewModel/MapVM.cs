using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
            //Savedqnnee = SharedData.Region;         
            Qnnee = SharedData.Region;
            Centre = QneUtils.CentrePoint(Qnnee);
            //RaisePropertyChanged("Centre");           
        }


        private void Invalidate()
        {
           // RaisePropertyChanged("StartCentre");
            //RaisePropertyChanged("PinCentre");
            //RaisePropertyChanged("Qnnee");
            //RaisePropertyChanged("Centre");
            //RaisePropertyChanged("FillColor");
            

        }

        // public string Savedqnnee { get; set; }
        //public string PinCentre { get; set; }
        //public string StartCentre { get; set; }
        //public string Topx;
        public string PinColour { get; set; }
        public string Zoom { get; set; }
        public string Centre { get; set; }
        public string Boundary { get; set; }
        public string Qnnee { get; set; }
        public string Qnnneee { get; set; }
        public string FillColor { get; set; }
        public string left { get; set; }
        //public string _Locations { get; set; }


        //public RelayCommand Search { get; private set; } //to implement patient search in region
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
            Qnnee = QneUtils.MoveNsew(Qnnee, 'n');//QneUtils.MoveN(Qnnee);
            Centre = QneUtils.CentrePoint(Qnnee);
            Boundary = QneUtils.Boundary(Qnnee);
            FillColor = SharedData.Isvalid(Qnnee) ? "Blue" : "White";
            RaisePropertyChanged("Centre");       
            RaisePropertyChanged("Qnnee");
            RaisePropertyChanged("Boundary");
            RaisePropertyChanged("Centre");
            RaisePropertyChanged("FillColor");
  
        }

        private void RegionSouth()
        {

            Qnnee = QneUtils.MoveNsew(Qnnee, 's');//QneUtils.MoveS(Qnnee);
            Centre = QneUtils.CentrePoint(Qnnee);
            Boundary = QneUtils.Boundary(Qnnee);
            FillColor = SharedData.Isvalid(Qnnee) ? "Blue" : "White";
            RaisePropertyChanged("Qnnee");
           RaisePropertyChanged("Boundary");
           RaisePropertyChanged("Centre");
           RaisePropertyChanged("FillColor");

        }

        private void RegionEast()
        {
            Qnnee = QneUtils.MoveNsew(Qnnee, 'e');//QneUtils.MoveE(Qnnee);
            Centre = QneUtils.CentrePoint(Qnnee);
            Boundary = QneUtils.Boundary(Qnnee);
            FillColor = SharedData.Isvalid(Qnnee) ? "Blue" : "White";
            RaisePropertyChanged("Qnnee");
           RaisePropertyChanged("Boundary");
           RaisePropertyChanged("Centre");
           RaisePropertyChanged("FillColor");
        }

        private void RegionWest()
        {
            Qnnee = QneUtils.MoveNsew(Qnnee, 'w');//QneUtils.MoveW(Qnnee);
            Centre = QneUtils.CentrePoint(Qnnee);
            Boundary = QneUtils.Boundary(Qnnee);
            FillColor = SharedData.Isvalid(Qnnee) ? "Blue" : "White";
            RaisePropertyChanged("Qnnee");
           RaisePropertyChanged("Boundary");
           RaisePropertyChanged("Centre");
            RaisePropertyChanged("FillColor");
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