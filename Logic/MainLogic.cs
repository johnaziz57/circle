using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Circle_2.Models;
using Circle_2.Utils;
using Newtonsoft.Json;
using NHotkey;
using NHotkey.Wpf;
using System.Linq;

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
            LoadGestures();
        }

        private void LoadGestures()
        {
            List<Gesture> gestures = LoadPreferences();

            foreach (var gesture in gestures)
            {
                Trace.WriteLine(gesture.type);
                if (gesture.shortcut == null) continue;
                var shortcut = gesture.shortcut;

                ModifierKeys modifiers = shortcut.modifierKeys.Aggregate((current, next) => current | next); ;
                KeyGesture keyGesture = new KeyGesture(gesture.shortcut.key, modifiers);
                EventHandler<HotkeyEventArgs> handler = GetGestureHandler(gesture.type);
                HotkeyManager.Current.AddOrReplace(gesture.type.ToString(), keyGesture, handler);
            }
        }


        private List<Gesture> LoadPreferences()
        {
            string preferencesPath = "preferences.json";
            if (!File.Exists(preferencesPath))
            {
                List<Gesture> newGestures = [
                    new Gesture(GestureType.MoveLeft, new Shortcut(Key.Left, [ModifierKeys.Windows, ModifierKeys.Alt])),
                    new Gesture(GestureType.MoveRight, new Shortcut(Key.Right, [ModifierKeys.Windows, ModifierKeys.Alt])),
                    new Gesture(GestureType.MoveTop, new Shortcut(Key.Up, [ModifierKeys.Windows, ModifierKeys.Alt])),
                    new Gesture(GestureType.MoveBottom, new Shortcut(Key.Down, [ModifierKeys.Windows, ModifierKeys.Alt])),
                    new Gesture(GestureType.Maximize, new Shortcut(Key.Enter, [ModifierKeys.Windows, ModifierKeys.Alt]))
                ];

                string json = JsonConvert.SerializeObject(newGestures, Formatting.Indented);

                // Write the JSON data to a new file
                File.WriteAllText(preferencesPath, json);

                Trace.WriteLine("Preferences file created with an empty list of gestures.");
            }

            using StreamReader r = new StreamReader(preferencesPath);
            List<Gesture> savedGestures = JsonConvert.DeserializeObject<List<Gesture>>(r.ReadToEnd()) ?? [];
            Trace.WriteLine(savedGestures.ToString());
            return savedGestures;
        }

        private EventHandler<HotkeyEventArgs>? GetGestureHandler(GestureType type)
        {
            switch (type)
            {
                case GestureType.MoveLeft:
                    return OnMoveWindowLeftGesture;  
                case GestureType.MoveRight:
                    return OnMoveWindowRightGesture;
                case GestureType.MoveTop:
                    return OnMoveWindowTopGesture; 
                case GestureType.MoveBottom:
                    return OnMoveWindowBottomGesture;
                case GestureType.Maximize:
                    return OnMaximizeWindowGesture;
                default:
                    return null;
            }
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
