using ConsoleUI;

namespace ConsoleApplication1
{
    internal static class TextBoxes
    {
        internal static void SetupTextBoxScreens(ScreenCollection screens)
        {
            BasicTextBoxScreen(screens);
            SingleBorderTextBoxScreen(screens);
            DoubleBorderTextBoxScreen(screens);
        }

        private static void BasicTextBoxScreen(ScreenCollection screens)
        {
            var screen = new Screen("Basic Text Boxes");

            var control1 = new TextBox();

            control1.Left = 10;
            control1.Top = 0;
            control1.MaxLength = 5;
            control1.Width = control1.MaxLength;
            control1.TreatEnterKeyAsTab = false;

            var control2 = new TextBox();

            control2.Left = 0;
            control2.Top = control1.Top + control1.Height;
            control2.Width = screen.Width;
            control2.TextAlign = TextAlign.Center;

            var control3 = new TextBox();

            control3.Left = 0;
            control3.Top = control2.Top + control2.Height;
            control3.MaxLength = 20;
            control3.Width = control3.MaxLength; ;

            screen.Controls.Add(control1);
            screen.Controls.Add(control2);
            screen.Controls.Add(control3);

            screen.Footer.Text = screen.Name + ". Press escape.";

            screens.Add(screen);
        }

        private static void DoubleBorderTextBoxScreen(ScreenCollection screens)
        {
            var screen = new Screen("Double Border Text Boxes");

            var control1 = new TextBox();

            control1.Left = 10;
            control1.Top = 0;
            control1.MaxLength = 6;
            control1.Width = control1.MaxLength + 2;
            control1.BorderStyle = BorderStyle.Double;

            var control2 = new TextBox();

            control2.Left = 0;
            control2.Top = control1.Top + control1.Height;
            control2.Width = screen.Width;
            control2.BorderStyle = BorderStyle.Double;

            var control3 = new TextBox();

            control3.Left = 0;
            control3.Top = control2.Top + control2.Height;
            control3.MaxLength = 20;
            control3.Width = control3.MaxLength;
            control3.BorderStyle = BorderStyle.Double;

            screen.Controls.Add(control1);
            screen.Controls.Add(control2);
            screen.Controls.Add(control3);

            screen.Footer.Text = screen.Name + ". Press escape.";

            screens.Add(screen);
        }

        private static void SingleBorderTextBoxScreen(ScreenCollection screens)
        {
            var screen = new Screen("Single Border Text Boxes");

            var control1 = new TextBox();

            control1.Left = 10;
            control1.Top = 0;
            control1.MaxLength = 5;
            control1.Width = control1.MaxLength;
            control1.BorderStyle = BorderStyle.Single;

            var control2 = new TextBox();

            control2.Left = 0;
            control2.Top = control1.Top + control1.Height;
            control2.Width = screen.Width;
            control2.BorderStyle = BorderStyle.Single;

            var control3 = new TextBox();

            control3.Left = 0;
            control3.Top = control2.Top + control2.Height;
            control3.MaxLength = 20;
            control3.Width = control3.MaxLength;
            control3.BorderStyle = BorderStyle.Single;

            screen.Controls.Add(control1);
            screen.Controls.Add(control2);
            screen.Controls.Add(control3);

            screen.Footer.Text = screen.Name + ". Press escape.";

            screens.Add(screen);
        }
    }
}