using System;
using System.Collections.Generic;
using System.Linq;

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

        public static IList<string> SplitIntoChunks(this string s, int length)
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
                {
                    var sub = s.Substring(start, length);

                    result.Add(sub);
                }

                start += length;
            }

            return result;
            //var charCount = 0;
            //var lines = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //return lines.GroupBy(w => (charCount += (((charCount % length) + w.Length + 1 >= length)
            //                ? length - (charCount % length) : 0) + w.Length + 1) / length)
            //            .Select(g => string.Join(" ", g.ToArray()))
            //            .ToArray();
        }
    }
}