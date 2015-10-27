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
        }

        public List<patient> Patients;

        int selectedpatientindex;

        public patient selectedpatient { get { return Patients[selectedpatientindex]; } }



        public string[] Regions { get { return SharedData.RegionNames; } }


        public RelayCommand EditMap { get; private set; }

        private static void ShowMapDlg()
        {
            var v = new MapV();
            v.ShowDialog();
        }

        private void SetupRelayCommands()
        {
           EditMap = new RelayCommand(ShowMapDlg);  
        }
    }
}