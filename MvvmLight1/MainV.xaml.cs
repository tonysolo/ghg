using System.Windows;
using Microsoft.Maps.MapControl.WPF;
using MvvmLight1.ViewModel;

namespace MvvmLight1
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainV : Window
    {
        /// <summary>
        ///     Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainV()
        {
            InitializeComponent();
            //this.MyMap.Center.
           // SetValue();
           // ViewModel.ViewModelLocator.
            Closing += (s, e) => ViewModelLocator.Cleanup();
           
        }

        private void listView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}