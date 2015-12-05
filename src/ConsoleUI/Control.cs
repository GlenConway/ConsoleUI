using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ConsoleUI
{
    public class Control : INotifyPropertyChanged
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
        private int height;
        private int left;
        private IControlContainer owner;

        private byte SingleBorderBottomLeft = 192;
        private byte SingleBorderBottomRight = 217;
        private byte SingleBorderHorizontal = 196;
        private byte SingleBorderTopLeft = 218;
        private byte SingleBorderTopRight = 191;
        private byte SingleBorderVertical = 179;

        private string text;
        private TextAlign textAlign;
        private int top;
        private bool visible;

        private int width;

        public Control()
        {
            Height = 1;
            Visible = true;

            PropertyChanged += (s, e) =>
            {
                HandlePropertyChanged(e);
            };
        }

        public event EventHandler AfterPaint;

        public event CancelEventHandler BeforePaint;

        /// <summary>
        /// Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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

                SetProperty(ref borderStyle, value);
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

        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                SetProperty(ref height, value);
            }
        }

        public int Left
        {
            get
            {
                return left;
            }
            set
            {
                SetProperty(ref left, value);
            }
        }

        public IControlContainer Owner
        {
            get
            {
                return owner;
            }
            set
            {
                SetProperty(ref owner, value);
            }
        }

        public int Right
        {
            get
            {
                return Left + Width - 1;
            }
        }

        public virtual string Text
        {
            get
            {
                return text;
            }
            set
            {
                SetProperty(ref text, value);
            }
        }

        public TextAlign TextAlign
        {
            get
            {
                return textAlign;
            }
            set
            {
                SetProperty(ref textAlign, value);
            }
        }

        public int Top
        {
            get
            {
                return top;
            }
            set
            {
                SetProperty(ref top, value);
            }
        }

        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                SetProperty(ref visible, value);
            }
        }

        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                SetProperty(ref width, value);
            }
        }

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

            if (Owner == null)
                return;

            DrawBackground();
            DrawBorder();
            DrawControl();
            DrawShadow();
        }

        public void Hide()
        {
            Visible = false;
        }

        public void Show()
        {
            Visible = true;
        }

        protected virtual void DrawBackground()
        {
            if (!Visible)
                return;

            if (Owner == null)
                return;

            for (int x = Left; x < Right + 1; x++)
            {
                for (int y = Top; y < Bottom + 1; y++)
                {
                    Owner.Buffer.Write(x, y, 32, ForegroundColor, BackgroundColor);
                }
            }
        }

        protected virtual void DrawBorder()
        {
            if (Owner == null)
                return;

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

        protected virtual void DrawShadow()
        {
            if (!Visible)
                return;

            if (!HasShadow)
                return;

            if (Owner == null)
                return;

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
            if (Owner == null)
                return;

            Write(Text);
        }

        protected virtual void HandlePropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Height" || e.PropertyName == "Width" || e.PropertyName == "Left" || e.PropertyName == "Top")
            {
                Draw();
                Paint();
            }

            if (e.PropertyName == "Visible")
            {
                if (Visible)
                {
                    Draw();
                    Paint();
                }
            }
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

        /// <summary>
        ///     Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">
        ///     Name of the property used to notify listeners.  This
        ///     value is optional and can be provided automatically when invoked from compilers
        ///     that support <see cref="CallerMemberNameAttribute" />.
        /// </param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler eventHandler = this.PropertyChanged;

            if (eventHandler == null)
                return;

            eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual string OnTruncateText(string text)
        {
            text = text.Remove(ClientWidth - 3, text.Length - ClientWidth + 3);
            text += "...";

            return text;
        }

        protected virtual void OnWrite(int x, int y, string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            if (Owner == null)
                return;

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

        /// <summary>
        ///     Checks if a property already matches a desired value.  Sets the property and
        ///     notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">
        ///     Name of the property used to notify listeners.  This
        ///     value is optional and can be provided automatically when invoked from compilers that
        ///     support CallerMemberName.
        /// </param>
        /// <returns>
        ///     True if the value was changed, false if the existing value matched the
        ///     desired value.
        /// </returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        protected void Write(string text)
        {
            if (Owner == null)
                return;

            if (ClientWidth < 1)
                return;

            if (text == null)
                text = string.Empty;

            if (text.Length > ClientWidth)
            {
                text = OnTruncateText(text);
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
        }

        private void DrawDoubleBorder()
        {
            if (Owner == null)
                return;

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
            if (Owner == null)
                return;

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