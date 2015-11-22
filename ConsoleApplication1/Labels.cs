using ConsoleUI;
using System;

namespace ConsoleApplication1
{
    internal static class Labels
    {
        internal static void SetupLabelScreens(ScreenCollection screens)
        {
            LabelScreen(screens);
            SingleBorderLabelScreen(screens);
            DoubleBorderLabelScreen(screens);
        }

        private static void DoubleBorderLabelScreen(ScreenCollection screens)
        {
            var screen = new Screen("Double Border Labels");

            var control1 = new Label("This is a left aligned label (full width, double border).");

            control1.Left = 0;
            control1.Top = 0;
            control1.Width = screen.Width;
            control1.BorderStyle = BorderStyle.Double;

            var control2 = new Label("This is a centered label (full width, double border).");

            control2.Left = 0;
            control2.Top = control1.Top + control1.Height;
            control2.Width = screen.Width;
            control2.TextAlign = TextAlign.Center;
            control2.BorderStyle = BorderStyle.Double;

            var control3 = new Label("This is a right aligned label (full width, double border).");

            control3.Left = 0;
            control3.Top = control2.Top + control2.Height;
            control3.Width = screen.Width;
            control3.TextAlign = TextAlign.Right;
            control3.BorderStyle = BorderStyle.Double;

            screen.Controls.Add(control1);
            screen.Controls.Add(control2);
            screen.Controls.Add(control3);

            screen.Footer.Text = screen.Name + ". Press any key.";

            screens.Add(screen);

            screen.AfterPaint += (s, e) =>
            {
                Console.ReadKey(true);
            };
        }

        private static void LabelScreen(ScreenCollection screens)
        {
            var screen = new Screen("Basic Labels");

            var control1 = new Label("This is a left aligned label (full width, no border).");

            control1.Left = 0;
            control1.Top = 0;
            control1.Width = screen.Width;

            var control2 = new Label("This is a centered label (full width, no border).");

            control2.Left = 0;
            control2.Top = control1.Top + control1.Height;
            control2.Width = screen.Width;
            control2.TextAlign = TextAlign.Center;

            var control3 = new Label("This is a right aligned label (full width, no border).");

            control3.Left = 0;
            control3.Top = control2.Top + control2.Height;
            control3.Width = screen.Width;
            control3.TextAlign = TextAlign.Right;

            screen.Controls.Add(control1);
            screen.Controls.Add(control2);
            screen.Controls.Add(control3);

            screen.Footer.Text = screen.Name + ". Press any key.";

            screens.Add(screen);

            screen.AfterPaint += (s, e) =>
            {
                Console.ReadKey(true);
            };
        }

        private static void SingleBorderLabelScreen(ScreenCollection screens)
        {
            var screen = new Screen("Single Border Labels");

            var control1 = new Label("This is a left aligned label (full width, single border).");

            control1.Left = 0;
            control1.Top = 0;
            control1.Width = screen.Width;
            control1.BorderStyle = BorderStyle.Single;

            var control2 = new Label("This is a centered label (full width, single border).");

            control2.Left = 0;
            control2.Top = control1.Top + control1.Height;
            control2.Width = screen.Width;
            control2.TextAlign = TextAlign.Center;
            control2.BorderStyle = BorderStyle.Single;

            var control3 = new Label("This is a right aligned label (full width, single border).");

            control3.Left = 0;
            control3.Top = control2.Top + control2.Height;
            control3.Width = screen.Width;
            control3.TextAlign = TextAlign.Right;
            control3.BorderStyle = BorderStyle.Single;

            screen.Controls.Add(control1);
            screen.Controls.Add(control2);
            screen.Controls.Add(control3);

            screen.Footer.Text = screen.Name + ". Press any key.";

            screens.Add(screen);

            screen.AfterPaint += (s, e) =>
            {
                Console.ReadKey(true);
            };
        }
    }
}