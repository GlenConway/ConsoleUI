using System;
using System.Runtime.InteropServices;

namespace ConsoleUI
{
    public static class NativeMethods
    {
        private const int STD_OUTPUT_HANDLE = -11;

        internal enum Color : short
        {
            Black = 0,
            ForegroundBlue = 0x1,
            ForegroundGreen = 0x2,
            ForegroundRed = 0x4,
            ForegroundYellow = 0x6,
            ForegroundIntensity = 0x8,
            BackgroundBlue = 0x10,
            BackgroundGreen = 0x20,
            BackgroundRed = 0x40,
            BackgroundYellow = 0x60,
            BackgroundIntensity = 0x80,

            ForegroundMask = 0xf,
            BackgroundMask = 0xf0,
            ColorMask = 0xff
        }

        internal static IntPtr OutputHandle
        {
            get
            {
                return GetStdHandle(STD_OUTPUT_HANDLE);
            }
        }

        internal static Color ConsoleColorToColorAttribute(ConsoleColor color, bool isBackground)
        {
            if ((((int)color) & ~0xf) != 0)
                throw new ArgumentException("Invalid console color");

            Color c = (Color)color;

            // Make these background colors instead of foreground
            if (isBackground)
                c = (Color)((int)c << 4);

            return c;
        }

        internal static void Paint(Buffer buffer)
        {
            bool b = WriteConsoleOutput(OutputHandle, buffer.Value,
              buffer.Size,
              buffer.Coord,
              ref buffer.Rectangle);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool WriteConsoleOutput(IntPtr hConsoleOutput, CharInfo[] lpBuffer, Coord dwBufferSize, Coord dwBufferCoord, ref SmallRect lpWriteRegion);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)]
            public CharUnion Char;

            [FieldOffset(2)]
            public short Attributes;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion
        {
            [FieldOffset(0)]
            public char UnicodeChar;

            [FieldOffset(0)]
            public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }
    }
}