using ConsoleUI;

namespace ConsoleApplication1
{
    internal static class KeyPressedEvents
    {
        internal static void SetupListBoxScreens(ScreenCollection screens)
        {
            ListBoxPopup(screens);
        }

        private static void ListBoxPopup(ScreenCollection screens)
        {
            var screen = new Screen("List Box Pop Up");

            var control1 = new ListBox();

            control1.Left = 0;
            control1.Top = 0;
            control1.Width = screen.Width; ;
            control1.Height = screen.Height - 1;
            control1.BorderStyle = BorderStyle.Double;

            for (int i = 0; i < 50; i++)
            {
                control1.Items.Add(string.Format("Item {0}", i + 1));
            }

            screen.Controls.Add(control1);

            var textBox = new TextBox();
            textBox.BorderStyle = BorderStyle.Double;
            textBox.Width = 8;
            textBox.Left = (screen.Width / 2) - (textBox.Width / 2);
            textBox.Top = (screen.Height / 2) - (textBox.Height / 2);
            textBox.MaxLength = 6;
            textBox.BackgroundColor = System.ConsoleColor.DarkGreen;
            textBox.Visible = false;

            screen.Controls.Add(textBox);

            screen.Footer.Text = screen.Name + ". Press C to popup a text box, enter or escape.";

            screens.Add(screen);

            control1.KeyPressed += (s, e) =>
            {
                if (e.Info.Key == System.ConsoleKey.C)
                {
                    textBox.Visible = true;
                    textBox.Focus();
                }
            };

            control1.Selected += (s, e) =>
            {
                screen.Footer.Text = control1.SelectedItem;
            };

            textBox.Leave += (s, e) =>
            {
                screen.Footer.Text = textBox.Text;
            };
        }
    }
}