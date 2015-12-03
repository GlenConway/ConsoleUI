using System;
using System.Linq;

namespace ConsoleUI
{
    public class TextBox : InputControl
    {
        public char PasswordCharacter = '*';
        private int CursorLeft;
        private int CursorTop;
        private bool Insert = true;
        private int lineOffset;
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

        private string[] DisplayLines
        {
            get
            {
                if (Lines == null)
                    return new string[0];

                return string.Join(Environment.NewLine, Lines).SplitIntoChunks(ClientWidth).ToArray();
            }
        }

        protected override void DrawText()
        {
            if (Lines == null || Lines.Length == 0)
            {
                Write(string.Empty);

                return;
            }

            if (TextBoxType != TextBoxType.Password)
            {
                var y = Y;

                var lines = DisplayLines;

                for (int i = lineOffset; i < lineOffset + ClientHeight; i++)
                {
                    var line = lines[i];

                    if (string.IsNullOrEmpty(line))
                        Write(string.Empty);
                    else
                    {
                        Write(line);

                        if (TextBoxType != TextBoxType.Multiline)
                            return;

                        Y++;

                        if (Y >= ClientHeight)
                            break;
                    }
                }

                Y = y;

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

                var line = string.Empty;

                var lines = DisplayLines;

                if (lines.Length > CursorTop)
                    line = lines[CursorTop];

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

                            if (TextBoxType == TextBoxType.Multiline)
                            {
                                ProcessKey(info);

                                break;
                            }

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
                            if (!string.IsNullOrEmpty(line))
                            {
                                if (CursorLeft > 0)
                                    CursorLeft--;
                                else
                                if (CursorTop > 0)
                                {
                                    DecrementCursorTop();

                                    CursorLeft = ClientWidth - 1;
                                }

                                SetCursorPosition();
                            }

                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            if (!string.IsNullOrEmpty(line))
                            {
                                if (CursorLeft < ClientWidth - 1 & CursorLeft < line.Length)
                                    CursorLeft++;
                                else
                                if (TextBoxType == TextBoxType.Multiline)
                                {
                                    IncrementCursorTop();

                                    CursorLeft = 0;
                                }

                                SetCursorPosition();
                            }

                            break;
                        }
                    case ConsoleKey.UpArrow:
                        {
                            DecrementCursorTop();

                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            IncrementCursorTop();

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

                            ProcessKey(info);

                            break;
                        }
                }
            }
        }

        private int CursorPositionToIndex()
        {
            return (CursorTop * ClientWidth) + CursorLeft;
        }

        private void DecrementCursorTop()
        {
            if (TextBoxType != TextBoxType.Multiline)
                return;

            if (CursorTop > 0)
            {
                CursorTop--;

                SetCursorPosition();
            }
            else
            {
                if (lineOffset > 0)
                {
                    lineOffset--;

                    DrawText();
                    Paint();
                }
            }
        }

        private void IncrementCursorTop()
        {
            if (TextBoxType != TextBoxType.Multiline)
                return;

            if (CursorTop < ClientHeight - 1)
            {
                CursorTop++;

                SetCursorPosition();
            }
            else
            {
                if (lineOffset + ClientHeight < DisplayLines.Count())
                {
                    lineOffset++;

                    DrawText();
                    Paint();
                }
            }
        }

        private void ProcessKey(ConsoleKeyInfo info)
        {
            if (Text == null || ((CursorLeft < ClientWidth & Text.Length < MaxLength) || (CursorLeft < ClientWidth & MaxLength == 0)))
            {
                var index = CursorPositionToIndex();

                if (Text == null || index == Text.Length)
                    Text += info.KeyChar;
                else
                {
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

            var text = string.Empty;

            var lines = DisplayLines;

            if (lines.Length > CursorTop)
                text = lines[CursorTop];

            if (TextAlign == TextAlign.Center)
                offset = (ClientWidth / 2) - (text.Length / 2);

            if (TextAlign == TextAlign.Right)
                offset = ClientWidth - text.Length - 1;

            if (CursorTop >= ClientHeight)
                CursorTop = ClientHeight;

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