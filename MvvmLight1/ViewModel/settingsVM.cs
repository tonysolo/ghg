using GalaSoft.MvvmLight;
using MvvmLight1.Model;

namespace MvvmLight1.ViewModel
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    /// <summary>
    ///     Initializes a new instance of SettingsVM class. This is where the azure
    ///     storage Account settings are stored. Each country has a separate storage
    ///     Account which contains the azure subscriptions Regions and their names.
    ///     This data is stored in the ghg (-root) storage Account in my susciption.
    ///     It could include ghg accounts in other subsciptions but this is not a likely
    ///     requirement.
    /// </summary>
    public class SettingsVm : ViewModelBase
    {
        // public string[] Subscriptions {get{ return Enum.GetNames(typeof(Model.subscription));}}//hard coded
        //  public string[] Accounts { get { return Model.settings.accounts; } } }//hard coded 0 to 5
        // public  string[] Regions { get { return Model.settings.Regions; } } }//hard coded while using 2 accounts
        //with 200 Regions for sa 
        //after that will use azure ghg root.

        public int SecurityChoice { get; set; }

        public string[] SecurityQuestions => Settings.Securityquestions;

        public string SecurityAnswer { get; set; }

        public bool Registered
        {
            get { return Settings.Registered; }
            set { Settings.Registered = value; }
        }
    }
}