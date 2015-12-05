using System;
using System.Collections.Generic;

namespace ConsoleUI
{
    public static class Utils
    {
        public static string[] AssembleChunks(this List<string> s, int length)
        {
            var result = new List<string>();

            if (s == null)
                return result.ToArray();

            var line = string.Empty;

            foreach (var item in s)
            {
                if (item.EndsWith(Environment.NewLine))
                {
                    line += item;
                    result.Add(line);
                    line = string.Empty;
                }
                else
                    line += item;
            }

            if (!string.IsNullOrEmpty(line))
                result.Add(line);

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

        /// <summary>
        /// Splits an array of strings into a new list based on a maximum length, adding Environment.NewLine to the end of all but the last line group.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IList<string> SplitIntoChunks(this string[] s, int length)
        {
            var result = new List<string>();

            for (int i = 0; i < s.Length; i++)
            {
                result.AddRange(s[i].SplitIntoChunks(length));

                if (i < s.Length - 1)
                    result[result.Count - 1] += Environment.NewLine;
            }
            
            return result;
        }

        /// <summary>
        /// Returns a list of strings split by length.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
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
                        result.Add(s.Substring(start));

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