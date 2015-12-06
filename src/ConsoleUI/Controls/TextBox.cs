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

        public int CurrentDisplayLineIndex
        {
            get
            {
                return CursorTop + LineOffset;
            }
        }

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
                Lines = value.SplitIntoLines();

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
        private string CurrentDisplayLine
        {
            get
            {
                if (DisplayLines.Count == 0)
                    return string.Empty;

                return DisplayLines[CurrentDisplayLineIndex];
            }
            set
            {
                if (DisplayLines.Count == 0)
                    DisplayLines.Add(value);
                else
                    DisplayLines[CurrentDisplayLineIndex] = value;

                RebuildLines();
            }
        }

        /// <summary>
        /// Gets the length of the current display line minus any new line characters
        /// </summary>
        private int CurrentDisplayLineLength
        {
            get
            {
                return CurrentDisplayLine.Replace(Environment.NewLine, string.Empty).Length;
            }
        }

        private List<string> DisplayLines
        {
            get
            {
                return displayLines;
            }
        }

        private bool IsCursorOnLastLine
        {
            get
            {
                return (CurrentDisplayLineIndex) == DisplayLines.Count - 1;
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
            if (CurrentDisplayLineLength == 0)
                return;

            // remove one character from the list of characters
            if (CursorLeft == CurrentDisplayLineLength)
                CurrentDisplayLine = CurrentDisplayLine.LeftPart(CursorLeft - 1);
            else
            {
                if (CursorLeft == 0)
                {
                    if (DecrementCursorTop())
                    {
                        MoveToEnd();

                        var nextLine = DisplayLines[CurrentDisplayLineIndex + 1];
                        var newLine = CurrentDisplayLine.LeftPart(CursorLeft - 1);

                        //DisplayLines.RemoveAt(CurrentDisplayLineIndex + 1);
                        DisplayLines[CurrentDisplayLineIndex + 1] = string.Empty;

                        CurrentDisplayLine = newLine + nextLine;
                    }
                }
                else
                {
                    CurrentDisplayLine = CurrentDisplayLine.Substring(0, CursorLeft - 1) + CurrentDisplayLine.Substring(CursorLeft, CurrentDisplayLine.Length - CursorLeft);
                }
            }

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

        private bool DecrementCursorTop()
        {
            if (TextBoxType != TextBoxType.Multiline)
                return false;

            if (CursorTop > 0)
            {
                CursorTop--;

                if (CursorLeft > CurrentDisplayLineLength)
                    CursorLeft = CurrentDisplayLineLength;

                SetCursorPosition();

                return true;
            }

            if (LineOffset > 0)
            {
                LineOffset--;

                if (CursorLeft > CurrentDisplayLineLength)
                    CursorLeft = CurrentDisplayLineLength;

                DrawText();
                Paint();

                return true;
            }

            return false;
        }

        private void Delete()
        {
            if (CurrentDisplayLineLength == 0 & TextBoxType != TextBoxType.Multiline)
                return;

            if (CursorLeft != CurrentDisplayLineLength)
            {
                var line = CurrentDisplayLine;

                // remove one character from the list of characters
                CurrentDisplayLine = CurrentDisplayLine.Remove(CursorLeft, 1);
            }
            else
            {
                if (CurrentDisplayLineIndex < DisplayLines.Count)
                    DisplayLines.RemoveAt(CurrentDisplayLineIndex);
            }

            // redraw and repaint
            DrawText();
            Paint();
        }

        private void IncrementCursorTop()
        {
            if (TextBoxType != TextBoxType.Multiline)
                return;

            if (CursorTop < ClientHeight - 1 & CursorTop < DisplayLines.Count - 1)
            {
                CursorTop++;

                if (CursorLeft > CurrentDisplayLineLength)
                    CursorLeft = CurrentDisplayLineLength;

                SetCursorPosition();
            }
            else
            {
                if (LineOffset + ClientHeight < DisplayLines.Count - 1)
                {
                    LineOffset++;

                    if (CursorLeft > CurrentDisplayLineLength)
                        CursorLeft = CurrentDisplayLineLength;

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
            if (CurrentDisplayLineIndex > 0)
            {
                DecrementCursorTop();

                CursorLeft = CurrentDisplayLineLength;
            }

            SetCursorPosition();
        }

        private void MoveCursorRight()
        {
            if (CursorLeft < ClientWidth - 1 & CursorLeft < CurrentDisplayLineLength)
                CursorLeft++;
            else
            if (TextBoxType == TextBoxType.Multiline)
            {
                if (CursorLeft == CurrentDisplayLineLength - 1 & !IsCursorOnLastLine)
                {
                    IncrementCursorTop();

                    CursorLeft = 0;
                }
            }

            SetCursorPosition();
        }

        private void MoveToEnd()
        {
            CursorLeft = CurrentDisplayLineLength;

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
                var keyValue = info.KeyChar.ToString();

                // at the end of the line so add the character
                if (CursorLeft == CurrentDisplayLineLength)
                {
                    if (info.Key == ConsoleKey.Enter)
                        DisplayLines.Insert(CurrentDisplayLineIndex + 1, string.Empty);
                    else
                        CurrentDisplayLine = CurrentDisplayLine + keyValue;
                }
                else
                {
                    var line = CurrentDisplayLine;

                    if (Insert)
                    {
                        if (info.Key == ConsoleKey.Enter)
                        {
                            CurrentDisplayLine = line.LeftPart(CursorLeft);
                            DisplayLines.Insert(CurrentDisplayLineIndex + 1, line.RightPart(CursorLeft));
                        }
                        else
                            CurrentDisplayLine = CurrentDisplayLine.Insert(CursorLeft, keyValue);
                    }
                    else
                    {
                        if (info.Key == ConsoleKey.Enter)
                        {
                            CurrentDisplayLine = line.LeftPart(CursorLeft);
                            DisplayLines.Insert(CurrentDisplayLineIndex + 1, line.RightPart(CursorLeft + 1));
                        }
                        else
                            CurrentDisplayLine = line.LeftPart(CursorLeft) + keyValue + line.RightPart(CursorLeft + 1);
                    }
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