using System;

namespace ConsoleUI
{
    public class Buffer
    {
        public NativeMethods.SmallRect Rectangle;
        private NativeMethods.CharInfo[] buffer;

        public Buffer(int left, int top, int height, int width)
        {
            Left = left;
            Top = top;
            Height = height;
            Width = width;

            buffer = new NativeMethods.CharInfo[width * height];

            Rectangle = new NativeMethods.SmallRect() { Top = (short)Top, Left = (short)Left, Bottom = (short)(Top + Height), Right = (short)(Left + Width) };
        }

        public NativeMethods.Coord Coord
        {
            get
            {
                return new NativeMethods.Coord((short)Left, (short)Top);
            }
        }

        public int Height { get; private set; }

        public int Left { get; private set; }

        public NativeMethods.Coord Size
        {
            get
            {
                return new NativeMethods.Coord((short)Width, (short)Height);
            }
        }

        public int Top { get; private set; }

        public NativeMethods.CharInfo[] Value
        {
            get
            {
                return buffer;
            }
        }

        public int Width { get; private set; }

        //public NativeMethods.CharInfo this[int x, int y]
        //{
        //    get
        //    {
        //        return buffer[GetIndex(x, y)];
        //    }
        //    set
        //    {
        //        buffer[GetIndex(x, y)] = value;
        //    }
        //}

        public ConsoleColor GetBackgroundColor(int x, int y)
        {
            var index = (Width * y) + x;

            if (index < buffer.Length)
            {
                var attrs = buffer[index].Attributes;

                return NativeMethods.ColorAttributeToConsoleColor((NativeMethods.Color)attrs & NativeMethods.Color.BackgroundMask);
            }

            return ConsoleColor.Black;
        }

        public ConsoleColor GetForegroundColor(int x, int y)
        {
            var index = (Width * y) + x;

            if (index < buffer.Length)
            {
                var attrs = buffer[index].Attributes;

                return NativeMethods.ColorAttributeToConsoleColor((NativeMethods.Color)attrs & NativeMethods.Color.ForegroundMask);
            }

            return ConsoleColor.Black;
        }

        public void SetColor(int x, int y, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            var index = (Width * y) + x;

            if (index < buffer.Length)
            {
                SetColor(index, foregroundColor, backgroundColor);
            }
        }

        public void Write(int x, int y, byte ascii, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            var index = (Width * y) + x;

            if (index < buffer.Length)
            {
                SetColor(x, y, foregroundColor, backgroundColor);

                buffer[index].Char.AsciiChar = ascii;
            }
        }

        public void Write(int x, int y, byte ascii, ConsoleColor foregroundColor)
        {
            var backgroundColor = GetBackgroundColor(x, y);

            Write(x, y, ascii, foregroundColor, backgroundColor);
        }

        public void Write(int x, int y, string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            if (string.IsNullOrEmpty(text))
                return;

            var fc = NativeMethods.ConsoleColorToColorAttribute(foregroundColor, false);
            var bc = NativeMethods.ConsoleColorToColorAttribute(backgroundColor, true);

            for (int i = 0; i < text.Length; i++)
            {
                var index = (Width * y) + x + i;

                if (index < buffer.Length)
                {
                    var attrs = buffer[index].Attributes;

                    attrs &= ~((short)NativeMethods.Color.ForegroundMask);
                    // C#'s bitwise-or sign-extends to 32 bits.
                    attrs = (short)(((uint)(ushort)attrs) | ((uint)(ushort)fc));

                    attrs &= ~((short)NativeMethods.Color.BackgroundMask);
                    // C#'s bitwise-or sign-extends to 32 bits.
                    attrs = (short)(((uint)(ushort)attrs) | ((uint)(ushort)bc));

                    buffer[index].Attributes = attrs;

                    buffer[index].Char.AsciiChar = (byte)text[i];
                }
            }
        }

        internal void SetColor(int index, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            if (index < buffer.Length)
            {
                var fc = NativeMethods.ConsoleColorToColorAttribute(foregroundColor, false);
                var bc = NativeMethods.ConsoleColorToColorAttribute(backgroundColor, true);

                var attrs = buffer[index].Attributes;

                attrs &= ~((short)NativeMethods.Color.ForegroundMask);
                // C#'s bitwise-or sign-extends to 32 bits.
                attrs = (short)(((uint)(ushort)attrs) | ((uint)(ushort)fc));

                attrs &= ~((short)NativeMethods.Color.BackgroundMask);
                // C#'s bitwise-or sign-extends to 32 bits.
                attrs = (short)(((uint)(ushort)attrs) | ((uint)(ushort)bc));

                buffer[index].Attributes = attrs;
            }
        }

        //private int GetIndex(int x, int y)
        //{
        //    var index = (Width * y) + x;

        //    if (index < buffer.Length)
        //        return index;

        //    throw new InvalidOperationException("Value is outside of the buffer.");
        //}
    }
}