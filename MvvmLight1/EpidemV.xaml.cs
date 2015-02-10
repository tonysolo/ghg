using System.Windows;
using System.Windows.Controls;

namespace MvvmLight1
{
    /// <summary>
    ///     Description for MvvmView1.
    /// </summary>
    public partial class EpidemV : Window
    {
        /// <summary>
        ///     Initializes a new instance of the MvvmView1 class.
        /// </summary>
        public EpidemV()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        // private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        // {

        // }
    }
}