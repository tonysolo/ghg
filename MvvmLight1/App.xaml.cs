using GalaSoft.MvvmLight.Threading;

namespace MvvmLight1
{
    /// <summary>
    ///     Interaction logic App.xaml
    /// </summary>
    public partial class App
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}