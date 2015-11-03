using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmLight1.Model;
using System.Collections.Generic;

namespace MvvmLight1.ViewModel
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class EHealthVm : ViewModelBase
    {
        public EHealthVm()
        {
            SetupRelayCommands();
            selectedpatientindex = 0;
            Patients = new List<patient>();
            patient p = new patient();
            p.name = "Tony Manicom";
            p.email = "tony@turbomed";
            p.gender = 'M';
            p.dateofbirth = new System.DateTime(1948, 7, 8);       
            p.lastvisit = new System.DateTime(2015, 11, 20);
            p.nextvisit = System.DateTime.MinValue;
            //for (int i=0; i<10; i++)
            Patients.Add(p);       
        }

        public List<patient> Patients { get; set; }

        int selectedpatientindex { get; set; }
        //string lastv { get; set; }
        //string nextv { get; set; }
        //string age { get; set; }

        public patient SelectedPatient { get { return Patients[selectedpatientindex]; } }



        public string[] Regions { get { return SharedData.RegionNames; } }


        public RelayCommand EditMap { get; private set; }
        public RelayCommand EditEhealth { get; private set; }

        private static void ShowMapDlg()
        {
            var v = new MapV();
            v.ShowDialog();
        }

        private static void ShowEhealthDlg()
        {
            System.Windows.MessageBox.Show("testing 1232344");
            var v = new EHealthV();
            v.ShowDialog();
        }

        private void SetupRelayCommands()
        {
           EditMap = new RelayCommand(ShowMapDlg);
           EditEhealth = new RelayCommand(ShowEhealthDlg);//public RelayCommand EditEhealth { get; private set; }
            //edits the selected patient
}
    }
}