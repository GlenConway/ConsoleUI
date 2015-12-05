using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUI
{
    public class TextBox : InputControl
    {
        public char PasswordCharacter = '*';
        private int CursorLeft;
        private int CursorTop;
        private List<string> displayLines;
        private bool Insert = true;
        private int lineOffset;
        private string[] lines;
        private int maxLength;
        private string OriginalText;

        private TextBoxType textBoxType;

        public TextBox()
        {
            TabStop = true;
            Width = 5;
            Height = 1;
            TreatEnterKeyAsTab = true;
        }

        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        public event EventHandler<TextChangedEventArgs> TextChanged;

        public string[] Lines
        {
            get
            {
                return lines;
            }
            set
            {
                lines = value;

                displayLines = lines.SplitIntoChunks(ClientWidth).ToList();
            }
        }

        public int MaxLength
        {
            get
            {
                return maxLength;
            }
            set
            {
                SetProperty(ref maxLength, value);
            }
        }

        public override string Text
        {
            get
            {
                if (Lines == null)
                    return string.Empty;

                return string.Join(Environment.NewLine, Lines);
            }

            set
            {
                Lines = value.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                CheckMaxLength();
            }
        }

        public TextBoxType TextBoxType
        {
            get
            {
                return textBoxType;
            }
            set
            {
                SetProperty(ref textBoxType, value);
            }
        }

        public bool TreatEnterKeyAsTab { get; set; }

        private List<string> DisplayLines
        {
            get
            {
                return displayLines;
            }
        }

        protected override void DrawText()
        {
            if (DisplayLines == null || DisplayLines.Count == 0)
            {
                Write(string.Empty);

                return;
            }

            if (TextBoxType != TextBoxType.Password)
            {
                var y = Y;

                for (int i = lineOffset; i < Math.Min(lineOffset + ClientHeight, DisplayLines.Count); i++)
                {
                    var line = DisplayLines[i];

                    if (string.IsNullOrEmpty(line))
                        Write(string.Empty);
                    else
                        Write(line.Replace(Environment.NewLine, string.Empty));

                    if (TextBoxType != TextBoxType.Multiline)
                        return;

                    Y++;

                    if (Y >= ClientHeight)
                        break;
                }

                Y = y;

                return;
            }

            Write(new string(PasswordCharacter, Text.Length));
        }

        protected override void HandlePropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "MaxLength")
                CheckMaxLength();

            if (e.PropertyName == "BorderStyle" || e.PropertyName == "Width")
                RebuildLines();

            base.HandlePropertyChanged(e);
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

                var line = DisplayLines[CursorTop + lineOffset];

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
                                MoveCursorLeft();

                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            if (!string.IsNullOrEmpty(line))
                                MoveCursorRight(line.Length);

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
                            MoveToEnd();

                            break;
                        }
                    case ConsoleKey.Home:
                        {
                            MoveToBeginning();

                            break;
                        }
                    case ConsoleKey.Backspace:
                        {
                            Backspace();

                            break;
                        }
                    case ConsoleKey.Delete:
                        {
                            Delete();

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

        private void Backspace()
        {
            var line = DisplayLines[CursorTop + lineOffset];

            if (!string.IsNullOrEmpty(line))
            {
                // remove one character from the list of characters
                if (CursorLeft == line.Length)
                    DisplayLines[CursorTop + lineOffset] = line.Substring(0, CursorLeft - 1);
                else
                    DisplayLines[CursorTop + lineOffset] = line.Substring(0, CursorLeft - 1) + line.Substring(CursorLeft, line.Length - CursorLeft);

                // move the cursor to the left by one character
                MoveCursorLeft();

                RebuildLines();

                // redraw and repaint
                DrawText();
                Paint();
            }
        }

        private void CheckMaxLength()
        {
            if (MaxLength == 0)
                return;

            var value = Text;

            if (!string.IsNullOrEmpty(value))
                if (value.Length > MaxLength)
                {
                    value = value.Substring(0, MaxLength);

                    Lines = value.Split(Environment.NewLine.ToCharArray());
                }
        }

        private void DecrementCursorTop()
        {
            if (TextBoxType != TextBoxType.Multiline)
                return;

            if (CursorTop > 0)
            {
                CursorTop--;

                var line = DisplayLines[CursorTop + lineOffset];

                if (CursorLeft > line.Length)
                    CursorLeft = line.Length;

                SetCursorPosition();
            }
            else
            {
                if (lineOffset > 0)
                {
                    lineOffset--;

                    var line = DisplayLines[CursorTop + lineOffset];

                    if (CursorLeft > line.Length)
                        CursorLeft = line.Length;

                    DrawText();
                    Paint();
                }
            }
        }

        private void Delete()
        {
            var line = DisplayLines[CursorTop + lineOffset];

            if (!string.IsNullOrEmpty(line))
            {
                if (CursorLeft != line.Length)
                {
                    // remove one character from the list of characters
                    DisplayLines[CursorTop + lineOffset] = line.Substring(0, CursorLeft) + line.Substring(CursorLeft + 1, line.Length - CursorLeft - 1);

                    RebuildLines();

                    // redraw and repaint
                    DrawText();
                    Paint();
                }
            }
        }

        private void IncrementCursorTop()
        {
            if (TextBoxType != TextBoxType.Multiline)
                return;

            if (CursorTop < ClientHeight - 1 & CursorTop < DisplayLines.Count - 1)
            {
                CursorTop++;

                var line = DisplayLines[CursorTop + lineOffset];

                if (CursorLeft > line.Length)
                    CursorLeft = line.Length;

                SetCursorPosition();
            }
            else
            {
                if (lineOffset + ClientHeight < DisplayLines.Count - 1)
                {
                    lineOffset++;

                    var line = DisplayLines[CursorTop + lineOffset];

                    if (CursorLeft > line.Length)
                        CursorLeft = line.Length;

                    DrawText();
                    Paint();
                }
            }
        }

        private void MoveCursorLeft()
        {
            if (CursorLeft > 0)
                CursorLeft--;
            else
            if (CursorTop + lineOffset > 0)
            {
                DecrementCursorTop();

                var line = DisplayLines[CursorTop + lineOffset];

                CursorLeft = line.Length;
            }

            SetCursorPosition();
        }

        private void MoveCursorRight(int lineLength)
        {
            if (CursorLeft < ClientWidth - 1 & CursorLeft < lineLength)
                CursorLeft++;
            else
            if (TextBoxType == TextBoxType.Multiline)
            {
                if (CursorLeft == lineLength)
                {
                    IncrementCursorTop();

                    CursorLeft = 0;
                }
            }

            SetCursorPosition();
        }

        private void MoveToBeginning()
        {
            lineOffset = 0;
            CursorLeft = 0;
            CursorTop = 0;

            SetCursorPosition();

            DrawText();
            Paint();
        }

        private void MoveToEnd()
        {
            var lineCount = DisplayLines.Count() - 1;
            var lastLine = DisplayLines.Last().Replace(Environment.NewLine, string.Empty);

            if (lineCount > ClientHeight)
            {
                lineOffset = lineCount - ClientHeight;
                CursorTop = ClientHeight;
            }
            else
            {
                CursorTop = lineCount;
            }

            CursorLeft = lastLine.Length;

            SetCursorPosition();

            DrawText();
            Paint();
        }

        private void ProcessKey(ConsoleKeyInfo info)
        {
            var line = DisplayLines[CursorTop + lineOffset];
            var lineLength = line.Replace(Environment.NewLine, string.Empty).Length;

            if (Text == null || ((CursorLeft < ClientWidth & Text.Length < MaxLength) || (CursorLeft < ClientWidth & MaxLength == 0)))
            {
                var keyValue = info.Key == ConsoleKey.Enter ? Environment.NewLine : info.KeyChar.ToString();

                if (Text == null || CursorLeft == lineLength)
                {
                    DisplayLines[CursorTop + lineOffset] += keyValue;
                }
                else
                {
                    if (Insert)
                        DisplayLines[CursorTop + lineOffset] = line.Insert(CursorLeft, keyValue);
                    else
                        DisplayLines[CursorTop + lineOffset] = line.Substring(0, CursorLeft) + keyValue + line.Substring(CursorLeft + 1, lineLength - CursorLeft - 1);
                }

                if (info.Key == ConsoleKey.Enter)
                {
                    IncrementCursorTop();
                    CursorLeft = 0;
                }
                else
                {
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
                }

                SetCursorPosition();
                RebuildLines();

                // redraw and repaint
                DrawText();
                Paint();
            }
        }

        private void RebuildLines()
        {
            Lines = DisplayLines.AssembleChunks(ClientWidth);
        }

        private void SetCursorPosition()
        {
            var offset = 0;

            var text = string.Empty;

            if (DisplayLines.Count > CursorTop)
                text = DisplayLines[CursorTop];

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