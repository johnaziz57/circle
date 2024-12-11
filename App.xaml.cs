using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Circle_2.Hotkeys;
using NHotkey;

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
            
        }

        private void ComponentDispatcher_ThreadPreprocessMessage(ref MSG msg, ref bool handled)
        {
            if (msg.message == HotkeysManager.WM_HOTKEY && msg.wParam.ToInt32() == HotkeysManager.HOTKEY_ID)
            {
                MessageBox.Show("Global hotkey triggered!");
                handled = true;
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            // Unregister the hotkey
            var hWnd = new System.Windows.Interop.WindowInteropHelper(new Window()).Handle;
            HotkeysManager.UnregisterHotKey(hWnd);
        }
    }

}
