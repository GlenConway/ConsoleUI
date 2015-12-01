using System;

namespace ConsoleUI
{
    public class TextBox : InputControl
    {
        public char PasswordCharacter = '*';
        private int CursorLeft;
        private int CursorTop;
        private bool Insert = true;
        private string OriginalText;

        public TextBox()
        {
            TabStop = true;
            Width = 5;
            Height = 1;
            TreatEnterKeyAsTab = true;
        }

        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        public event EventHandler<TextChangedEventArgs> TextChanged;

        public string[] Lines { get; set; }
        public int MaxLength { get; set; }

        public override string Text
        {
            get
            {
                if (Lines == null)
                    return string.Empty;

                return string.Join("", Lines);
            }

            set
            {
                Lines = value.Split(Environment.NewLine.ToCharArray());
            }
        }

        public TextBoxType TextBoxType { get; set; }
        public bool TreatEnterKeyAsTab { get; set; }

        public bool WordWrap { get; set; }

        protected override void DrawText()
        {
            if (Lines == null || Lines.Length == 0)
            {
                Write(string.Empty);

                return;
            }

            if (TextBoxType != TextBoxType.Password)
            {
                foreach (var line in Lines)
                {
                    if (string.IsNullOrEmpty(line))
                        Write(string.Empty);
                    else
                    {
                        var chunks = line.SplitIntoChunks(ClientWidth);

                        foreach (var chunk in chunks)
                        {
                            Write(chunk);

                            if (TextBoxType != TextBoxType.Multiline)
                                return;

                            Y++;

                            if (Y >= ClientHeight)
                                break;
                        }

                        if (Y >= ClientHeight)
                            break;
                    }
                }

                return;
            }

            Write(new string(PasswordCharacter, Text.Length));
        }

        protected override void OnEnter()
        {
            Console.ForegroundColor = ForegroundColor;
            Console.BackgroundColor = BackgroundColor;

            base.OnEnter();

            SetCursorPosition();

            OriginalText = Text;

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
            if (OriginalText == Text)
                return;

            if (TextChanged != null)
                TextChanged(this, new TextChangedEventArgs(OriginalText, Text));

            OriginalText = Text;
        }

        protected override string OnTruncateText(string text)
        {
            return text;
        }

        protected override void ReadKey()
        {
            while (HasFocus)
            {
                ConsoleKeyInfo info = Console.ReadKey(true);

                if (OnKeyPressed(info))
                    return;

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
                                if (CursorLeft > 0)
                                {
                                    CursorLeft--;

                                    SetCursorPosition();
                                }
                                else
                                {
                                    if (CursorTop > 0)
                                    {
                                        CursorTop--;
                                        CursorLeft = ClientWidth;

                                        SetCursorPosition();
                                    }
                                }
                            }

                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            if (!string.IsNullOrEmpty(Text))
                            {
                                switch (TextBoxType)
                                {
                                    case TextBoxType.Multiline:
                                        {
                                            if (CursorTop < ClientHeight)
                                            {
                                                CursorTop++;
                                                CursorLeft = 0;

                                                SetCursorPosition();
                                            }

                                            break;
                                        }
                                    default:
                                        {
                                            if (CursorLeft < ClientWidth & CursorLeft < Text.Length)
                                            {
                                                CursorLeft++;

                                                SetCursorPosition();
                                            }

                                            break;
                                        }
                                }
                            }

                            break;
                        }
                    case ConsoleKey.End:
                        {
                            SetCursorPosition(Text.Length);

                            break;
                        }
                    case ConsoleKey.Home:
                        {
                            SetCursorPosition(0);

                            break;
                        }
                    case ConsoleKey.Backspace:
                        {
                            if (!string.IsNullOrEmpty(Text))
                            {
                                var index = CursorPositionToIndex();

                                if (index == 0)
                                    break;

                                // remove one character from the list of characters
                                if (index == Text.Length)
                                    Text = Text.Substring(0, index - 1);
                                else
                                    Text = Text.Substring(0, index - 1) + Text.Substring(index, Text.Length - index);

                                // move the cursor to the left by one character
                                CursorLeft--;
                                SetCursorPosition();

                                // redraw and repaint
                                DrawText();
                                Paint();
                            }

                            break;
                        }
                    case ConsoleKey.Delete:
                        {
                            if (!string.IsNullOrEmpty(Text))
                            {
                                var index = CursorPositionToIndex();

                                if (index == Text.Length)
                                    break;

                                // remove one character from the list of characters
                                Text = Text.Substring(0, index) + Text.Substring(index + 1, Text.Length - index - 1);

                                // redraw and repaint
                                DrawText();
                                Paint();
                            }

                            break;
                        }
                    case ConsoleKey.Insert:
                        {
                            Insert = !Insert;

                            SetCursorSize();

                            break;
                        }
                    default:
                        {
                            if (char.IsControl(info.KeyChar))
                                break;

                            if (Text == null || ((CursorLeft < ClientWidth & Text.Length < MaxLength) || (CursorLeft < ClientWidth & MaxLength == 0)))
                            {
                                if (Text == null || CursorLeft == Text.Length)
                                    Text += info.KeyChar;
                                else
                                {
                                    var index = CursorPositionToIndex();

                                    if (Insert)
                                        Text = Text.Insert(index, info.KeyChar.ToString());
                                    else
                                        Text = Text.Substring(0, index) + info.KeyChar.ToString() + Text.Substring(index + 1, Text.Length - index - 1);
                                }

                                CursorLeft++;

                                if (CursorLeft >= ClientWidth)
                                    if (TextBoxType == TextBoxType.Multiline)
                                    {
                                        CursorLeft = 0;

                                        if (CursorTop < ClientHeight)
                                            CursorTop++;
                                    }
                                    else
                                        CursorLeft = ClientWidth;

                                SetCursorPosition();

                                // redraw and repaint
                                DrawText();
                                Paint();
                            }

                            break;
                        }
                }
            }
        }

        private int CursorPositionToIndex()
        {
            return (CursorTop * ClientWidth) + CursorLeft;
        }

        private void SetCursorPosition(int index)
        {
            var top = index / ClientWidth;
            var left = index - (top * ClientWidth);

            CursorTop = top;
            CursorLeft = left;

            SetCursorPosition();
        }

        private void SetCursorPosition()
        {
            var offset = 0;

            if (TextAlign == TextAlign.Center)
                offset = (Width / 2) - (Text.Length / 2);

            if (TextAlign == TextAlign.Right)
                offset = Width - Text.Length - 1;

            Console.CursorLeft = ClientLeft + CursorLeft + offset;
            Console.CursorTop = ClientTop + CursorTop;
        }

        private void SetCursorSize()
        {
            if (Insert)
                Console.CursorSize = 10;
            else
                Console.CursorSize = 100;
        }
    }
}