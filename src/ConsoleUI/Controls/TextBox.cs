using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUI
{
    public class TextBox : InputControl
    {
        private int CursorLeft;
        private int CursorTop;
        private List<string> displayLines;
        private bool insert = true;
        private int LineOffset;
        private string[] lines;
        private int maxLength;
        private string OriginalText;
        private char passwordCharacter = '*';
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

        public bool Insert
        {
            get
            {
                return insert;
            }
            set
            {
                SetProperty(ref insert, value);
            }
        }

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

        public char PasswordCharacter
        {
            get
            {
                return passwordCharacter;
            }
            set
            {
                SetProperty(ref passwordCharacter, value);
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

        /// <summary>
        /// Gets or sets the display line where the cursor is.
        /// </summary>
        private string CurrentLine
        {
            get
            {
                if (DisplayLines.Count == 0)
                    return string.Empty;

                return DisplayLines[CursorTop + LineOffset];
            }
            set
            {
                if (DisplayLines.Count == 0)
                    DisplayLines.Add(value);
                else
                    DisplayLines[CursorTop + LineOffset] = value;

                RebuildLines();
            }
        }

        /// <summary>
        /// Gets the length of the current display line minus any new line characters
        /// </summary>
        private int CurrentLineLength
        {
            get
            {
                return CurrentLine.Replace(Environment.NewLine, string.Empty).Length;
            }
        }

        private bool IsCursorOnLastLine
        {
            get
            {
                return (CursorTop + LineOffset) == DisplayLines.Count - 1;
            }
        }

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

                for (int i = LineOffset; i < Math.Min(LineOffset + ClientHeight, DisplayLines.Count); i++)
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
                            MoveCursorLeft();

                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            MoveCursorRight();

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
                            MoveToHome();

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
            if (CurrentLineLength == 0)
                return;

            // remove one character from the list of characters
            if (CursorLeft == CurrentLineLength)
                CurrentLine = CurrentLine.Substring(0, CursorLeft - 1);
            else
                CurrentLine = CurrentLine.Substring(0, CursorLeft - 1) + CurrentLine.Substring(CursorLeft, CurrentLine.Length - CursorLeft);

            // move the cursor to the left by one character
            MoveCursorLeft();

            // redraw and repaint
            DrawText();
            Paint();
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

                if (CursorLeft > CurrentLineLength)
                    CursorLeft = CurrentLineLength;

                SetCursorPosition();
            }
            else
            {
                if (LineOffset > 0)
                {
                    LineOffset--;

                    if (CursorLeft > CurrentLineLength)
                        CursorLeft = CurrentLineLength;

                    DrawText();
                    Paint();
                }
            }
        }

        private void Delete()
        {
            if (CurrentLineLength == 0)
                return;

            if (CursorLeft != CurrentLineLength)
            {
                // remove one character from the list of characters
                CurrentLine = CurrentLine.Substring(0, CursorLeft) + CurrentLine.Substring(CursorLeft + 1, CurrentLine.Length - CursorLeft - 1);

                // redraw and repaint
                DrawText();
                Paint();
            }
        }

        private void IncrementCursorTop()
        {
            if (TextBoxType != TextBoxType.Multiline)
                return;

            if (CursorTop < ClientHeight - 1 & CursorTop < DisplayLines.Count - 1)
            {
                CursorTop++;

                if (CursorLeft > CurrentLineLength)
                    CursorLeft = CurrentLineLength;

                SetCursorPosition();
            }
            else
            {
                if (LineOffset + ClientHeight < DisplayLines.Count - 1)
                {
                    LineOffset++;

                    if (CursorLeft > CurrentLineLength)
                        CursorLeft = CurrentLineLength;

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
            if (CursorTop + LineOffset > 0)
            {
                DecrementCursorTop();

                CursorLeft = CurrentLineLength;
            }

            SetCursorPosition();
        }

        private void MoveCursorRight()
        {
            if (CursorLeft < ClientWidth - 1 & CursorLeft < CurrentLineLength)
                CursorLeft++;
            else
            if (TextBoxType == TextBoxType.Multiline)
            {
                if (CursorLeft == CurrentLineLength - 1 & !IsCursorOnLastLine)
                {
                    IncrementCursorTop();

                    CursorLeft = 0;
                }
            }

            SetCursorPosition();
        }

        private void MoveToEnd()
        {
            CursorLeft = CurrentLineLength;

            SetCursorPosition();
        }

        private void MoveToHome()
        {
            CursorLeft = 0;

            SetCursorPosition();
        }

        private void ProcessKey(ConsoleKeyInfo info)
        {
            if ((CursorLeft < ClientWidth & Text.Length < MaxLength) || (CursorLeft < ClientWidth & MaxLength == 0))
            {
                var keyValue = info.Key == ConsoleKey.Enter ? Environment.NewLine : info.KeyChar.ToString();

                if (CursorLeft == CurrentLineLength)
                {
                    CurrentLine = CurrentLine + keyValue;
                }
                else
                {
                    if (Insert)
                        CurrentLine = CurrentLine.Insert(CursorLeft, keyValue);
                    else
                        CurrentLine = CurrentLine.Substring(0, CursorLeft) + keyValue + CurrentLine.Substring(CursorLeft + 1, CurrentLine.Length - CursorLeft - 1);
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