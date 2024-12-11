using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using NHotkey;
using NHotkey.Wpf;

namespace Circle_2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private static readonly KeyGesture LeftGesture = new KeyGesture(Key.Left, ModifierKeys.Windows | ModifierKeys.Alt);
        private static readonly KeyGesture RightGesture = new KeyGesture(Key.Right, ModifierKeys.Windows | ModifierKeys.Alt);

        /// TODO here
        /// 
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            HotkeyManager.HotkeyAlreadyRegistered += HotkeyManager_HotkeyAlreadyRegistered;

            HotkeyManager.Current.AddOrReplace("LeftGesture", LeftGesture, OnIncrement);
            HotkeyManager.Current.AddOrReplace("RightGesture", RightGesture, OnDecrement);

        }

        private void HotkeyManager_HotkeyAlreadyRegistered(object sender, HotkeyAlreadyRegisteredEventArgs e)
        {
            MessageBox.Show(string.Format("The hotkey {0} is already registered by another application", e.Name));
        }

        private void OnIncrement(object sender, HotkeyEventArgs e)
        {
            MessageBox.Show(string.Format("The hotkey {0} has triggered OnIncrement", e.Name)); ;
            e.Handled = true;
        }

        private void OnDecrement(object sender, HotkeyEventArgs e)
        {
            MessageBox.Show(string.Format("The hotkey {0} has triggered OnDecrement", e.Name)); ;
            e.Handled = true;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }

}
