using System;

namespace ConsoleUI
{
    public class Control
    {
        public ConsoleColor BackgroundColor = ConsoleColor.Blue;
        public ConsoleColor ForegroundColor = ConsoleColor.Gray;
        protected int X;
        protected int Y;
        private BorderStyle borderStyle;
        private byte DoubleBorderBottomLeft = 200;
        private byte DoubleBorderBottomRight = 188;
        private byte DoubleBorderHorizontal = 205;
        private byte DoubleBorderTopLeft = 201;
        private byte DoubleBorderTopRight = 187;
        private byte DoubleBorderVertical = 186;
        private IControlContainer owner;

        private byte SingleBorderBottomLeft = 192;
        private byte SingleBorderBottomRight = 217;
        private byte SingleBorderHorizontal = 196;
        private byte SingleBorderTopLeft = 218;
        private byte SingleBorderTopRight = 191;
        private byte SingleBorderVertical = 179;

        private bool visible;

        public Control()
        {
            Height = 1;
            Visible = true;
        }

        public event EventHandler Enter;

        public event EventHandler EscPressed;

        public event EventHandler Leave;

        public event EventHandler Repaint;

        public event EventHandler<TabEventArgs> TabPressed;

        public BorderStyle BorderStyle
        {
            get
            {
                return borderStyle;
            }
            set
            {
                if (value != BorderStyle.None && Height < 3)
                    Height = 3;

                borderStyle = value;
            }
        }

        public int Bottom
        {
            get
            {
                return Top + Height - 1;
            }
        }

        public int ClientRight
        {
            get
            {
                return ClientLeft + ClientWidth - 1;
            }
        }

        public bool HasFocus { get; set; }

        public int Height { get; set; }

        public int Left { get; set; }

        public IControlContainer Owner
        {
            get
            {
                return owner;
            }
            set
            {
                owner = value;
            }
        }

        public int Right
        {
            get
            {
                return Left + Width - 1;
            }
        }

        public int TabOrder { get; set; }

        public bool TabStop { get; set; }

        public string Text { get; set; }

        public TextAlign TextAlign { get; set; }

        public int Top { get; set; }

        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;

                OnRepaint();
            }
        }

        public int Width { get; set; }

        protected int ClientHeight
        {
            get
            {
                return Height - (Offset * 2);
            }
        }

        protected int ClientLeft
        {
            get
            {
                return Left + Offset;
            }
        }

        protected int ClientTop
        {
            get
            {
                return Top + Offset;
            }
        }

        protected int ClientWidth
        {
            get
            {
                return Width - (Offset * 2);
            }
        }

        protected int Offset
        {
            get
            {
                return BorderStyle == BorderStyle.None ? 0 : 1;
            }
        }

        public void Draw()
        {
            if (!Visible)
                return;

            DrawBorder();
            DrawControl();
        }

        public void Focus()
        {
            if (!TabStop)
                return;

            HasFocus = true;
            Console.CursorVisible = true;
            Console.CursorLeft = Left;
            Console.CursorTop = Top;

            OnEnter();
        }

        protected void Blur()
        {
            if (!TabStop)
                return;

            HasFocus = false;
            Console.CursorVisible = false;

            OnLeave();
        }

        protected virtual void DrawBorder()
        {
            if (BorderStyle == BorderStyle.None)
                return;

            if (BorderStyle == BorderStyle.Single)
                DrawSingleBorder();

            if (BorderStyle == BorderStyle.Double)
                DrawDoubleBorder();
        }

        protected virtual void DrawControl()
        {
            DrawText();
        }

        protected virtual void DrawText()
        {
            Write(Text);
        }

        protected virtual void OnEnter()
        {
            if (Enter != null)
                Enter(this, new EventArgs());
        }

        protected virtual void OnEscPressed()
        {
            if (EscPressed != null)
                EscPressed(this, new EventArgs());
        }

        protected virtual void OnLeave()
        {
            if (Leave != null)
                Leave(this, new EventArgs());
        }

        protected virtual void OnRepaint()
        {
            if (Repaint != null)
                Repaint(this, new EventArgs());
        }

        protected virtual void OnTabPressed(bool shift)
        {
            if (TabPressed != null)
                TabPressed(this, new TabEventArgs(shift));
        }

        protected void Write(string text)
        {
            if (text == null)
                text = string.Empty;

            if (text.Length > ClientWidth)
            {
                text = text.Remove(ClientWidth - 3, text.Length - ClientWidth + 3);
                text += "...";
            }

            switch (TextAlign)
            {
                case TextAlign.Left:
                    {
                        text = text.PadRight(ClientWidth);

                        break;
                    }
                case TextAlign.Center:
                    {
                        var padding = (ClientWidth - text.Length) / 2;

                        text = string.Format("{0}{1}{0}", new string(' ', padding), text);

                        text = text.PadLeft(ClientWidth);

                        break;
                    }
                case TextAlign.Right:
                    {
                        text = text.PadLeft(ClientWidth);

                        break;
                    }
            }

            owner.Buffer.Write((short)(X + ClientLeft), (short)(Y + ClientTop), text, ForegroundColor, BackgroundColor);

            //X += text.Length;

            //if (X > ClientWidth)
            //{
            //    X = 0;
            //    Y++;
            //}
        }

        private void DrawDoubleBorder()
        {
            owner.Buffer.Write((short)Left, (short)Top, DoubleBorderTopLeft, ForegroundColor, BackgroundColor);
            owner.Buffer.Write((short)Right, (short)Top, DoubleBorderTopRight, ForegroundColor, BackgroundColor);
            owner.Buffer.Write((short)Left, (short)Bottom, DoubleBorderBottomLeft, ForegroundColor, BackgroundColor);
            owner.Buffer.Write((short)Right, (short)Bottom, DoubleBorderBottomRight, ForegroundColor, BackgroundColor);

            for (int i = Left + 1; i < Right; i++)
            {
                owner.Buffer.Write((short)i, (short)Top, DoubleBorderHorizontal, ForegroundColor, BackgroundColor);
                owner.Buffer.Write((short)i, (short)Bottom, DoubleBorderHorizontal, ForegroundColor, BackgroundColor);
            }

            for (int i = Top + 1; i < Bottom; i++)
            {
                owner.Buffer.Write((short)Left, (short)i, DoubleBorderVertical, ForegroundColor, BackgroundColor);
                owner.Buffer.Write((short)Right, (short)i, DoubleBorderVertical, ForegroundColor, BackgroundColor);
            }
        }

        private void DrawSingleBorder()
        {
            owner.Buffer.Write((short)Left, (short)Top, SingleBorderTopLeft, ForegroundColor, BackgroundColor);
            owner.Buffer.Write((short)Right, (short)Top, SingleBorderTopRight, ForegroundColor, BackgroundColor);
            owner.Buffer.Write((short)Left, (short)Bottom, SingleBorderBottomLeft, ForegroundColor, BackgroundColor);
            owner.Buffer.Write((short)Right, (short)Bottom, SingleBorderBottomRight, ForegroundColor, BackgroundColor);

            for (int i = Left + 1; i < Right; i++)
            {
                owner.Buffer.Write((short)i, (short)Top, SingleBorderHorizontal, ForegroundColor, BackgroundColor);
                owner.Buffer.Write((short)i, (short)Bottom, SingleBorderHorizontal, ForegroundColor, BackgroundColor);
            }

            for (int i = Top + 1; i < Bottom; i++)
            {
                owner.Buffer.Write((short)Left, (short)i, SingleBorderVertical, ForegroundColor, BackgroundColor);
                owner.Buffer.Write((short)Right, (short)i, SingleBorderVertical, ForegroundColor, BackgroundColor);
            }
        }
    }
}