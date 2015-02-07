using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;


namespace MvvmLight1.ViewModel
{
   

    public class MainVm : ViewModelBase
    {

        public bool IsRegistered
        {
            get { return Model.Settings.Registered; }
            set { Model.Settings.Registered = value; }
        }


        private void ShowMesg()
        {
            MessageBox.Show("This is a test");
        }


        private static void ShowMapDlg()
        {
            var v = new MapV();
            v.ShowDialog();

        }

        private static void ShowEpidemiologyDlg()
        {
            var v = new EpidemV();
            v.ShowDialog();
        }

        private static void ShowLoaderDlg()
        {
            //Userdata.LoadCountryNames();
            var v = new LoaderV();
            v.ShowDialog();
        }

        private static void ShowEhealthDlg()
        {
            var v = new EHealthV();
            v.ShowDialog();
        }

        //       public string Centre { get; private set; }
        //       public string Boundary { get; private set; }
        public string Qnnee { get; private set; }
        public string Qnnneee { get; private set; }

       
        public RelayCommand EditMap { get; private set; }
        public RelayCommand EditEpidemiology { get; private set; }
        public RelayCommand EditLoader { get; private set; }
        public RelayCommand EditEhealth { get; private set; }


        private void SetupRelayCommands()
        {
            EditMap = new RelayCommand(ShowMapDlg);  // (ShowMapDlg);         
            EditEpidemiology = new RelayCommand(ShowEpidemiologyDlg);
            EditLoader = new RelayCommand(ShowLoaderDlg);
            EditEhealth = new RelayCommand(ShowEhealthDlg);          
        }


        public MainVm()
        {
            SetupRelayCommands();
            IsRegistered = true;
           // Userdata.LoadCountryNames();

            Qnnee = Model.QneUtils.to_qnnee("-26.20,28.04");

           
        }


       
    }
}



