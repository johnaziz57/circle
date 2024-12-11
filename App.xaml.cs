using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Circle_2.Logic;
using NHotkey;
using NHotkey.Wpf;

namespace Circle_2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        /// TODO here
        /// 
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            (new MainLogic()).OnAppLoaded();
            
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }

}
