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
        public string[] Genders { get { return Enum.GetNames(typeof(Model.gender)); } }
        public string[] AgeGroups { get { return Enum.GetNames(typeof(Model.agegroup)); } }
        public string[] Facilities { get { return Enum.GetNames(typeof(Model.agegroup)); } }
        public string[] Stays { get { return Enum.GetNames(typeof(Model.stay)); } }
        public string[] Funders { get { return Enum.GetNames(typeof(Model.funder)); } }
        public string[] Treaters { get { return Enum.GetNames(typeof(Model.treater)); } }
        public string[] Qualifications { get { return Enum.GetNames(typeof(Model.qualification)); } }

        public byte Gender { get; set; }
        public byte AgeGroup { get; set; }
        public byte Facility { get; set; }
        public byte Stay { get; set; }
        public byte Funder { get; set; }
        public byte Treater { get; set; }
        public byte Qualification { get; set; }

        /// <summary>
        /// Initializes a new instance of the EpidemVM class.
        /// </summary>
        public EpidemVM() { }

        public EpidemVM(string str) { }


    }
}