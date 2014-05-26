using System;
using GalaSoft.MvvmLight;

namespace MvvmLight1.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class EpidemVM : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the EpidemVM class.
        /// </summary>
        public EpidemVM()
        {
        }

        public string[] Gender { get { return Enum.GetNames(typeof(Model.gender)); } }
        public string[] AgeGroup { get { return Enum.GetNames(typeof(Model.agegroup)); } }
        public string[] Facility { get { return Enum.GetNames(typeof(Model.agegroup)); } }
        public string[] Stay { get { return Enum.GetNames(typeof(Model.stay)); } }
        public string[] Funder { get { return Enum.GetNames(typeof(Model.funder)); } }
        public string[] Treater { get { return Enum.GetNames(typeof(Model.treater)); } }
        public string[] Qualification { get { return Enum.GetNames(typeof(Model.qualification)); } }


    }
}