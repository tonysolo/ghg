/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:MvvmLight1.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using MvvmLight1.Model;

namespace MvvmLight1.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
                //SimpleIoc.Default.Register<userdata>();
            }

            SimpleIoc.Default.Register<MainVM>();
            SimpleIoc.Default.Register<eHealthVM>();
            SimpleIoc.Default.Register<EpidemVM>();
            SimpleIoc.Default.Register<LoaderVM>();
            SimpleIoc.Default.Register<MapVM>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]

        public MainVM Main { get { return ServiceLocator.Current.GetInstance<MainVM>(); } }
        public MapVM Map { get { return ServiceLocator.Current.GetInstance<MapVM>(); } }
        public LoaderVM Loader { get { return ServiceLocator.Current.GetInstance<LoaderVM>(); } }
        public eHealthVM eHealth { get { return ServiceLocator.Current.GetInstance<eHealthVM>(); } }
        public EpidemVM Epidem { get { return ServiceLocator.Current.GetInstance<EpidemVM>(); } }
       // public userdata Userdata { get { return ServiceLocator.Current.GetInstance<userdata>(); } }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}