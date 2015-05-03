using System.Windows;
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
            //this.MyMap.Center.Latitude = 12.34;
            
            Closing += (s, e) => ViewModelLocator.Cleanup();
           
        }
    }
}