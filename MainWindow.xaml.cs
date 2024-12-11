using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Circle_2.Hotkeys;

namespace Circle_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Register the hotkey: Win + Alt + Enter
            var helper = new WindowInteropHelper(this);
            
            //if (!HotkeysManager.RegisterHotKey(helper, HotKeyModifier.WIN | HotKeyModifier.ALT, Key.Enter))
            //{
            //    MessageBox.Show("Failed to register hotkey.");
            //}

            //// Hook into WPF's message loop
            //HwndSource source = HwndSource.FromHwnd(helper.Handle);
            //source.AddHook(HwndHook);
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Unregister the hotkey when the application is closing
            var helper = new WindowInteropHelper(this);
            HotkeysManager.UnregisterHotKey(helper);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == HotkeysManager.WM_HOTKEY && wParam.ToInt32() == HotkeysManager.HOTKEY_ID)
            {
                // Hotkey triggered
                MessageBox.Show("Hotkey triggered: Win + Alt + Enter");
                handled = true; // Mark the message as handled
            }

            return IntPtr.Zero;
        }
    }
}