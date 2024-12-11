using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using NHotkey;
using NHotkey.Wpf;

namespace Circle_2.Logic
{
    public class MainLogic
    {
        private readonly KeyGesture LeftGesture = new KeyGesture(Key.Left, ModifierKeys.Windows | ModifierKeys.Alt);
        private readonly KeyGesture RightGesture = new KeyGesture(Key.Right, ModifierKeys.Windows | ModifierKeys.Alt);
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public override string ToString()
            {
                return $"RECT(Left: {Left}, Top: {Top}, Right: {Right}, Bottom: {Bottom})";
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;

            public const uint MONITORINFOF_PRIMARY = 1; // Flag to indicate primary monitor
        }

        public class MonitorInfo
        {
            public bool IsPrimary { get; set; }
            public RECT Bounds { get; set; }
            public RECT WorkArea { get; set; }

            public override string ToString()
            {
                return $"Monitor(IsPrimary: {IsPrimary}, Bounds: {Bounds}, WorkArea: {WorkArea})";
            }
        }

        public class NativeMethods
        {
            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        }

        public delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, IntPtr lprcMonitor, IntPtr dwData);

        private static List<MonitorInfo> GetMonitors()
        {
            List<MonitorInfo> monitors = new List<MonitorInfo>();

            MonitorEnumProc callback = (hMonitor, hdcMonitor, lprcMonitor, dwData) =>
            {
                MONITORINFO monitorInfo = new MONITORINFO { cbSize = Marshal.SizeOf(typeof(MONITORINFO)) };

                if (NativeMethods.GetMonitorInfo(hMonitor, ref monitorInfo))
                {
                    monitors.Add(new MonitorInfo
                    {
                        IsPrimary = (monitorInfo.dwFlags & MONITORINFO.MONITORINFOF_PRIMARY) != 0,
                        Bounds = monitorInfo.rcMonitor,
                        WorkArea = monitorInfo.rcWork
                    });
                }
                return true; // Continue enumeration
            };

            if (!NativeMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, callback, IntPtr.Zero))
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Unable to enumerate monitors.");
            }

            return monitors;
        }
        public void OnAppLoaded()
        {
            HotkeyManager.HotkeyAlreadyRegistered += HotkeyManager_HotkeyAlreadyRegistered;

            HotkeyManager.Current.AddOrReplace("LeftGesture", LeftGesture, OnIncrement);
            HotkeyManager.Current.AddOrReplace("RightGesture", RightGesture, OnDecrement);

            //MonitorHelper monitorHelper = new MonitorHelper();
            //var monitors = monitorHelper.GetMonitorsInfo();
            //foreach (var item in new MonitorHelper().GetMonitorsInfo())
            //{
            //    Console.WriteLine(item.ToString());
            //}
            //MessageBox.Show(string.Format("Monitor Count is {0} {1}", monitors, monitors.Count));

            List<MonitorInfo> monitors = GetMonitors();

            foreach (var monitor in monitors)
            {
                Console.WriteLine(monitor);
            }
            MessageBox.Show(string.Format("Monitor Count is {0} {1}", monitors, monitors.Count));
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
    }
}
