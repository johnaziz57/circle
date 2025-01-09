This repo is just the MacOS amazing [Rectangle](https://github.com/rxhanson/Rectangle) program but for Windows called Circle
Windows 11 already have the support for this. Just use (win + direction) to move windows around.

## Sad backstory
### (Please play your smallest violin when reading this to get in the mood)

I switched my main personal driver from MacOS to Windows. 
Besides the obvious pitfalls of Windows and Microsoft as a company in general, I was furious that Windows 10 doesn't have any keyboard shortcuts
for moving windows around when using multiple monitors. 
I also don't want to upgrade to Windows 11 which is full of Microsft lovely even more toxic bloatware.
So here we are, I embarked on my own journey to implement something similar.

This is by no way finished, there are a few things still to do.
I am not sure I have enough motiviation to finish it because Windows 10 is being used less and less every day and Windows 11 supports this by default.
For now, I think it just does the job.

Thanks for reading :bow:

## How to use
Just clone this sad slob of code and open it in Visual Studio (or whatever IDE that you like to torture yourself with) and then build and voila.
Can be easier, but I don't care that much yet.


### TODO

1. Save/Register the new hotkeys after they have changed
1. Move the recording logic within the clearable textbox
1. Fix textbox background colour doesn't change back when pressing ESC
1. Allow only one non-modifier key when recording shortcuts
1. Make sure the sender of any events is `ClearableTextBox` instead of the inner component like TextBox or CheckBox
1. Make better icons for the app
1. When recording, remove the keys when key up event happens
1. Handle moving window to different monitor when the window doesn't resize
1. Show snappable areas when moving windows ??



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
1. Handle multiple recording happening at the same time case (stop the recording of other fields)
1. Give the text box different background when recording
1. Give the text box a different value when it is waiting for recording
1. Fix startup shortcut