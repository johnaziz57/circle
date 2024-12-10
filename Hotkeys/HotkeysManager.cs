using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Circle_2.Hotkeys
{
    public static class HotkeysManager
    {
        #region WinAPI Functions

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // Windows message type when the hot key is triggered
        public const int WM_HOTKEY = 0x0312;

        // Modifier key constants
        public const int HOTKEY_ID = 9000; // A unique ID for the hotkey

        #endregion

        public static bool RegisterHotKey(
            WindowInteropHelper helper,
            HotKeyModifier hotKeyModifier,
            Key key)
        {
            return RegisterHotKey(helper.Handle, HOTKEY_ID, (uint)hotKeyModifier, (int)KeyInterop.VirtualKeyFromKey(key));
        }


        public static bool UnregisterHotKey(WindowInteropHelper helper)
        {
            return UnregisterHotKey(helper.Handle, HOTKEY_ID);
        }
    }
}
