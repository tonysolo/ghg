using System;
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
        /*
        public static string MoveS(string qnnee)
        {
            var q = (byte)(qnnee[0] - '0');
           // var q = Convert.ToInt16(qnnee.Substring(0, 1), 16);//Substring(0, 1);
            var ns = Convert.ToInt16(qnnee.Substring(1, 2), 16);
            var ew = Convert.ToInt16(qnnee.Substring(3, 2), 16);
            var south = (q & 0x02) == 2;
            if (south)
            {
                if (ns < 127)
                    ns++;
            }
            else //if north will move south until zero north
            //then step to zero south at the equator, quadrant change,
            //to draw different boundaries
            {
                ns--;
                if (ns >= 0) return String.Format("{0:x1}{1:x2}{2:x2}", q, ns, ew);
                q = (byte)(q | 0x02); //change quadrant to south 
                ns = 0;
            }
            return String.Format("{0:x1}{1:x2}{2:x2}", q, ns, ew);
        }
*/

        public MapVm()
        {
            Qnnee = Userdata.Region;
            Centre = QneUtils.IndexPoint(Qnnee);
            SetupRelayCommands();
        }

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
            Fill = Userdata.Isvalid(Qnnee) ? "RoyalBlue" : "";
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
            Fill = Userdata.Isvalid(Qnnee) ? "RoyalBlue" : "";
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
            Fill = Userdata.Isvalid(Qnnee) ? "RoyalBlue" : "";
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
            Fill = Userdata.Isvalid(Qnnee) ? "RoyalBlue" : "";
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
    }
}