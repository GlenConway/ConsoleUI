using System;
using System.ComponentModel;

namespace ConsoleUI
{
    public class Control
    {
        public ConsoleColor BackgroundColor = ConsoleColor.Blue;
        public ConsoleColor ForegroundColor = ConsoleColor.Gray;

        public bool HasShadow = false;
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

        private string text;
        private bool visible;

        public Control()
        {
            Height = 1;
            Visible = true;
        }

        public event EventHandler AfterPaint;

        public event CancelEventHandler BeforePaint;

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

        public int ClientBottom
        {
            get
            {
                return ClientTop + ClientHeight - 1;
            }
        }

        public int ClientRight
        {
            get
            {
                return ClientLeft + ClientWidth - 1;
            }
        }

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

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (value != text)
                {
                    text = value;
                }
            }
        }

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
                if (value != visible)
                {
                    visible = value;

                    if (visible)
                    {
                        Draw();
                        Paint();
                    }
                }
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

            System.Diagnostics.Debug.WriteLine("{0}::{1}", this.GetType().Name, "Draw");

            DrawBorder();
            DrawControl();
            DrawShadow();
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
            System.Diagnostics.Debug.WriteLine("{0}::{1}", this.GetType().Name, "DrawControl");

            DrawText();
        }

        protected virtual void DrawShadow()
        {
            if (!HasShadow)
                return;

            System.Diagnostics.Debug.WriteLine("{0}::{1}", this.GetType().Name, "DrawShadow");

            var right = Right + 1;
            var bottom = Bottom + 1;

            if (bottom < Owner.Buffer.Rectangle.Bottom)
                for (int i = Left + 1; i <= Right + 1; i++)
                {
                    if (i < Owner.Buffer.Rectangle.Right)
                        Owner.Buffer.SetColor((short)i, (short)bottom, ConsoleColor.DarkGray, ConsoleColor.Black);
                }

            if (right < Owner.Buffer.Rectangle.Right)
                for (int i = Top + 1; i <= Bottom; i++)
                {
                    if (i < Owner.Buffer.Rectangle.Bottom)
                        Owner.Buffer.SetColor((short)right, (short)i, ConsoleColor.DarkGray, ConsoleColor.Black);
                }
        }

        protected virtual void DrawText()
        {
            System.Diagnostics.Debug.WriteLine("{0}::{1}", this.GetType().Name, "DrawText");

            Write(Text);
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

        protected virtual void OnWrite(int x, int y, string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Owner.Buffer.Write((short)x, (short)y, text, foregroundColor, backgroundColor);
        }

        protected virtual void Paint()
        {
            if (Owner == null)
                return;
            
            var args = new CancelEventArgs();

            OnBeforePaint(args);

            if (args.Cancel)
                return;

            var width = HasShadow ? Width + 1 : Width;
            var height = HasShadow ? Height + 1 : Height;

            NativeMethods.Paint(Left, Top, height, width, Owner.Buffer);

            OnAfterPaint();
        }

        protected void Write(string text)
        {
            if (ClientWidth < 1)
                return;

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

            OnWrite(X + ClientLeft, Y + ClientTop, text, ForegroundColor, BackgroundColor);

            //X += text.Length;

            //if (X > ClientWidth)
            //{
            //    X = 0;
            //    Y++;
            //}
        }

        private void DrawDoubleBorder()
        {
            System.Diagnostics.Debug.WriteLine("{0}::{1}", this.GetType().Name, "DrawDoubleBorder");

            Owner.Buffer.Write((short)Left, (short)Top, DoubleBorderTopLeft, ForegroundColor, BackgroundColor);
            Owner.Buffer.Write((short)Right, (short)Top, DoubleBorderTopRight, ForegroundColor, BackgroundColor);
            Owner.Buffer.Write((short)Left, (short)Bottom, DoubleBorderBottomLeft, ForegroundColor, BackgroundColor);
            Owner.Buffer.Write((short)Right, (short)Bottom, DoubleBorderBottomRight, ForegroundColor, BackgroundColor);

            for (int i = Left + 1; i < Right; i++)
            {
                Owner.Buffer.Write((short)i, (short)Top, DoubleBorderHorizontal, ForegroundColor, BackgroundColor);
                Owner.Buffer.Write((short)i, (short)Bottom, DoubleBorderHorizontal, ForegroundColor, BackgroundColor);
            }

            for (int i = Top + 1; i < Bottom; i++)
            {
                Owner.Buffer.Write((short)Left, (short)i, DoubleBorderVertical, ForegroundColor, BackgroundColor);
                Owner.Buffer.Write((short)Right, (short)i, DoubleBorderVertical, ForegroundColor, BackgroundColor);
            }
        }

        private void DrawSingleBorder()
        {
            System.Diagnostics.Debug.WriteLine("{0}::{1}", this.GetType().Name, "DrawSingleBorder");

            Owner.Buffer.Write((short)Left, (short)Top, SingleBorderTopLeft, ForegroundColor, BackgroundColor);
            Owner.Buffer.Write((short)Right, (short)Top, SingleBorderTopRight, ForegroundColor, BackgroundColor);
            Owner.Buffer.Write((short)Left, (short)Bottom, SingleBorderBottomLeft, ForegroundColor, BackgroundColor);
            Owner.Buffer.Write((short)Right, (short)Bottom, SingleBorderBottomRight, ForegroundColor, BackgroundColor);

            for (int i = Left + 1; i < Right; i++)
            {
                Owner.Buffer.Write((short)i, (short)Top, SingleBorderHorizontal, ForegroundColor, BackgroundColor);
                Owner.Buffer.Write((short)i, (short)Bottom, SingleBorderHorizontal, ForegroundColor, BackgroundColor);
            }

            for (int i = Top + 1; i < Bottom; i++)
            {
                Owner.Buffer.Write((short)Left, (short)i, SingleBorderVertical, ForegroundColor, BackgroundColor);
                Owner.Buffer.Write((short)Right, (short)i, SingleBorderVertical, ForegroundColor, BackgroundColor);
            }
        }
    }
}