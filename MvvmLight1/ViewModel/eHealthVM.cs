using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace MvvmLight1.ViewModel
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class EHealthVm : ViewModelBase
    {
        public EHealthVm()
        {
            SetupRelayCommands();
            Regions = new[] {"Sandton", "Fourways", "Pretoria"};
        }

        public string[] Regions { get; set; }


        public RelayCommand EditMap { get; private set; }

        private static void ShowMapDlg()
        {
            var v = new MapV();
            v.ShowDialog();
        }


        private void SetupRelayCommands()
        {
           EditMap = new RelayCommand(ShowMapDlg);  
        }
    }
}