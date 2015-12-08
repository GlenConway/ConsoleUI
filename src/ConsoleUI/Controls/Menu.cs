using System;
using System.Linq;

namespace ConsoleUI
{
    public class Menu : Control
    {
        private ControlCollection<MenuItem> menuItems;
        private Rectangle rectangle;

        private bool showMenuItems;
        private bool menuItemsHasFocus;

        public Menu(IControlContainer owner)
        {
            Owner = owner;
            Height = 1;
            TabStop = true;

            FocusBackgroundColor = ConsoleColor.Black;
            FocusForegroundColor = ConsoleColor.Gray;
            ForegroundColor = ConsoleColor.Black;
            BackgroundColor = ConsoleColor.Gray;

            rectangle = new Rectangle();
            rectangle.Owner = owner;
        }

        public ControlCollection<MenuItem> MenuItems
        {
            get
            {
                if (menuItems == null)
                    menuItems = new ControlCollection<MenuItem>(Owner);

                return menuItems;
            }
        }

        protected override void DrawBackground()
        {
            base.DrawBackground();

            if (!showMenuItems)
                return;

            if (MenuItems.Count == 0)
                return;

            rectangle.SuspendLayout();

            rectangle.Left = Left;
            rectangle.Top = Top + 1;
            rectangle.Height = MenuItems.Count + 2;
            rectangle.Width = Math.Max(20, Math.Max(Width, MenuItems.Max(p => p.Width)));
            rectangle.HasShadow = true;
            rectangle.BorderStyle = BorderStyle.Single;

            PreserveArea(rectangle.Left, rectangle.Top, rectangle.Height + 1, rectangle.Width + 1);

            rectangle.ResumeLayout();
            rectangle.Draw();

            DrawMenuItems();
        }

        private void DrawMenuItems()
        {
            var y = rectangle.ClientTop;
            var x = rectangle.ClientLeft;
            var width = rectangle.ClientWidth;

            foreach (var item in MenuItems)
            {
                item.SuspendLayout();
                item.Top = y;
                item.Left = x;
                item.Width = width;
                item.ResumeLayout();
                item.Draw();

                if (item.IsSeparator)
                {
                    Owner.Buffer.Write((short)rectangle.Left, (short)y, 195, item.ForegroundColor, item.BackgroundColor);
                    Owner.Buffer.Write((short)rectangle.Right, (short)y, 180, item.ForegroundColor, item.BackgroundColor);

                    for (int i = 1; i < rectangle.Width - 1; i++)
                    {
                        Owner.Buffer.Write((short)rectangle.Left + i, (short)y, 196, item.ForegroundColor, item.BackgroundColor);
                    }
                }

                y++;
            }

            rectangle.Paint();
        }

        protected override void OnEnter()
        {
            Console.CursorVisible = false;

            base.OnEnter();

            showMenuItems = true;

            Draw();
            Paint();

            ReadKey();
        }

        protected override void OnLeave()
        {
            base.OnLeave();

            showMenuItems = false;
            menuItemsHasFocus = false;

            MenuItems.RemoveFocus();

            RestoreArea();

            DrawControl();
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
                    case ConsoleKey.DownArrow:
                        {
                            if (!menuItemsHasFocus)
                            {
                                menuItemsHasFocus = true;

                                MenuItems.SetFocus();
                            }
                            else
                                MenuItems.TabToNextControl(false);

                            DrawMenuItems();

                            break;
                        }
                    case ConsoleKey.UpArrow:
                        {
                            if (menuItemsHasFocus)
                            {
                                MenuItems.TabToNextControl(true);

                                DrawMenuItems();
                            }

                            break;
                        }
                }
            }
        }

        public MenuItem AddMenuItem(string text)
        {
            var item = new MenuItem(Owner);
            item.Text = text;

            MenuItems.Add(item);

            return item;
        }

        public MenuItem AddSeparator()
        {
            var item = new MenuItem(Owner);
            item.IsSeparator = true;

            MenuItems.Add(item);

            return item;
        }
    }
}