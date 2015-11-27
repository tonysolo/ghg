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
            Closing += (s, e) => ViewModelLocator.Cleanup();          
        }

        
    } 
}