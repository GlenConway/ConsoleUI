using System;

namespace ConsoleUI
{
    public class TextBox : Control
    {
        private string originalText;

        public TextBox()
        {
            TabStop = true;
            Width = 5;
            Height = 1;
            TreatEnterKeyAsTab = true;
        }

        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        public event EventHandler<TextChangedEventArgs> TextChanged;

        public int MaxLength { get; set; }

        public bool TreatEnterKeyAsTab { get; set; }

        private int Position
        {
            get
            {
                return Console.CursorLeft - ClientLeft;
            }
        }

        protected override void OnEnter()
        {
            Console.ForegroundColor = ForegroundColor;
            Console.BackgroundColor = BackgroundColor;

            base.OnEnter();

            Console.CursorLeft = ClientLeft;
            Console.CursorTop = ClientTop;

            if (!string.IsNullOrEmpty(Text))
                Console.CursorLeft += Text.Length;

            originalText = Text;

            ReadKey();
        }

        protected virtual bool OnKeyPressed(ConsoleKeyInfo info)
        {
            if (KeyPressed != null)
            {
                var args = new KeyPressedEventArgs(info);

                KeyPressed(this, args);

                return args.Handled;
            }

            return false;
        }

        protected virtual void OnTextChanged()
        {
            if (originalText == Text)
                return;

            if (TextChanged != null)
                TextChanged(this, new TextChangedEventArgs(originalText, Text));

            OnRepaint();

            originalText = Text;
        }

        private void ReadKey()
        {
            while (HasFocus)
            {
                ConsoleKeyInfo info = Console.ReadKey(true);

                if (OnKeyPressed(info))
                    continue;

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
                            OnTextChanged();

                            Blur();

                            OnTabPressed(info.Modifiers.HasFlag(ConsoleModifiers.Shift));

                            return;
                        }
                    case ConsoleKey.Enter:
                        {
                            OnTextChanged();

                            if (TreatEnterKeyAsTab)
                            {
                                Blur();

                                OnTabPressed(false);

                                return;
                            }

                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            if (!string.IsNullOrEmpty(Text))
                            {
                                if (Position > 0)
                                    // move the cursor to the left by one character
                                    Console.SetCursorPosition(Position - 1, Console.CursorTop);
                            }

                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            if (!string.IsNullOrEmpty(Text))
                            {
                                if (Position < Text.Length)
                                    // move the cursor to the right by one character
                                    Console.SetCursorPosition(Position + 1, Console.CursorTop);
                            }

                            break;
                        }
                    case ConsoleKey.Backspace:
                        {
                            if (!string.IsNullOrEmpty(Text))
                            {
                                // remove one character from the list of characters
                                Text = Text.Substring(0, Text.Length - 1);

                                // move the cursor to the left by one character
                                Console.SetCursorPosition(Position - 1 + ClientLeft, Console.CursorTop);
                                // replace it with space
                                Console.Write(" ");
                                // move the cursor to the left by one character again
                                Console.SetCursorPosition(Position - 1 + ClientLeft, Console.CursorTop);
                            }

                            break;
                        }
                    default:
                        {
                            if (char.IsControl(info.KeyChar))
                                break;

                            if (Text == null || ((Position < ClientWidth & Position < MaxLength) || (Position < ClientWidth & MaxLength == 0)))
                            {
                                if (Text == null || Position == Text.Length)
                                    Text += info.KeyChar;
                                else
                                    Text = Text.Insert(Position, info.KeyChar.ToString());

                                Console.Write(info.KeyChar);
                            }

                            break;
                        }
                }
            }
        }
    }
}