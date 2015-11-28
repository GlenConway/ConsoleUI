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

            ShadowLabelScreen(screens);
            ShadowSingleBorderLabelScreen(screens);
            ShadowDoubleBorderLabelScreen(screens);
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

        private static void ShadowDoubleBorderLabelScreen(ScreenCollection screens)
        {
            var screen = new Screen("Double Border Labels");

            var control1 = new Label("This is a left aligned label (shadow, double border).");

            control1.Left = 0;
            control1.Top = 0;
            control1.Width = screen.Width - 2;
            control1.BorderStyle = BorderStyle.Double;

            var control2 = new Label("This is a centered label (shadow, double border).");

            control2.Left = 0;
            control2.Top = control1.Top + control1.Height + 2;
            control2.Width = screen.Width - 1;
            control2.TextAlign = TextAlign.Center;
            control2.BorderStyle = BorderStyle.Double;

            var control3 = new Label("This is a right aligned label (shadow, full width, double border).");

            control3.Left = 0;
            control3.Top = control2.Top + control2.Height + 2;
            control3.Width = screen.Width;
            control3.TextAlign = TextAlign.Right;
            control3.BorderStyle = BorderStyle.Double;

            control1.HasShadow = true;
            control2.HasShadow = true;
            control3.HasShadow = true;

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

        private static void ShadowLabelScreen(ScreenCollection screens)
        {
            var screen = new Screen("Basic Labels");

            var control1 = new Label("This is a left aligned label (shadow, no border).");

            control1.Left = 0;
            control1.Top = 0;
            control1.Width = screen.Width - 2;

            var control2 = new Label("This is a centered label (shadow, no border).");

            control2.Left = 0;
            control2.Top = control1.Top + control1.Height + 2;
            control2.Width = screen.Width - 1;
            control2.TextAlign = TextAlign.Center;

            var control3 = new Label("This is a right aligned label (shadow, full width, no border).");

            control3.Left = 0;
            control3.Top = control2.Top + control2.Height + 2;
            control3.Width = screen.Width;
            control3.TextAlign = TextAlign.Right;

            control1.HasShadow = true;
            control2.HasShadow = true;
            control3.HasShadow = true;

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

        private static void ShadowSingleBorderLabelScreen(ScreenCollection screens)
        {
            var screen = new Screen("Single Border Labels");

            var control1 = new Label("This is a left aligned label (shadow, single border).");

            control1.Left = 0;
            control1.Top = 0;
            control1.Width = screen.Width - 2;
            control1.BorderStyle = BorderStyle.Single;

            var control2 = new Label("This is a centered label (shadow, single border).");

            control2.Left = 0;
            control2.Top = control1.Top + control1.Height + 2;
            control2.Width = screen.Width - 1;
            control2.TextAlign = TextAlign.Center;
            control2.BorderStyle = BorderStyle.Single;

            var control3 = new Label("This is a right aligned label (shadow, full width, single border).");

            control3.Left = 0;
            control3.Top = control2.Top + control2.Height + 2;
            control3.Width = screen.Width;
            control3.TextAlign = TextAlign.Right;
            control3.BorderStyle = BorderStyle.Single;

            control1.HasShadow = true;
            control2.HasShadow = true;
            control3.HasShadow = true;

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