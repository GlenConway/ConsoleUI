using System;
using System.ComponentModel;

namespace ConsoleUI
{
    public class Screen : IControlContainer
    {
        public ConsoleColor BackgroundColor = ConsoleColor.Gray;
        public ConsoleColor ForegroundColor = ConsoleColor.DarkGray;
        private readonly Buffer buffer;
        private readonly ControlCollection controls;
        private readonly int height;
        private readonly int width;
        private Label footer;
        private bool visible;

        public Screen(string name) : this(Console.WindowWidth, Console.WindowHeight, name)
        {
        }

        public Screen(int width, int height, string name)
        {
            this.width = width;
            this.height = height;

            Name = name;

            Console.WindowHeight = height;
            Console.WindowWidth = width;
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;

            buffer = new Buffer(0, 0, height, width);

            Clear();

            controls = new ControlCollection(this);
        }

        public event EventHandler AfterPaint;

        public event CancelEventHandler BeforePaint;

        public event EventHandler Shown;

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

                    footer.BackgroundColor = BackgroundColor;
                    footer.ForegroundColor = ForegroundColor;

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

        public bool Visible
        {
            get
            {
                return visible;
            }
        }

        public int Width
        {
            get
            {
                return width;
            }
        }

        public void Clear()
        {
            var x = 0;

            while (x < width)
            {
                var y = 0;

                while (y <= height)
                {
                    Buffer.Write((short)x, (short)y, 32, ForegroundColor, BackgroundColor);

                    y++;
                }

                x++;
            }
        }

        public void Exit()
        {
            Hide();

            Controls.Exit();
        }

        public void Hide()
        {
            visible = false;
        }

        public void Paint()
        {
            var args = new CancelEventArgs();

            OnBeforePaint(args);

            if (args.Cancel)
                return;

            NativeMethods.Paint(Buffer);

            OnAfterPaint();
        }

        public virtual void Show()
        {
            Console.Title = Name;

            Console.CursorVisible = false;

            visible = true;

            Draw();
            Paint();

            OnShown();

            Controls.SetFocus();
        }

        internal void Show(InputControl focus)
        {
            Console.Title = Name;

            Console.CursorVisible = false;

            Draw();
            Paint();

            OnShown();

            Controls.SetFocus(focus);
        }

        protected void Draw()
        {
            Clear();

            foreach (var control in Controls)
                control.Draw();

            Footer.Draw();
        }

        protected virtual void OnAfterPaint()
        {
            if (AfterPaint != null)
                AfterPaint(this, new EventArgs());
        }

        protected virtual void OnBeforePaint(CancelEventArgs args)
        {
            if (BeforePaint != null)
                BeforePaint(this, args);
        }

        protected virtual void OnShown()
        {
            visible = true;

            if (Shown != null)
                Shown(this, new EventArgs());
        }
    }
}