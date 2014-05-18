using GalaSoft.MvvmLight;
using MvvmLight1.Model;

namespace MvvmLight1.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// 
    /// The main view is a menu with three items:
    /// 
    /// 1 User registration - users need to register before they can do anything - view data, or 
    /// add epidemiology or patient records. They need to log in before they can do anything.
    /// 
    /// 2 Epidemiology view or edit
    /// 
    /// 3 Patient records view or edit
    /// 
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainVM : ViewModelBase
    {
        private readonly IDataService _dataService;

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
       // public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }

            set
            {
                if (_welcomeTitle == value)
                {
                    return;
                }

                _welcomeTitle = value;
                RaisePropertyChanged("Welcome Title");
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainVM(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    WelcomeTitle = item.Title;
                });
        }
       //todo  test this code
        
        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}