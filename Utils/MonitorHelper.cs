using System.Runtime.InteropServices;
using Circle_2.Models;
using System.Windows;
using System.Threading;
using Circle_2.Utils.Models;

namespace Circle_2.Utils
{
    public class MonitorHelper
    {

        private delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, IntPtr lprcMonitor, IntPtr dwData);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);


        private const uint FLAG_MONITOR_IS_PRIMARY = 0x1;
        private const uint MONITOR_DEFAULT_TO_NEAREST = 0x2;

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
                        Rectangle.CreateFromRECT(monitorInfo.rcMonitor),
                        Rectangle.CreateFromRECT(monitorInfo.rcWork)
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

        public MonitorInfo GetCurrentMonitor(IntPtr windowHandle)
        {
            IntPtr monitorHandle = MonitorFromWindow(windowHandle, MONITOR_DEFAULT_TO_NEAREST);

            if (monitorHandle == IntPtr.Zero)
            {
                throw new Exception(string.Format("Unable to get monitor for window with handle {0}", windowHandle));
            }

            MONITORINFO monitorInfo = new MONITORINFO { cbSize = Marshal.SizeOf(typeof(MONITORINFO)) };

            if (!GetMonitorInfo(monitorHandle, ref monitorInfo))
            {
                throw new Exception(string.Format("Unable to get monitor info for monitor with handle {0}", monitorHandle));
            }
            return new MonitorInfo
            (
                monitorHandle,
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
            );
        }

        public MonitorInfo GetNextMonitor(MonitorInfo currentMonitorInfo)
        {
            var monitors = GetMonitorsInfo();
            var currentMonitorIndex = 0;

            foreach (MonitorInfo monitorInfo in monitors)
            {
                if (currentMonitorInfo.Handle >= monitorInfo.Handle) { break; }
                currentMonitorIndex++;
            }
            return monitors[(currentMonitorIndex + 1) % monitors.Count];
        }

        public MonitorInfo GetPreviousMonitor(MonitorInfo currentMonitorInfo)
        {
            var monitors = GetMonitorsInfo();
            var currentMonitorIndex = 0;

            foreach (MonitorInfo monitorInfo in monitors)
            {
                if (currentMonitorInfo.Handle >= monitorInfo.Handle) { break; }
                currentMonitorIndex++;
            }
            return monitors[(currentMonitorIndex - 1) % monitors.Count];
        }
    }
}
