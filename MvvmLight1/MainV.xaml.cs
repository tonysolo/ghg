using System.Windows;
using Microsoft.Maps.MapControl.WPF;
using MvvmLight1.ViewModel;
using MessageBox = System.Windows.Forms.MessageBox;
//using System.Windows;
using System.Windows.Controls;

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

        //private void ListView1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        //{
        //    MessageBox.Show("Button Clkicked");
        //}

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            
MessageBox.Show("Button Clicked");
            var p = MainVm.Patients[0];
            //throw new System.NotImplementedException();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }


        //private void ListView1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        //{
           
        //}
    } 
}