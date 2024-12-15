using System.Drawing;
using System.Windows;
using Circle_2.Logic;
using Hardcodet.Wpf.TaskbarNotification;

namespace Circle_2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        MainWindow? mainWindow = null;
        TaskbarIcon? trayIcon = null;
        /// TODO here
        /// 
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            (new MainLogic()).OnAppLoaded();

            // Ensure TaskbarIcon is loaded
            trayIcon = (TaskbarIcon)FindResource("TrayIcon");

        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            trayIcon?.Dispose();
        }

        public void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            mainWindow = mainWindow ?? new MainWindow();
            mainWindow.Show();
        }

        public void Exit_Click(object sender, RoutedEventArgs e)
        {
            Current.Shutdown();
        }
    }

}
