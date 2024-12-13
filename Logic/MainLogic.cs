using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Circle_2.Models;
using Circle_2.Utils;
using Circle_2.Utils.Models;
using NHotkey;
using NHotkey.Wpf;

namespace Circle_2.Logic
{
    public class MainLogic
    {
        private readonly KeyGesture LeftGesture = new KeyGesture(Key.Left, ModifierKeys.Windows | ModifierKeys.Alt);
        private readonly KeyGesture RightGesture = new KeyGesture(Key.Right, ModifierKeys.Windows | ModifierKeys.Alt);

        private readonly MonitorHelper monitorHelper = new MonitorHelper();
        private readonly WindowHelper windowHelper = new WindowHelper();


        public void OnAppLoaded()
        {
            HotkeyManager.HotkeyAlreadyRegistered += HotkeyManager_HotkeyAlreadyRegistered;

            HotkeyManager.Current.AddOrReplace("LeftGesture", LeftGesture, OnMoveWindowLeftGesture);
            HotkeyManager.Current.AddOrReplace("RightGesture", RightGesture, OnMoveWindowRightGesture);
        }

        private void HotkeyManager_HotkeyAlreadyRegistered(object sender, HotkeyAlreadyRegisteredEventArgs e)
        {
            MessageBox.Show(string.Format("The hotkey {0} is already registered by another application", e.Name));
        }

        private void OnMoveWindowLeftGesture(object sender, HotkeyEventArgs e)
        {
            MoveWindowToLeft();
            e.Handled = true;
        }

        private void OnMoveWindowRightGesture(object sender, HotkeyEventArgs e)
        {
            MoveWindowToRight();
            e.Handled = true;
        }

        private void MoveWindowToLeft(IntPtr? windowHandle = null, MonitorInfo? monitorInfo = null)
        {
            var lWindowHandle = windowHandle ?? WindowHelper.GetForegroundWindow();
            var lMonitorInfo = monitorInfo ?? monitorHelper.GetCurrentMonitor(lWindowHandle);


            windowHelper.ShowWindow(lWindowHandle);
            var workArea = lMonitorInfo.WorkArea;
            var newWidth = (workArea.Right - workArea.Left) / 2;
            var newHeight = workArea.Bottom - workArea.Top;

            var windowBounds = windowHelper.GetWindowBounds(lWindowHandle);
            if (windowBounds.Equals(workArea.Left, workArea.Top, newWidth, newHeight))
            {
                MoveWindowToRight(lWindowHandle, lMonitorInfo);
            }
            else
            {
                windowHelper.MoveWindow(lWindowHandle, workArea.Left, workArea.Top, newWidth, newHeight);
            }
        }

        private void MoveWindowToRight(IntPtr? windowHandle = null, MonitorInfo? monitorInfo = null)
        {
            var lWindowHandle = windowHandle ?? WindowHelper.GetForegroundWindow();
            var lMonitorInfo = monitorInfo ?? monitorHelper.GetCurrentMonitor(lWindowHandle);


            windowHelper.ShowWindow(lWindowHandle);
            var workArea = lMonitorInfo.WorkArea;
            var newWidth = (workArea.Right - workArea.Left) / 2;
            var newHeight = workArea.Bottom - workArea.Top;
            var newLeft = workArea.Left + newWidth;

            var windowBounds = windowHelper.GetWindowBounds(lWindowHandle);
            if (windowBounds.Equals(newLeft, workArea.Top, newWidth, newHeight))
            {
                MoveWindowToLeft(lWindowHandle, lMonitorInfo);
            }
            else
            {
                windowHelper.MoveWindow(lWindowHandle, newLeft, workArea.Top, newWidth, newHeight);
            }
        }
    }
}
