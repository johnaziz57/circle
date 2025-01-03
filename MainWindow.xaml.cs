using System.Windows;
using System.IO;
using System.Windows.Input;
using System.Windows.Controls;
using Circle_2.Utils;
using System.Diagnostics;
using Circle_2.Components;
using Circle_2.Logic;
using static Circle_2.Logic.ShortcutRecorder;
using IWshRuntimeLibrary;

namespace Circle_2
{

    // TODO record keys here
    // TODO override all system wide hotkey presses
    // TODO make it work with the rest of the fields
    // TODO LWin + Direction doesn't work
    // TODO Add `WM_SYSKEYDOWN 0x0104` to the recording Util
    // Apply the recording functions to all other TextBoxes
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isRecording = false;
        private string textBoxValue = "";
        private TextBox? currentTextBox = null;
        private ShortcutRecorder shortcutRecorder = new ShortcutRecorder();

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
                string shortcutPath = Path.Combine(startupFolder, "Circle.lnk");

                WshShell shell = new WshShell();
                IWshShortcut shortcut = shell.CreateShortcut(shortcutPath);
                shortcut.TargetPath = Environment.ProcessPath;
                Trace.WriteLine("Shortcut Location: " + System.Reflection.Assembly.GetExecutingAssembly().Location);
                shortcut.Save();
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
                string shortcutPath = Path.Combine(startupFolder, "Circle-2.lnk");

                if (System.IO.File.Exists(shortcutPath))
                {
                    System.IO.File.Delete(shortcutPath);
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
            string shortcutPath = Path.Combine(startupFolder, "Circle-2.lnk");

            // Check if the shortcut file exists
            return System.IO.File.Exists(shortcutPath);
        }



        private void StartRecording(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("StartRecording: TextboxValue: " + textBoxValue);
            if (!(sender is TextBox))
            {
                return;
            }
            TextBox textBox = (TextBox)sender;
            currentTextBox = textBox;
            // Start recording when TextBox is clicked
            if (!isRecording) // TODO do something when the user is already recording
            {
                isRecording = true;
                textBoxValue = textBox.Text;
                Trace.WriteLine("StartRecording 2: TextboxValue: " + textBoxValue);

            }
            // Prevent default handling of keys like Tab
            e.Handled = true;
            shortcutRecorder.StartRecording((shortcutEvent, keys) =>
            {
                switch (shortcutEvent)
                {
                    case ShortcutEvent.Cancelled:
                        AbortRecording(textBox);
                        break;
                    case ShortcutEvent.Updated:
                        textBox.Text = FormatShortcutKeys(keys);
                        break;
                    case ShortcutEvent.Finished:
                        textBox.Text = FormatShortcutKeys(keys);
                        FinishRecording(textBox);
                        break;
                }
            });
        }

        private void OnLostKeyboardFocus(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("OnLostKeyboardFocus");
            if (currentTextBox != null)
            {
                AbortRecording(currentTextBox);
            }
        }

        private void OnWindowDeactivated(object sender, EventArgs e)
        {
            Trace.WriteLine("OnWindowDeactivated");
            if (currentTextBox != null)
            {
                AbortRecording(currentTextBox);
            }
        }

        private string FormatShortcutKeys(List<Key> keys)
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

        private void FinishRecording(TextBox textBox)
        {
            Trace.WriteLine("FinishRecording: TextboxValue: " + textBoxValue);
            CleanUpRecording();
            Keyboard.ClearFocus();
        }

        private void ClearRecording(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = "";
        }


        private void AbortRecording(TextBox textBox)
        {
            shortcutRecorder.StopRecording();
            textBox.Text = textBoxValue;

            CleanUpRecording();
        }

        private void CleanUpRecording()
        {
            isRecording = false;
            textBoxValue = "";
            currentTextBox = null;
        }
    }
}