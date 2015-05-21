using System.Windows.Controls;

namespace MvvmLight1
{
    /// <summary>
    ///     Description for CountryView.
    /// </summary>
    public partial class EHealthV
    {
        /// <summary>
        ///     Initializes a new instance of the CountryView class.
        /// </summary>
        public EHealthV()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        // private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        // {

        //}
    }

    internal class EHealthVImpl : EHealthV
    {
    }
}