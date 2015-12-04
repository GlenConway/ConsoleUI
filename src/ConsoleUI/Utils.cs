using System;
using System.Collections.Generic;

namespace ConsoleUI
{
    public static class Utils
    {
        public static string[] AssembleChunks(this List<string> s, int length)
        {
            var result = new List<string>();
            var line = string.Empty;

            foreach (var item in s)
            {
                if (item.Contains(Environment.NewLine))
                {
                    line += item;
                    result.Add(line);
                    line = string.Empty;
                }
                else
                    line += item;
            }

            return result.ToArray();
        }

        public static void SetWindowPosition(int x, int y, int width, int height)
        {
            NativeMethods.SetWindowPosition(x, y, width, height);
        }

        public static void SetWindowPosition(int x, int y)
        {
            NativeMethods.SetWindowPosition(x, y);
        }

        public static IList<string> SplitIntoChunks(this string[] s, int length)
        {
            var result = new List<string>();

            for (int i = 0; i < s.Length; i++)
            {
                result.AddRange(s[i].SplitIntoChunks(length));
            }

            return result;
        }

        public static IList<string> SplitIntoChunks(this string s, int length)
        {
            var result = new List<string>();

            var lines = s.Split(Environment.NewLine.ToCharArray());

            if (lines.Length > 1)
            {
                foreach (var line in lines)
                {
                    result.AddRange(line.SplitIntoChunks(length));
                }
            }
            else
            {
                int start = 0;

                while (start < s.Length)
                {
                    if (start + length >= s.Length)
                    {
                        result.Add(s.Substring(start) + Environment.NewLine);

                        break;
                    }
                    else
                    {
                        var sub = s.Substring(start, length);

                        result.Add(sub);

                        start += length;
                    }
                }
            }

            return result;
        }
    }
}