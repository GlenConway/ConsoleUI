using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ConsoleUI
{
    public static class NativeMethods
    {
        private const int STD_OUTPUT_HANDLE = -11;

        private const int SWP_NOACTIVATE = 0x10;

        private const int SWP_NOZORDER = 0x4;

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

        public static IntPtr Handle
        {
            get
            {
                //Initialize();
                return GetConsoleWindow();
            }
        }

        internal static IntPtr OutputHandle
        {
            get
            {
                return GetStdHandle(STD_OUTPUT_HANDLE);
            }
        }

        internal static ConsoleColor ColorAttributeToConsoleColor(Color c)
        {
            // Turn background colors into foreground colors.
            if ((c & Color.BackgroundMask) != 0)
            {
                c = (Color)(((int)c) >> 4);
            }
            return (ConsoleColor)c;
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

        internal static RECT GetWindowRectangle()
        {
            RECT rct;

            if (!GetWindowRect(Handle, out rct))
                return rct;

            return rct;
        }

        internal static void Paint(Buffer buffer)
        {
            bool b = WriteConsoleOutput(OutputHandle, buffer.Value,
              buffer.Size,
              buffer.Coord,
              ref buffer.Rectangle);

            if (!b)
            {
                var e = new Win32Exception();

                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        internal static void Paint(int left, int top, int height, int width, Buffer buffer)
        {
            var rectangle = new NativeMethods.SmallRect() { Top = (short)top, Left = (short)left, Bottom = (short)(top + height), Right = (short)(left + width) };
            var coord = new NativeMethods.Coord((short)left, (short)top);

            bool b = WriteConsoleOutput(OutputHandle, buffer.Value,
              buffer.Size,
              coord,
              ref rectangle);

            if (!b)
            {
                var e = new Win32Exception();

                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        internal static void SetWindowPosition(int x, int y, int width, int height)
        {
            SetWindowPos(Handle, IntPtr.Zero, x, y, width, height, SWP_NOZORDER | SWP_NOACTIVATE);
        }

        internal static void SetWindowPosition(int x, int y)
        {
            var rect = GetWindowRectangle();

            SetWindowPos(Handle, IntPtr.Zero, x, y, rect.Right - rect.Left, rect.Bottom - rect.Top, SWP_NOZORDER | SWP_NOACTIVATE);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool WriteConsoleOutput(IntPtr hConsoleOutput, CharInfo[] lpBuffer, Coord dwBufferSize, Coord dwBufferCoord, ref SmallRect lpWriteRegion);

        [DllImport("kernel32")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags);

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
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

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