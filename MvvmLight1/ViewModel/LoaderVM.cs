using GalaSoft.MvvmLight;

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
        public LoaderVM()
        {
            Model.settings.registered = true;
        }

        public bool Registered 
          { get {return Model.settings.registered;}
            set { Model.settings.registered = value; }  
          }
    }
}