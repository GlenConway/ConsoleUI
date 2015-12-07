using System;

namespace ConsoleUI
{
    public abstract class InputControl : Control
    {
        public InputControl() : base()
        {
        }

        protected override void OnEnter()
        {
            Console.CursorVisible = true;
            Console.CursorLeft = Left;
            Console.CursorTop = Top;

            base.OnEnter();
        }

        protected override void OnLeave()
        {
            Console.CursorVisible = false;

            base.OnLeave();
        }
    }
}