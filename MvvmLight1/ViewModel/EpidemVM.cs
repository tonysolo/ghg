using System;
using GalaSoft.MvvmLight;
using MvvmLight1.Model;

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
        // public string[] AgeGroups { get {return AzureAccess.Qnames;} }
        public string[] AgeGroups{ get { return Enum.GetNames(typeof(Model.agegroup)); } }
        public string[] Facilities { get { return Enum.GetNames(typeof(Model.facilty)); } }
        public string[] Stays { get { return Enum.GetNames(typeof(Model.stay)); } }
        public string[] Funders { get { return Enum.GetNames(typeof(Model.funder)); } }
        public string[] Treaters { get { return Enum.GetNames(typeof(Model.treater)); } }
        public string[] Qualifications { get { return Enum.GetNames(typeof(Model.qualification)); } }
        public string[] Regions { get; set; } //will have to be set from map info

        public byte AgeGroup { get; set; }
        public byte Gender { get; set; }
        public byte Facility { get; set; }

        private byte _stay;
        public byte Stay
        {
            get { return _stay; }
            set { _stay = value; RaisePropertyChanged("staymessage"); }
        }
        public byte Funder { get; set; }
        public byte Treater { get; set; }
        public byte Qualification { get; set; }
        public byte Visits_Days { get; set; }
        public string PatientRegion { get; set; }
        public string TreaterRegion { get; set; }

        public string staymessage
        { get { return (Stay == 1) ? "Number of Days Admitted" : "Number of Similar Outpatients"; } }

        /// <summary>
        /// Initializes a new instance of the EpidemVM class.
        /// </summary>
        /// 
        public EpidemVM() { }



    }
}