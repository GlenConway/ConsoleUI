using ConsoleUI;

namespace ConsoleApplication1
{
    internal static class ListBoxes
    {
        internal static void SetupListBoxScreens(ScreenCollection screens)
        {
            BasicListBoxScreen(screens);
            SingleBorderListBoxScreen(screens);
            DoubleBorderListBoxScreen(screens);
        }

        private static void BasicListBoxScreen(ScreenCollection screens)
        {
            var screen = new Screen("Basic List Boxes");

            var control1 = new ListBox();

            control1.Left = 0;
            control1.Top = 0;
            control1.Width = 20;
            control1.Height = 10;
            
            for (int i = 0; i < 20; i++)
            {
                control1.Items.Add(string.Format("Item {0}", i + 1));
            }

            var control2 = new ListBox();

            control2.Left = 30;
            control2.Top = 0;
            control2.Width = 30;
            control2.Height = 15;

            for (int i = 0; i < 40; i++)
            {
                control2.Items.Add(string.Format("Item {0}", i + 1));
            }

            var control3 = new ListBox();

            control3.Left = 0;
            control3.Top = 11;
            control3.Width = 25;
            control3.Height = 10;

            for (int i = 0; i < 5; i++)
            {
                control3.Items.Add(string.Format("Item {0}", i + 1));
            }
            
            screen.Controls.Add(control1);
            screen.Controls.Add(control2);
            screen.Controls.Add(control3);

            screen.Footer.Text = screen.Name + ". Press enter or escape.";

            screens.Add(screen);
        }
        private static void SingleBorderListBoxScreen(ScreenCollection screens)
        {
            var screen = new Screen("Single Border List Boxes");

            var control1 = new ListBox();

            control1.Left = 0;
            control1.Top = 0;
            control1.Width = 20;
            control1.Height = 10;
            control1.BorderStyle = BorderStyle.Single;

            for (int i = 0; i < 20; i++)
            {
                control1.Items.Add(string.Format("Item {0}", i + 1));
            }

            var control2 = new ListBox();

            control2.Left = 30;
            control2.Top = 0;
            control2.Width = 30;
            control2.Height = 15;
            control2.BorderStyle = BorderStyle.Single;

            for (int i = 0; i < 40; i++)
            {
                control2.Items.Add(string.Format("Item {0}", i + 1));
            }

            var control3 = new ListBox();

            control3.Left = 0;
            control3.Top = 11;
            control3.Width = 25;
            control3.Height = 10;
            control3.BorderStyle = BorderStyle.Single;

            for (int i = 0; i < 5; i++)
            {
                control3.Items.Add(string.Format("Item {0}", i + 1));
            }

            screen.Controls.Add(control1);
            screen.Controls.Add(control2);
            screen.Controls.Add(control3);

            screen.Footer.Text = screen.Name + ". Press enter or escape.";

            screens.Add(screen);
        }
        private static void DoubleBorderListBoxScreen(ScreenCollection screens)
        {
            var screen = new Screen("Double Border List Boxes");

            var control1 = new ListBox();

            control1.Left = 0;
            control1.Top = 0;
            control1.Width = 20;
            control1.Height = 10;
            control1.BorderStyle = BorderStyle.Double;
            control1.HasShadow = true;

            for (int i = 0; i < 20; i++)
            {
                control1.Items.Add(string.Format("Item {0}", i + 1));
            }

            var control2 = new ListBox();

            control2.Left = 30;
            control2.Top = 0;
            control2.Width = 30;
            control2.Height = 15;
            control2.BorderStyle = BorderStyle.Double;
            control2.HasShadow = true;

            for (int i = 0; i < 40; i++)
            {
                control2.Items.Add(string.Format("Item {0}", i + 1));
            }

            var control3 = new ListBox();

            control3.Left = 0;
            control3.Top = 11;
            control3.Width = 25;
            control3.Height = 10;
            control3.BorderStyle = BorderStyle.Double;
            control3.HasShadow = true;

            for (int i = 0; i < 5; i++)
            {
                control3.Items.Add(string.Format("Item {0}", i + 1));
            }

            screen.Controls.Add(control1);
            screen.Controls.Add(control2);
            screen.Controls.Add(control3);

            screen.Footer.Text = screen.Name + ". Press enter or escape.";

            screens.Add(screen);
        }
    }
}