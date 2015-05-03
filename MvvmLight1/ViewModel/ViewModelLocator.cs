/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:MvvmLight1"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/
//using GalaSoft.MvvmLight;

//using GalaSoft.MvvmLight.Helpers;
//using GalaSoft.MvvmLight.Command;
//using GalaSoft.MvvmLight.Messaging;
//using GalaSoft.MvvmLight.Threading;

using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace MvvmLight1.ViewModel
{
    /// <summary>
    ///     This class contains static references to all the view models in the
    ///     application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        ///     Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainVm>(); //<MainViewModel>()
            SimpleIoc.Default.Register<LoaderVm>();
            //SimpleIoc.Default.Register<MapVm>();
            SimpleIoc.Default.Register<EHealthVm>();
            SimpleIoc.Default.Register<EpidemVm>();
        }

        public MainVm Main
        {
            get { return ServiceLocator.Current.GetInstance<MainVm>(); }
        }

        public LoaderVm Loader
        {
            get { return ServiceLocator.Current.GetInstance<LoaderVm>(); }
        }

       // public MapVm Map
       // {
         //  get { return ServiceLocator.Current.GetInstance<MapVm>(); }
        //}

        public EHealthVm EHealth
        {
            get { return ServiceLocator.Current.GetInstance<EHealthVm>(); }
        }

        public EpidemVm Epidem
        {
            get { return ServiceLocator.Current.GetInstance<EpidemVm>(); }
        }

        public static void Cleanup()
        {
            //Main.Cleanup();
            // TODO Clear the ViewModels
        }
    }
}