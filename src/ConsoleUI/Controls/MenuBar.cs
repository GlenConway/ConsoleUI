namespace ConsoleUI
{
    public class MenuBar : InputControl
    {
        private ControlCollection menus;

        public MenuBar(IControlContainer owner)
        {
            Owner = owner;
            Height = 1;
            TabStop = true;
        }

        public ControlCollection Menus
        {
            get
            {
                if (menus == null)
                    menus = new ControlCollection(Owner);

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

        
    }
}