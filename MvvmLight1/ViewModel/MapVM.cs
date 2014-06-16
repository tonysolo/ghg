using GalaSoft.MvvmLight;
using MvvmLight1.Model;
using GalaSoft.MvvmLight.Command;
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

public class MapVM : ViewModelBase
    {


 private void ShowMsg()
        {
            MessageBox.Show("This is a test");
        }



public RelayCommand Search {get; private set;}



private void SetupRelayCommands()
{
Search = new RelayCommand(ShowMsg);
}

        /// <summary>
        /// Initializes a new instance of the MapVM class.
        /// </summary>
        public MapVM()
        {
            SetupRelayCommands();
        }

    }
}