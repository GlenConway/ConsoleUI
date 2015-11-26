using System;

namespace ConsoleUI
{
    public class Button : InputControl
    {
        public Button()
        {
            TabStop = true;
            Width = 5;
            Height = 3;

            FocusBackgroundColor = ConsoleColor.White;
            FocusForegroundColor = ConsoleColor.Blue;
        }

        public event EventHandler Click;

        protected virtual void OnClick()
        {
            if (Click != null)
                Click(this, new EventArgs());
        }

        protected override void OnEnter()
        {
            Console.CursorVisible = false;

            base.OnEnter();

            DrawText();

            OnRepaint();

            ReadKey();
        }

        protected override void OnLeave()
        {
            base.OnLeave();

            OnRepaint();
        }

        protected override void ReadKey()
        {
            while (HasFocus)
            {
                ConsoleKeyInfo info = Console.ReadKey(true);

                switch (info.Key)
                {
                    case ConsoleKey.Escape:
                        {
                            Blur();

                            OnEscPressed();

                            return;
                        }
                    case ConsoleKey.Tab:
                        {
                            Blur();

                            OnTabPressed(info.Modifiers.HasFlag(ConsoleModifiers.Shift));

                            return;
                        }
                    case ConsoleKey.Enter:
                        {
                            Blur();

                            OnClick();

                            return;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            Blur();

                            OnTabPressed(false);

                            return;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            Blur();

                            OnTabPressed(true);

                            return;
                        }
                }
            }
        }
    }
}