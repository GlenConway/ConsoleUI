using System;

namespace ConsoleUI
{
    public class TextBox : Control
    {
        public TextBox()
        {
            TabStop = true;
        }

        public int MaxLength { get; set; }

        protected override void OnEnter()
        {
            Console.ForegroundColor = ForegroundColor;
            Console.BackgroundColor = BackgroundColor;

            base.OnEnter();

            if (!string.IsNullOrEmpty(Text))
                Console.CursorLeft = Text.Length;

            if (BorderStyle != BorderStyle.None)
            {
                Console.CursorTop = Console.CursorTop + 1;
                Console.CursorLeft = Console.CursorLeft + 1;
            }

            ReadKey();
        }

        private void ReadKey()
        {
            while (true)
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

                            OnTabPressed(false);

                            return;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            if (!string.IsNullOrEmpty(Text))
                            {
                                // get the location of the cursor
                                int pos = Console.CursorLeft;

                                if (pos > 0)
                                    // move the cursor to the left by one character
                                    Console.SetCursorPosition(pos - 1, Console.CursorTop);
                            }

                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            if (!string.IsNullOrEmpty(Text))
                            {
                                // get the location of the cursor
                                int pos = Console.CursorLeft;

                                if (pos < Text.Length)
                                    // move the cursor to the right by one character
                                    Console.SetCursorPosition(pos + 1, Console.CursorTop);
                            }

                            break;
                        }
                    case ConsoleKey.Backspace:
                        {
                            if (!string.IsNullOrEmpty(Text))
                            {
                                // get the location of the cursor
                                int pos = Console.CursorLeft;

                                // remove one character from the list of characters
                                Text = Text.Substring(0, Text.Length - 1);

                                // move the cursor to the left by one character
                                Console.SetCursorPosition(pos - 1, Console.CursorTop);
                                // replace it with space
                                Console.Write(" ");
                                // move the cursor to the left by one character again
                                Console.SetCursorPosition(pos - 1, Console.CursorTop);
                            }

                            break;
                        }
                    default:
                        {
                            int pos = Console.CursorLeft;

                            if (BorderStyle != BorderStyle.None)
                                pos--;

                            if (Text == null || ((pos < ClientWidth & pos < MaxLength) || (pos < ClientWidth & MaxLength == 0)))
                            {
                                Console.Write(info.KeyChar);

                                if (Text == null || pos == Text.Length)
                                    Text += info.KeyChar;
                                else
                                    Text = Text.Insert(pos, info.KeyChar.ToString());
                            }

                            break;
                        }
                }
            }
        }
    }
}