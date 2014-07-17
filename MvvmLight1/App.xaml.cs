using System.Windows;
using GalaSoft.MvvmLight.Threading;
using System.Configuration;

namespace MvvmLight1
{
    /// <summary>
    /// Interaction logic App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
