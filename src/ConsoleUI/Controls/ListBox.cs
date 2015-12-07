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

        //private byte ScrollBarMedium = 177;
        private int startIndex = 0;

        public ListBox() : base()
        {
            TabStop = true;
        }

        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        public event EventHandler Selected;

        public bool HasVerticalScrollBar
        {
            get
            {
                return Items.Count >= ClientHeight;
            }
        }

        public IList<string> Items
        {
            get
            {
                if (items == null)
                    items = new List<string>();

                return items;
            }
        }

        public string SelectedItem
        {
            get
            {
                if (SelectedIndex < 0)
                    return string.Empty;

                if (Items.Count == 0)
                    return string.Empty;

                if (SelectedIndex > Items.Count)
                    return string.Empty;

                return Items[SelectedIndex];
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
            if (!ShouldDraw)
                return;

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

            var labels = new List<Label>();

            var width = HasVerticalScrollBar ? ClientWidth - 1 : ClientWidth;

            for (int i = startIndex; i < endIndex; i++)
            {
                var label = new Label(Items[i]);
                label.Owner = Owner;
                label.Width = width;
                label.Left = ClientLeft;
                label.Top = ClientTop + y;

                label.BackgroundColor = i == CurrentIndex ? (HasFocus ? SelectedBackgroundColor : SelectedNoFocusBackgroundColor) : BackgroundColor;
                label.ForegroundColor = i == CurrentIndex ? (HasFocus ? SelectedForegroundColor : SelectedNoFocusForegroundColor) : ForegroundColor;

                labels.Add(label);

                y++;
            }

            for (int i = 0; i < labels.Count; i++)
            {
                var label = labels[i];
                label.ResumeLayout();
                label.Draw();
            }
           
            DrawVerticalScrollBar();

            Paint();
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

            ReadKey();
        }

        protected override void OnLeave()
        {
            base.OnLeave();

            DrawControl();
        }

        protected virtual void OnSelected()
        {
            if (Selected != null)
                Selected(this, new EventArgs());
        }

        protected override void ReadKey()
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
            }
        }

        private void DrawVerticalScrollBar()
        {
            if (!ShouldDraw)
                return;

            if (!HasVerticalScrollBar)
                return;

            var position = (int)(ClientHeight * ScrollBarPercent);

            for (int i = 0; i < ClientHeight; i++)
            {
                Owner.Buffer.Write((short)ClientRight, (short)ClientTop + (short)i, i == position ? ScrollBarDark : ScrollBarLight, i == position ? ConsoleColor.White : ForegroundColor, BackgroundColor);
            }
        }
    }
}