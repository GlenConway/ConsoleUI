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
            MultilineBasicTextBoxScreen(screens);
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
            control3.Width = control3.MaxLength;
            control3.TextBoxType = TextBoxType.Password;

            var control4 = new TextBox();

            control4.Left = 0;
            control4.Top = control3.Top + control3.Height;
            control4.MaxLength = 20;
            control4.TextAlign = TextAlign.Right;

            screen.Controls.Add(control1);
            screen.Controls.Add(control2);
            screen.Controls.Add(control3);
            screen.Controls.Add(control4);

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

        private static void MultilineBasicTextBoxScreen(ScreenCollection screens)
        {
            var screen = new Screen("Multiline Basic Text Boxes");

            var control1 = new TextBox();

            control1.Left = 10;
            control1.Top = 0;
            control1.MaxLength = 150;
            control1.Width = control1.MaxLength;
            control1.TreatEnterKeyAsTab = false;
            control1.Height = 5;
            control1.Width = 40;
            control1.TextBoxType = TextBoxType.Multiline;

            var control2 = new TextBox();

            control2.Left = 0;
            control2.Top = control1.Top + control1.Height;
            control2.Width = screen.Width;
            control2.TextAlign = TextAlign.Center;
            control2.Height = 8;
            control2.Width = 20;
            control2.TextBoxType = TextBoxType.Multiline;

            var control3 = new TextBox();

            control3.Left = 0;
            control3.Top = control2.Top + control2.Height;
            control3.Height = 20;
            control3.Width = 20;
            control3.TextBoxType = TextBoxType.Multiline;
            control3.Text = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?";
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