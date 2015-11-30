using System;

namespace ConsoleUI
{
    public class Button : InputControl
    {
        public Button()
        {
            TabStop = true;
            Width = 5;
            Height = 1;

            FocusBackgroundColor = ConsoleColor.White;
            FocusForegroundColor = ConsoleColor.Blue;
        }

        public event EventHandler Click;

        protected override void DrawShadow()
        {
            if (!HasShadow)
                return;

            for (int i = Left + 1; i <= Right + 1; i++)
            {
                Owner.Buffer.Write((short)i, (short)Bottom + 1, 223, ConsoleColor.Black);
            }

            for (int i = Top; i <= Bottom; i++)
            {
                Owner.Buffer.Write((short)Right + 1, (short)i, 220, ConsoleColor.Black);
            }
        }

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
            Paint();

            ReadKey();
        }

        protected override void OnLeave()
        {
            Console.CursorVisible = false;

            base.OnLeave();

            DrawText();
            Paint();
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