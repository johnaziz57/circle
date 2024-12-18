﻿using System.Windows;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Input;
using System.Windows.Controls;

namespace Circle_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isRecording = false;
        private HashSet<Key> pressedKeys = new HashSet<Key>(); // To track pressed keys
        private DateTime? lastRecordingTime = null;
        private string textBoxValue = "";

        public MainWindow()
        {
            InitializeComponent();
            StartupCheckbox.IsChecked = IsAppInStartupFolder();
        }

        public void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                // The link should be added here
                // C:\Users\John\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup
                string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                string shortcutPath = System.IO.Path.Combine(startupFolder, "Circle-2.lnk");

                // Path to the executable
                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

                using (StreamWriter writer = new StreamWriter(shortcutPath))
                {
                    writer.WriteLine("[InternetShortcut]");
                    writer.WriteLine($"URL=file:///{exePath.Replace("\\", "/")}");
                    writer.WriteLine("IconIndex=0");
                    writer.WriteLine($"IconFile={exePath}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to set startup: " + ex.Message);
            }
        }

        public void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                string shortcutPath = System.IO.Path.Combine(startupFolder, "Circle-2.lnk");

                if (File.Exists(shortcutPath))
                {
                    File.Delete(shortcutPath);
                    MessageBox.Show("Application removed from startup.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to remove startup: " + ex.Message);
            }
        }

        private bool IsAppInStartupFolder()
        {
            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string shortcutPath = System.IO.Path.Combine(startupFolder, "Circle-2.lnk");

            // Check if the shortcut file exists
            return File.Exists(shortcutPath);
        }

        private void RecordKeys(object sender, KeyEventArgs e)
        {
            // TODO override all system wide hotkey presses
            // TODO make it work with the rest of the fields
            // TODO LWin + Direction doesn't work
            // Apply the recording functions to all other TextBoxes
            if (!(sender is TextBox))
            {
                return;
            }
            TextBox textBox = (TextBox)sender;
            // Start recording when TextBox is clicked
            if (!isRecording)
            {
                isRecording = true;
                lastRecordingTime = DateTime.Now;
                textBoxValue = textBox.Text;
                pressedKeys.Clear();
            }

            // Prevent default handling of keys like Tab
            e.Handled = true;

            // Handle special keys: Enter and Escape
            if (e.Key == Key.Enter)
            {
                // Save the recorded keys to the TextBox
                textBox.Text = FormatShortcutKeys(pressedKeys);
                ResetRecording();
            }
            else if (e.Key == Key.Escape)
            {
                // Cancel recording and keep the previous value
                AbortRecording(textBox);
            }
            else
            {
                // Add the key to the set if it's not already there
                if ((DateTime.Now - (lastRecordingTime ?? DateTime.Now)).TotalMilliseconds > 500)
                {
                    pressedKeys.Clear();
                }
                if (!pressedKeys.Contains(e.Key))
                {
                    pressedKeys.Add(e.Key);
                    textBox.Text = FormatShortcutKeys(pressedKeys);
                    lastRecordingTime = DateTime.Now;
                }
            }
        }

        private void RemoveKeys(object sender, KeyEventArgs e)
        {
            // Optional: Remove keys that are released
            if (!(sender is TextBox))
            {
                return;
            }
            TextBox textBox = (TextBox)sender;
            pressedKeys.Remove(e.Key);
            textBox.Text = FormatShortcutKeys(pressedKeys);
        }

        private string FormatShortcutKeys(HashSet<Key> keys)
        {
            // Convert the keys to a display-friendly string
            List<string> keyNames = new();
            foreach (var key in keys)
            {
                if (key == Key.LeftCtrl || key == Key.RightCtrl) keyNames.Add("Ctrl");
                else if (key == Key.LeftAlt || key == Key.RightAlt) keyNames.Add("Alt");
                else if (key == Key.LeftShift || key == Key.RightShift) keyNames.Add("Shift");
                else keyNames.Add(key.ToString());
            }

            return string.Join(" + ", keyNames);
        }

        private void ResetRecording()
        {
            isRecording = false;
            pressedKeys.Clear();
            textBoxValue = "";
        }

        private void AbortRecording(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!(sender is TextBox))
            {
                return;
            }
            AbortRecording((TextBox)sender);
            e.Handled = true;
        }

        private void AbortRecording(TextBox textBox)
        {
            textBox.Text = textBoxValue;
            isRecording = false;
            pressedKeys.Clear();
            textBoxValue = "";
        }
    }
}