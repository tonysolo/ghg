﻿using GalaSoft.MvvmLight;
using System;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MvvmLight1.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class LoaderVM : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MvvmViewModel1 class.
        /// </summary>
        /// 
        public LoaderVM()
        {
            // Model.AzureStorage.SetupAzureAccount();          
        }

        public string[] Regions { get; set; }

        public string Region { get; set; }

        public string Countries { get; set; }

        public string[] Qualifications { get { return Enum.GetNames(typeof(Model.Qualification)); } }

        public int SecurityChoice { get; set; }

        public string[] SecurityQuestions { get { return Model.Settings.Securityquestions; } }

        public string SecurityAnswer { get; set; }

        public bool Registered
        {
            get { return Model.Settings.Registered; }
            set { Model.Settings.Registered = value; }
        }
    }
}