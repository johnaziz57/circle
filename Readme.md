### TODO
1. Handle multiple recording happening at the same time case (stop the recording of other fields)
1. Give the text box different background when recording
1. Give the text box a different value when it is waiting for recording
1. Make sure the sender of any events is `ClearableTextBox` instead of the inner component like TextBox or CheckBox
1. Save/Register the new hotkeys after they have changed
1. Make better icons for the app
1. When recording, remove the keys when key up event happens
1. Allow only one non-modifier key when recording shortcuts


### Done
1. Record keys
2. override all system wide hotkey presses
3. make recording work with the rest of the fields
1. LWin + Direction doesn't work -> Now fixed
1. TODO Add `WM_SYSKEYDOWN 0x0104` to the recording Util -> Ignored the condition all together
1. Improve the distance between the textbox and the buttons
1. Remove the buttons or add (x) at the end of the text field
1. Stop recording when the whole window loses focus
1. Improve the recording of the shortcut to record as many modifiers but once you enter a non modifier key it stops
1. Clear recording when users press the x button (Stop recording from starting recording when that happens)