using ConsoleUI;
using System;

namespace ConsoleApplication1
{
    internal static class ProgressBars
    {
        internal static void SetupProgressBars(ScreenCollection screens)
        {
            TestProgressBars(screens);
        }

        private static void TestProgressBars(ScreenCollection screens)
        {
            var screen = new Screen("Progress Bars - No Border");

            var control1 = new ProgressBar();

            control1.Left = 0;
            control1.Top = 0;
            control1.Width = 20;
            control1.BlockColor = ConsoleColor.Yellow;

            screen.Controls.Add(control1);

            var control2 = new ProgressBar();

            control2.Left = 20;
            control2.Top = 10;
            control2.Width = 5;
            control2.BorderStyle = BorderStyle.Single;

            screen.Controls.Add(control2);

            var control3 = new ProgressBar();

            control3.Left = 35;
            control3.Top = 20;
            control3.Width = 30;
            control3.BorderStyle = BorderStyle.Double;
            control3.BlockColor = ConsoleColor.Red;
            control3.HasShadow = true;

            screen.Controls.Add(control3);

            var control4 = new ProgressBar();

            control4.Left = 0;
            control4.Top = 15;
            control4.Width = 30;
            control4.BorderStyle = BorderStyle.Double;
            control4.BlockColor = ConsoleColor.Green;
            control4.HasShadow = true;
            control4.ProgressBarStyle = ProgressBarStyle.Marquee;

            screen.Controls.Add(control4);

            screen.Footer.Text = screen.Name + ". Press any key.";

            screens.Add(screen);

            screen.Shown += (s, e) =>
            {
                while (control1.Value < control1.Maximum)
                {
                    Console.ReadKey(true);

                    control1.Increment(10);
                    control2.Increment(10);
                    control3.Increment(10);

                    screen.Footer.Text = string.Format("Value: {0}", control1.Value);
                }
            };
        }
    }
}