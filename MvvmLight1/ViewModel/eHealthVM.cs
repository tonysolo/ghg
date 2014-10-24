using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace MvvmLight1.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class EHealthVm : ViewModelBase
    {

        private static void ShowMapDlg()
        {
            var v = new MapV();
            v.ShowDialog();
        }



 public RelayCommand EditMap { get; private set; }


        private void SetupRelayCommands()
        {
            EditMap = new RelayCommand(ShowMapDlg); // (ShowMapDlg);  
        }


        public EHealthVm()
        {
            SetupRelayCommands();

        }
    }
}