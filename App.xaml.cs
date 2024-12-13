using System.Windows;
using Circle_2.Logic;

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
