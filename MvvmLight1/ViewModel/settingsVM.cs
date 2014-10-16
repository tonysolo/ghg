using GalaSoft.MvvmLight;
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
   
        /// <summary>
        /// Initializes a new instance of SettingsVM class. This is where the azure 
        /// storage account settings are stored. Each country has a separate storage 
        /// account which contains the azure subscriptions regions and their names. 
        /// This data is stored in the ghg (-root) storage account in my susciption. 
        /// It could include ghg accounts in other subsciptions but this is not a likely
        /// requirement.
        /// </summary>
 public class SettingsVM : ViewModelBase
    {

        public SettingsVM()
        {
           // Model.AzureStorage.SetupAzureAccount();          
        }

       // public string[] Subscriptions {get{ return Enum.GetNames(typeof(Model.subscription));}}//hard coded
      //  public string[] Accounts { get { return Model.settings.accounts; } } }//hard coded 0 to 5
      // public  string[] Regions { get { return Model.settings.regions; } } }//hard coded while using 2 accounts
                                                                              //with 200 regions for sa 
                                                                              //after that will use azure ghg root.
       
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