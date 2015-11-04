using System.Collections;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmLight1.Model;

namespace MvvmLight1.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class CountryVm : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MvvmViewModel1 class.
        /// </summary>
        /// 
        public IEnumerable CountryNames { get; set; }
        public static int ItemIndex { get; set; }

        private static void SaveCountry()
        {
            SharedData.SelectedCountryIndex = ItemIndex;
            //save county selected index to shared data
        }


        public RelayCommand Save { get; private set; }

        public CountryVm()
        {
            CountryNames = SharedData.CountryNames;
            Save = new RelayCommand(SaveCountry);
            //load counties from shared data
        }
    }
}