using System.Windows;
using System.Windows.Input;
using Circle_2.Models;
using Circle_2.Utils;
using NHotkey;
using NHotkey.Wpf;

namespace Circle_2.Logic
{
    public class MainLogic
    {
        private readonly KeyGesture LeftGesture = new KeyGesture(Key.Left, ModifierKeys.Windows | ModifierKeys.Alt);
        private readonly KeyGesture RightGesture = new KeyGesture(Key.Right, ModifierKeys.Windows | ModifierKeys.Alt);
        private readonly KeyGesture MaximizeGesture = new KeyGesture(Key.Enter, ModifierKeys.Windows | ModifierKeys.Alt);
        private readonly KeyGesture TopGesture = new KeyGesture(Key.Up, ModifierKeys.Windows | ModifierKeys.Alt);
        private readonly KeyGesture BottomGetsure = new KeyGesture(Key.Down, ModifierKeys.Windows | ModifierKeys.Alt);

        private readonly MonitorHelper monitorHelper = new MonitorHelper();
        private readonly WindowHelper windowHelper = new WindowHelper();


        public void OnAppLoaded()
        {
            HotkeyManager.HotkeyAlreadyRegistered += HotkeyManager_HotkeyAlreadyRegistered;

            HotkeyManager.Current.AddOrReplace("LeftGesture", LeftGesture, OnMoveWindowLeftGesture);
            HotkeyManager.Current.AddOrReplace("RightGesture", RightGesture, OnMoveWindowRightGesture);
            HotkeyManager.Current.AddOrReplace("MaximizeGesture", MaximizeGesture, OnMaximizeWindowGesture);
            HotkeyManager.Current.AddOrReplace("TopGesture", TopGesture, OnMoveWindowTopGesture);
            HotkeyManager.Current.AddOrReplace("BottomGetsure", BottomGetsure, OnMoveWindowBottomGesture);
        }

        private void HotkeyManager_HotkeyAlreadyRegistered(object sender, HotkeyAlreadyRegisteredEventArgs e)
        {
            MessageBox.Show(string.Format("The hotkey {0} is already registered by another application", e.Name));
        }

        private void OnMoveWindowLeftGesture(object? sender, HotkeyEventArgs e)
        {
            MoveWindowToLeft();
            e.Handled = true;
        }

        private void OnMoveWindowRightGesture(object? sender, HotkeyEventArgs e)
        {
            MoveWindowToRight();
            e.Handled = true;
        }

        private void OnMaximizeWindowGesture(object? sender, HotkeyEventArgs e)
        {
            OnMaximizeWindowGesture();
            e.Handled = true;

        }

        private void OnMoveWindowTopGesture(object? sender, HotkeyEventArgs e)
        {
            OnMoveWindowTopGesture();
            e.Handled = true;

        }

        private void OnMoveWindowBottomGesture(object? sender, HotkeyEventArgs e)
        {
            OnMoveWindowBottomGesture();
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
                MoveWindowToRight(lWindowHandle, monitorHelper.GetNextMonitor(lMonitorInfo));
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
                MoveWindowToLeft(lWindowHandle, monitorHelper.GetPreviousMonitor(lMonitorInfo));
            }
            else
            {
                windowHelper.MoveWindow(lWindowHandle, newLeft, workArea.Top, newWidth, newHeight);
            }
        }
        private void OnMaximizeWindowGesture(IntPtr? windowHandle = null, MonitorInfo? monitorInfo = null)
        {
            var lWindowHandle = windowHandle ?? WindowHelper.GetForegroundWindow();
            var lMonitorInfo = monitorInfo ?? monitorHelper.GetCurrentMonitor(lWindowHandle);


            windowHelper.MaximizeWindow(lWindowHandle);
        }

        private void OnMoveWindowTopGesture(IntPtr? windowHandle = null, MonitorInfo? monitorInfo = null)
        {
            var lWindowHandle = windowHandle ?? WindowHelper.GetForegroundWindow();
            var lMonitorInfo = monitorInfo ?? monitorHelper.GetCurrentMonitor(lWindowHandle);


            windowHelper.ShowWindow(lWindowHandle);
            var workArea = lMonitorInfo.WorkArea;
            var newWidth = workArea.Right - workArea.Left;
            var newHeight = (workArea.Bottom - workArea.Top) / 2;

            windowHelper.MoveWindow(lWindowHandle, workArea.Left, workArea.Top, newWidth, newHeight);
        }
        private void OnMoveWindowBottomGesture(IntPtr? windowHandle = null, MonitorInfo? monitorInfo = null)
        {
            var lWindowHandle = windowHandle ?? WindowHelper.GetForegroundWindow();
            var lMonitorInfo = monitorInfo ?? monitorHelper.GetCurrentMonitor(lWindowHandle);


            windowHelper.ShowWindow(lWindowHandle);
            var workArea = lMonitorInfo.WorkArea;
            var newWidth = workArea.Right - workArea.Left;
            var newHeight = (workArea.Bottom - workArea.Top) / 2;
            var newTop = workArea.Top + newHeight;


            windowHelper.MoveWindow(lWindowHandle, workArea.Left, newTop, newWidth, newHeight);
        }
    }
}
