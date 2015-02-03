﻿using System.Text;
using GalaSoft.MvvmLight;
using System;
using GalaSoft.MvvmLight.Command;
using MvvmLight1.Model;
using Newtonsoft.Json;


namespace MvvmLight1.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// Manages things preferences and details for individual loaders / providers that will be stoed in the loader blob
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class LoaderVm : ViewModelBase
    {
        public string[] RegionNames { get; set; }
        public static int RegionIndex { get; set; }
        public static int CountryIndex { get; set; }

        public static string[] CountryShortNames
        {
            get { return Userdata.CountryNames; }
        }
        public string Country { get; set; }

        public static string[] Providers
        {
            get { return Enum.GetNames(typeof(Model.Providers)); }
        }
        public static string[] SecurityQuestions
        {
            get { return Model.Settings.Securityquestions; }
        }
        public bool Registered
        {
            get { return Model.Settings.Registered; }
            set { Model.Settings.Registered = value; }
        }
        //stored in loader pageblob // pinoffset set in pageblob and recorded by user
        public int SecurityChoice { get; set; }
        public string SecurityAnswer { get; set; }
        public int ProviderChoice { get; set; }
        public string Surname { get; set; }
        public string Initials { get; set; }
        public string FirstName { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }
        public string RegAurthority { get; set; }
        public string RegNumber { get; set; }



        public void SubmitData()
        {
            string[] str =
            {   RegionNames[RegionIndex],
                String.Format("{0:x1}", SecurityChoice),
                SecurityAnswer,
                String.Format("{0:x1}", ProviderChoice),
                FirstName, Initials, Surname, Cellphone, Email, RegAurthority, RegNumber
            };

            AzureUtil.RegisterLoader(str);
        }

        public void ShowMapDlg()
        {
            Userdata.Selectedcountryindex = CountryIndex; 
            Userdata.SelectedQnnee = RegionNames[RegionIndex];
            RaisePropertyChanged("RegionIndex");
            RaisePropertyChanged("CountryIndex");
            var v = new MapV();
            v.ShowDialog();
        }

        public void GetRegions()
        {
            Userdata.Selectedcountryindex = CountryIndex;
            Userdata.LoadRegions();//Regio(CountryShortNames[CountryIndex]);
            RegionNames = Userdata.Regions;
            RegionIndex = 1;
            RaisePropertyChanged("SelectedIndex");
            RaisePropertyChanged("RegionNames");      
            RaisePropertyChanged("RegionIndex");
            RaisePropertyChanged("CountryIndex");
            //return Userdata.;
        }

       // public void RegisterLoader(string[] sarr)
      //  {
           // AzureUtil.RegisterLoader(sarr, enc: null);
       // }

        public RelayCommand EditMap { get; set; }
        public RelayCommand Submit { get; set; }
        public RelayCommand LoadRegions { get; set; }


        private void SetupRelayCommands()
        {
            EditMap = new RelayCommand(ShowMapDlg); // (ShowMapDlg);        
            LoadRegions = new RelayCommand(GetRegions);
            Submit = new RelayCommand(SubmitData);
        }

        /// <summary>
        /// Initializes a new instance of the MvvmViewModel1 class.
        /// </summary>
        public LoaderVm()
        {
            SetupRelayCommands();
        }
    }
}