using System.Runtime.InteropServices;


namespace Circle_2.Utils.Models
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MONITORINFO
    {
        public int cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public uint dwFlags;

        public const uint MONITORINFOF_PRIMARY = 1; // Flag to indicate primary monitor
    }
}
