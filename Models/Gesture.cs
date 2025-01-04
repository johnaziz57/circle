using System.Windows.Input;

namespace Circle_2.Models
{
    public enum GestureType
    {
        MoveLeft, MoveRight, MoveTop, MoveBottom, Maximize
    }
    public class Gesture
    {
        public readonly GestureType type;
        public readonly Shortcut? shortcut;

        public Gesture(GestureType type, Shortcut? shortcut)
        {
            this.type = type;
            this.shortcut = shortcut;
        }
    }

    public class Shortcut
    {
        public readonly Key key;
        public readonly List<ModifierKeys> modifierKeys;

        public Shortcut(Key key, List<ModifierKeys> modifierKeys)
        {
            this.key = key;
            this.modifierKeys = modifierKeys;
        }
    }
}
