using System.Runtime.InteropServices;
using Circle_2.Models;
using System.Windows;

namespace Circle_2.Utils
{
    public class MonitorHelper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

        private delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, IntPtr lprcMonitor, IntPtr dwData);


        public const uint FLAG_MONITOR_IS_PRIMARY = 1;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public uint dwFlags = 0;
        }

        public List<MonitorInfo> GetMonitorsInfo()
        {
            List<MonitorInfo> monitors = new List<MonitorInfo>();


            MonitorEnumProc callback = (hMonitor, hdcMonitor, lprcMonitor, dwData) =>
            {
                MONITORINFO monitorInfo = new MONITORINFO { cbSize = Marshal.SizeOf(typeof(MONITORINFO)) };

                if (GetMonitorInfo(hMonitor, ref monitorInfo))
                {
                    monitors.Add(new MonitorInfo
                    (
                        hMonitor,
                        (monitorInfo.dwFlags & FLAG_MONITOR_IS_PRIMARY) != 0,
                        new Rectangle
                        (
                           monitorInfo.rcMonitor.Left,
                           monitorInfo.rcMonitor.Top,
                           monitorInfo.rcMonitor.Right,
                           monitorInfo.rcMonitor.Bottom
                        ),
                        new Rectangle
                        (
                            monitorInfo.rcWork.Left,
                            monitorInfo.rcWork.Top,
                            monitorInfo.rcWork.Right,
                            monitorInfo.rcWork.Bottom
                        )
                    ));
                }

                return true; // Continue enumeration
            };

            if (!EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, callback, IntPtr.Zero))
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Unable to enumerate monitors.");
            }

            return monitors;
        }
    }
}
