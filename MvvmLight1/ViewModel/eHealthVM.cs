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
            Selectedpatientindex = 0;
           // Patients = new List<patient>();
            var p = new Provider.Patient
            {
                
                Name = "Tony Manicom",
                Email = "tony@turbomed.co.za",
                Sex = "M",
                Birthday = new System.DateTime(1948,7,8).ToShortDateString(),
                NextVisit = new System.DateTime().AddDays(30).ToShortDateString()

            };
            
            //{
            //    name = "Tony Manicom",
            //    email = "tony@turbomed",
            //    sex = "M",
            //    birthday = new System.DateTime(1948, 7, 8).ToShortDateString()
            
            //    //Lastvisit = new System.DateTime(2015, 11, 20),
            //    //Nextvisit = System.DateTime.MinValue
            //};

            //for (int i=0; i<10; i++)
           // Patients.Add(p);
        }

        public ObservableCollection<Provider.Patient> Patients { get; set; }

        public int Selectedpatientindex { get; set; }
        //string lastv { get; set; }
        //string Nextv { get; set; }
        //string Age { get; set; }

        public Provider.Patient SelectedPatient => Patients[Selectedpatientindex];


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

