using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmLight1.Model;

namespace MvvmLight1.ViewModel
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class EpidemVm : ViewModelBase
    {
        private byte _stay;

        public EpidemVm()
        {
            SetupRelayCommands();
        }

        public string[] Genders
        {
            get { return Enum.GetNames(typeof (Gender)); }
        }

        // public string[] AgeGroups { get {return AzureAccess.Qnames;} }
        public string[] AgeGroups
        {
            get { return Enum.GetNames(typeof (Agegroup)); }
        }

        public string[] Facilities
        {
            get { return Enum.GetNames(typeof (Facilty)); }
        }

        public string[] Stays
        {
            get { return Enum.GetNames(typeof (Stay)); }
        }

        public string[] Funders
        {
            get { return Enum.GetNames(typeof (Funder)); }
        }

        public string[] Treaters
        {
            get { return Enum.GetNames(typeof (Treater)); }
        }

        public string[] Qualifications
        {
            get { return Enum.GetNames(typeof (Providers)); }
        }

        public string[] Regions { get; set; } //will have to be set from map info

        public byte AgeGroup { get; set; }
        public byte Gender { get; set; }
        public byte Facility { get; set; }

        public byte Stay
        {
            get { return _stay; }
            set
            {
                _stay = value;
                RaisePropertyChanged("staymessage");
            }
        }

        public byte Funder { get; set; }
        public byte Treater { get; set; }
        public byte Qualification { get; set; }
        public byte VisitsDays { get; set; }
        public string PatientRegion { get; set; }
        public string TreaterRegion { get; set; }

        public string Staymessage
        {
            get { return (Stay == 1) ? "Number of Days Admitted" : "Number of Similar Outpatients"; }
        }


        public RelayCommand EditMap { get; private set; }

        private static void ShowMapDlg()
        {
            var v = new MapV();
            v.ShowDialog();
        }


        private void SetupRelayCommands()
        {
            EditMap = new RelayCommand(ShowMapDlg); // (ShowMapDlg);  
        }
    }
}

// IsRegistered = true;


// Qnnee = Model.QneUtils.to_qnnee("-26.20,28.04");

//    Centre = Model.QneUtils.CentrePoint(Qnnee);
//   Boundary = Model.QneUtils.Boundary(Qnnee);