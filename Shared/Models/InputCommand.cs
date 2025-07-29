namespace Shared.Models
{
    public enum InputCommandType
    {
        MouseMove,
        MouseDown,
        MouseUp,
        KeyDown,
        KeyUp
    }

    public class InputCommand
    {
        public InputCommandType CommandType { get; set; }
        public int X { get; set; }          // Mouse X coordinate (for mouse events)
        public int Y { get; set; }          // Mouse Y coordinate
        public int KeyCode { get; set; }    // Virtual key code (for keyboard events)
    }
}