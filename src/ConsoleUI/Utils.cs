using System;
using System.Collections.Generic;

namespace ConsoleUI
{
    public static class Utils
    {
        public static string[] AssembleChunks(this List<string> s, int length)
        {
            var result = new List<string>();

            if (s == null || s.Count == 0)
                return result.ToArray();

            string line = null;

            foreach (var item in s)
            {
                if (item.Length < length)
                {
                    line += item;
                    result.Add(line);
                    line = null;
                }
                else
                    line += item;
            }

            if (line != null)
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
                if (s[i] == string.Empty)
                    result.Add(string.Empty);
                else
                    result.AddRange(s[i].SplitIntoChunks(length));
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

        public static string[] SplitIntoLines(this string s)
        {
            var result = new List<string>();
            var index = 0;

            while (index >= 0)
            {
                index = s.IndexOf(Environment.NewLine);

                if (index >= 0)
                {
                    result.Add(s.Substring(0, index));
                    s = s.Substring(index + Environment.NewLine.Length, s.Length - index - Environment.NewLine.Length);
                }
            }

            if (!string.IsNullOrEmpty(s))
                result.Add(s);

            return result.ToArray();
        }

        public static string LeftPart(this string s, int index)
        {
            return s.Substring(0, index);
        }
        public static string RightPart(this string s, int index)
        {
            return s.Substring(index, s.Length - index);
        }
    }
}