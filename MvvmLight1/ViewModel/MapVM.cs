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
    //private string _centre; 
    public string Centre { get; set; } 
 
    private void ShowMsg()
        {
       //Centre = "37.000,-122.000";
       
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
            Centre = "37.806029,-122.407007";
            SetupRelayCommands();
        }

    }
}