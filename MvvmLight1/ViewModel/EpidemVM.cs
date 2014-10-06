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
    public class EpidemVm : ViewModelBase
    {
        public string[] Genders { get { return Enum.GetNames(typeof(Model.Gender)); } }
        // public string[] AgeGroups { get {return AzureAccess.Qnames;} }
        public string[] AgeGroups{ get { return Enum.GetNames(typeof(Model.Agegroup)); } }
        public string[] Facilities { get { return Enum.GetNames(typeof(Model.Facilty)); } }
        public string[] Stays { get { return Enum.GetNames(typeof(Model.Stay)); } }
        public string[] Funders { get { return Enum.GetNames(typeof(Model.Funder)); } }
        public string[] Treaters { get { return Enum.GetNames(typeof(Model.Treater)); } }
        public string[] Qualifications { get { return Enum.GetNames(typeof(Model.Qualification)); } }
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
        public byte VisitsDays { get; set; }
        public string PatientRegion { get; set; }
        public string TreaterRegion { get; set; }

        public string Staymessage
        { get { return (Stay == 1) ? "Number of Days Admitted" : "Number of Similar Outpatients"; } }

       // public object EditMap
       // {
      //      get { throw new NotImplementedException(); }
       // }
    }
}