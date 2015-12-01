using System.Collections.Generic;

namespace ConsoleUI
{
    public static class Utils
    {
        public static void SetWindowPosition(int x, int y, int width, int height)
        {
            NativeMethods.SetWindowPosition(x, y, width, height);
        }

        public static void SetWindowPosition(int x, int y)
        {
            NativeMethods.SetWindowPosition(x, y);
        }

        public static IEnumerable<string> SplitIntoChunks(this string s, int length)
        {
            var result = new List<string>();

            int start = 0;

            while (start < s.Length)
            {
                if ((start + length) >= s.Length)
                {
                    result.Add(s.Substring(start));

                    break;
                }
                else
                    result.Add(s.Substring(start, length));

                start += length;
            }

            return result;
        }
    }
}