using System;
using System.Collections.Generic;

namespace ConsoleUI
{
    public class ListBox : Control
    {
        public int CurrentIndex = 0;

        public ConsoleColor SelectedBackgroundColor = ConsoleColor.White;
        public ConsoleColor SelectedForegroundColor = ConsoleColor.Blue;

        public int SelectedIndex = -1;
        public ConsoleColor SelectedNoFocusBackgroundColor = ConsoleColor.Blue;
        public ConsoleColor SelectedNoFocusForegroundColor = ConsoleColor.Green;
        private int endIndex = 0;
        private List<string> items;
        private byte ScrollBarDark = 178;
        private byte ScrollBarLight = 176;
        private byte ScrollBarMedium = 177;
        private int startIndex = 0;

        public ListBox()
        {
            TabStop = true;
        }

        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        public event EventHandler Selected;

        public IList<string> Items
        {
            get
            {
                if (items == null)
                    items = new List<string>();

                return items;
            }
        }

        protected double ScrollBarPercent
        {
            get
            {
                return (double)CurrentIndex / (double)Items.Count;
            }
        }

        public virtual bool OnKeyPressed(ConsoleKeyInfo info)
        {
            if (KeyPressed != null)
            {
                var args = new KeyPressedEventArgs(info);

                KeyPressed(this, args);

                return args.Handled;
            }

            return false;
        }

        protected override void DrawControl()
        {
            var maxRows = ClientHeight;

            if (maxRows > Items.Count)
                maxRows = Items.Count;

            startIndex = 0;
            endIndex = startIndex + maxRows;

            if (CurrentIndex >= maxRows)
            {
                startIndex = CurrentIndex - (maxRows - 1);
                endIndex = CurrentIndex + 1;
            }

            var y = 0;

            for (int i = startIndex; i < endIndex; i++)
            {
                var label = new Label(Items[i]);
                label.Owner = Owner;
                label.Width = ClientWidth;
                label.Left = ClientLeft;
                label.Top = ClientTop + y;

                label.BackgroundColor = i == CurrentIndex ? (HasFocus ? SelectedBackgroundColor : SelectedNoFocusBackgroundColor) : BackgroundColor;
                label.ForegroundColor = i == CurrentIndex ? (HasFocus ? SelectedForegroundColor : SelectedNoFocusForegroundColor) : ForegroundColor;

                label.Draw();

                y++;
            }

            for (int i = y; i < ClientHeight; i++)
            {
                var label = new Label();
                label.Owner = Owner;
                label.Width = ClientWidth;
                label.Left = ClientLeft;
                label.Top = ClientTop + i;

                label.BackgroundColor = BackgroundColor;
                label.ForegroundColor = ForegroundColor;

                label.Draw();
            }

            DrawVerticalScrollBar();
        }

        protected override void OnEnter()
        {
            Console.CursorVisible = false;

            base.OnEnter();

            if (BorderStyle != BorderStyle.None)
            {
                Console.CursorTop = Console.CursorTop + 1;
                Console.CursorLeft = Console.CursorLeft + 1;
            }

            DrawControl();
            OnRepaint();

            ReadKey();
        }

        protected override void OnLeave()
        {
            base.OnLeave();

            DrawControl();
            OnRepaint();
        }

        protected virtual void OnSelected()
        {
            if (Selected != null)
                Selected(this, new EventArgs());
        }

        private void DrawVerticalScrollBar()
        {
            if (Items.Count < ClientHeight)
                return;

            var position = (int)(ClientHeight * ScrollBarPercent);

            for (int i = 0; i < ClientHeight; i++)
            {
                Owner.Buffer.Write((short)ClientRight, (short)ClientTop + (short)i, i == position ? ScrollBarDark : ScrollBarLight, i == position ? ConsoleColor.White : ForegroundColor, BackgroundColor);
            }
        }

        private void ReadKey()
        {
            while (true)
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
                            Blur();

                            OnTabPressed(info.Modifiers.HasFlag(ConsoleModifiers.Shift));

                            return;
                        }
                    case ConsoleKey.Enter:
                        {
                            SelectedIndex = CurrentIndex;

                            OnSelected();

                            Blur();

                            OnTabPressed(false);

                            return;
                        }
                    case ConsoleKey.UpArrow:
                        {
                            if (CurrentIndex > 0)
                                CurrentIndex--;

                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            if (CurrentIndex < Items.Count - 1)
                                CurrentIndex++;

                            break;
                        }
                    case ConsoleKey.PageUp:
                        {
                            if (CurrentIndex > ClientHeight)
                                CurrentIndex -= ClientHeight;
                            else
                                CurrentIndex = 0;

                            break;
                        }
                    case ConsoleKey.PageDown:
                        {
                            if (CurrentIndex < Items.Count - ClientHeight)
                                CurrentIndex += ClientHeight;
                            else
                                CurrentIndex = Items.Count - 1;

                            break;
                        }
                    case ConsoleKey.Home:
                        {
                            CurrentIndex = 0;

                            break;
                        }
                    case ConsoleKey.End:
                        {
                            CurrentIndex = Items.Count - 1;

                            break;
                        }
                }

                DrawControl();
                OnRepaint();
            }
        }
    }
}