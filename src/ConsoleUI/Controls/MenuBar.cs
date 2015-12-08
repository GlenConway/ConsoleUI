using System;

namespace ConsoleUI
{
    public class MenuBar : Control
    {
        private ControlCollection<Menu> menus;

        public MenuBar(IControlContainer owner)
        {
            Owner = owner;
            Height = 1;
            TabStop = true;

            FocusBackgroundColor = ConsoleColor.Black;
            FocusForegroundColor = ConsoleColor.Gray;
            ForegroundColor = ConsoleColor.Black;
            BackgroundColor = ConsoleColor.Gray;
        }

        public ControlCollection<Menu> Menus
        {
            get
            {
                if (menus == null)
                {
                    menus = new ControlCollection<Menu>(Owner);

                    menus.EscPressed += (s, e) =>
                    {
                        OnTabPressed(false);
                    };
                }

                return menus;
            }
        }

        public override void Draw()
        {
            if (!ShouldDraw)
                return;

            DrawBackground();
            DrawControl();
        }

        protected override void DrawControl()
        {
            var left = 0;
            var width = 0;

            foreach (var menu in Menus)
            {
                width = menu.Text.Length;
                menu.Left = left;
                menu.Width = width;
                menu.Top = Top;
                menu.ResumeLayout();
                menu.Draw();

                left += width + 2;
            }

            Owner.Paint();
        }

        protected override void OnEnter()
        {
            Console.CursorVisible = false;

            base.OnEnter();

            DrawControl();

            Menus.SetFocus();
        }
    }
}