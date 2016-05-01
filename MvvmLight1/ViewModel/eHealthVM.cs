using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmLight1.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;

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
           // Selectedpatientindex = 0;
           // Patients = new List<patient>();

            Name = SelectedPatient.Name;
            var p = new Patient
            {             
                Name = "Tony Manicom",
                Email = "tony@turbomed.co.za",
                Sex = "M",
                Birthday = new System.DateTime(1948,7,8).ToShortDateString(),
                NextVisit = new System.DateTime().AddDays(30).ToShortDateString()
            };
    
        }

        public Patient SelectedPatient => SharedData.Patients[SharedData.Selectedpatientindex];

        public string Name { get; set; }
   // => SelectedPatient.Name;
        public string[] Regions => SharedData.RegionNames;


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
                                                           //edits the selected Patient
        }

    }

}

