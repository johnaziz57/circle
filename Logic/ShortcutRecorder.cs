using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Circle_2.Utils;

namespace Circle_2.Logic
{
    class ShortcutRecorder
    {
        public enum ShortcutEvent
        {
            Finished,
            Cancelled,
            Updated,
        }

        public delegate void OnShortcutChanged(ShortcutEvent shortcutEvent, List<Key> keys);

        private KeyboardHelper keyboardHelper = new KeyboardHelper();
        private HashSet<Key> pressedKeys = new HashSet<Key>(); // To track pressed keys
        private bool hasModifierKey = false;
        private bool hasNonModifierKey = false;

        public void StartRecording(OnShortcutChanged onShortcutChanged)
        {
            ResetRecording();

            keyboardHelper.StartRecording((key) =>
            {
                Trace.WriteLine("Started recording");
                if (key == Key.Escape)
                {
                    StopRecording();
                    onShortcutChanged(ShortcutEvent.Cancelled, pressedKeys.ToList());
                    return;
                }
                if (IsModifierKey(key))
                {
                    hasModifierKey = true;
                }
                else
                {
                    hasNonModifierKey = true;
                }
                pressedKeys.Add(key);
                onShortcutChanged(ShortcutEvent.Updated, pressedKeys.ToList());
                Trace.WriteLine("Pressed keys: " + pressedKeys.ToString());
                if (hasModifierKey && hasNonModifierKey)
                {
                    Trace.WriteLine("Finished recording. Has both needed keys: " + pressedKeys.ToString());
                    onShortcutChanged(ShortcutEvent.Finished, pressedKeys.ToList());
                    StopRecording();
                }
                else
                {
                    Trace.WriteLine("Keep recording. Doesn't have both keys: " + pressedKeys.ToString());

                }
            });
        }

        public void StopRecording()
        {
            keyboardHelper.StopRecording();
        }

        private bool IsModifierKey(Key key)
        {
            switch (key)
            {
                case Key.LeftCtrl:
                case Key.RightCtrl:
                case Key.RightShift:
                case Key.LeftShift:
                case Key.LeftAlt:
                case Key.RightAlt:
                case Key.LWin:
                case Key.RWin:
                    return true;
                default:
                    return false;
            }
        }

        private void ResetRecording()
        {
            hasModifierKey = false;
            hasNonModifierKey = false;
            pressedKeys.Clear();
        }

        private string FormatShortcutKeys(List<Key> keys) // TOOD only for testing
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
    }
}
