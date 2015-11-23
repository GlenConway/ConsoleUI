using System;

namespace ConsoleUI
{
    public class Screen : IControlContainer
    {
        private readonly Buffer buffer;
        private readonly int height;
        private readonly int width;
        private readonly ControlCollection controls;
        private Label footer;

        public Screen(string name) : this(Console.WindowWidth, Console.WindowHeight, name)
        {
        }

        public Screen(int width, int height, string name)
        {
            this.width = width;
            this.height = height;

            Name = name;

            buffer = new Buffer(0, 0, height, width);
            controls = new ControlCollection(this);

            controls.Repaint += (s, e) =>
            {
                Draw();
                Paint();
            };
        }

        public event EventHandler AfterPaint;

        public event EventHandler BeforePaint;

        public Buffer Buffer
        {
            get
            {
                return buffer;
            }
        }

        public ControlCollection Controls
        {
            get
            {
                return controls;
            }
        }

        public Label Footer
        {
            get
            {
                if (footer == null)
                {
                    footer = new Label();

                    footer.Top = Console.WindowHeight - 1;
                    footer.Left = Console.WindowLeft;
                    footer.Width = Console.WindowWidth;

                    footer.Owner = this;
                }

                return footer;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
        }

        public string Name { get; set; }
        public bool Visible { get; set; }

        public int Width
        {
            get
            {
                return width;
            }
        }

        public void Show()
        {
            Console.CursorVisible = false;

            Draw();
            Paint();

            Controls.SetFocus();
        }

        protected void Draw()
        {
            foreach (var control in Controls)
                control.Draw();

            Footer.Draw();
        }

        protected virtual void OnAfterPaint()
        {
            if (AfterPaint != null)
                AfterPaint(this, new EventArgs());
        }

        protected virtual void OnBeforePaint()
        {
            if (BeforePaint != null)
                BeforePaint(this, new EventArgs());
        }

        protected void Paint()
        {
            OnBeforePaint();

            NativeMethods.Paint(Buffer);

            OnAfterPaint();
        }
    }
}