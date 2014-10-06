using System.Windows;

namespace MvvmLight1
{
    /// <summary>
    /// Description for MvvmView1.
    /// </summary>
    public partial class EHealthV
    {
        /// <summary>
        /// Initializes a new instance of the MvvmView1 class.
        /// </summary>
        public EHealthV()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
   
    }

    class EHealthVImpl : EHealthV
    {
    }
}