using System.Runtime.InteropServices;
using Circle_2.Models;
using Circle_2.Utils.Models;

namespace Circle_2.Utils
{
    internal class WindowHelper
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        private const int WINDOW_RESTORE = 9;
        private const int WINDOW_MAXIMIZE = 3;


        public void MoveWindow(IntPtr hWnd, int left, int top, int width, int height)
        {
            if (!MoveWindow(hWnd, left, top, width, height, true))
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Failed to move or resize the window.");
            }
        }

        public void ShowWindow(IntPtr windowHandle)
        {
            if (!ShowWindow(windowHandle, WINDOW_RESTORE))
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Failed to show the window.");
            }
        }

        public Rectangle GetWindowBounds(IntPtr windowHandle)
        {
            RECT rect = new RECT();
            if (!GetWindowRect(windowHandle, ref rect))
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Failed to get window rectangle");
            }

            return Rectangle.CreateFromRECT(rect);
        }

        public bool MaximizeWindow(IntPtr windowHandle)
        {
            return ShowWindow(windowHandle, WINDOW_MAXIMIZE);
        }
    }
}
