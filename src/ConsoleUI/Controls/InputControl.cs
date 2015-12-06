using System;

namespace ConsoleUI
{
    public abstract class InputControl : Control
    {
        public ConsoleColor FocusBackgroundColor = ConsoleColor.Blue;

        public ConsoleColor FocusForegroundColor = ConsoleColor.Gray;

        public InputControl() : base()
        {
        }

        public event EventHandler Enter;

        public event EventHandler EscPressed;

        public event EventHandler Leave;

        public event EventHandler<TabEventArgs> TabPressed;

        public bool HasFocus { get; set; }
        public int TabOrder { get; set; }

        public bool TabStop { get; set; }

        public void Focus()
        {
            if (!TabStop)
                return;

            HasFocus = true;
            Console.CursorVisible = true;
            Console.CursorLeft = Left;
            Console.CursorTop = Top;

            OnEnter();
        }

        protected void Blur()
        {
            if (!TabStop)
                return;

            HasFocus = false;
            Console.CursorVisible = false;

            OnLeave();
        }

        protected virtual void OnEnter()
        {
            if (Enter != null)
                Enter(this, new EventArgs());
        }

        protected virtual void OnEscPressed()
        {
            if (EscPressed != null)
                EscPressed(this, new EventArgs());
        }

        protected virtual void OnLeave()
        {
            if (Leave != null)
                Leave(this, new EventArgs());
        }

        protected virtual void OnTabPressed(bool shift)
        {
            if (TabPressed != null)
                TabPressed(this, new TabEventArgs(shift));
        }

        protected override void OnWrite(int x, int y, string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            base.OnWrite(x, y, text, HasFocus ? FocusForegroundColor : foregroundColor, HasFocus ? FocusBackgroundColor : backgroundColor);
        }

        protected virtual void ReadKey()
        {
        }
    }
}