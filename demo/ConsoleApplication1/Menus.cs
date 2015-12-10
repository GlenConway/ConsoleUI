using ConsoleUI;

namespace ConsoleApplication1
{
    internal static class Menus
    {
        internal static void SetupMenu(ScreenCollection screens)
        {
            var screen = new Screen("Menus");
            MenuBar menuBar = SetupMenuBar(screen);

            screen.Controls.Add(menuBar);

            screens.Add(screen);
        }

        internal static MenuBar SetupMenuBar(Screen screen)
        {
            var menuBar = new MenuBar(screen);
            menuBar.Left = 0;
            menuBar.Top = 0;
            menuBar.Width = screen.Width;

            SetupFileMenu(screen, menuBar);
            SetupEditMenu(screen, menuBar);
            SetupViewMenu(screen, menuBar);

            return menuBar;
        }

        private static void SetupEditMenu(Screen screen, MenuBar menuBar)
        {
            var menu = new Menu(screen);
            menu.Text = "Edit";

            AddMenuItem(menu, "Cut");
            AddMenuItem(menu, "Copy");
            menu.AddSeparator();
            AddMenuItem(menu, "Paste");

            menuBar.Menus.Add(menu);
        }

        private static void SetupFileMenu(Screen screen, MenuBar menuBar)
        {
            var menu = new Menu(screen);
            menu.Text = "File";

            AddMenuItem(menu, "New");
            AddMenuItem(menu, "Open");
            menu.AddSeparator();
            AddMenuItem(menu, "Save");
            menu.AddSeparator();
            AddMenuItem(menu, "Close");

            menuBar.Menus.Add(menu);
        }

        private static void AddMenuItem(Menu menu, string text)
        {
            var menuItem = menu.AddMenuItem(text);

            menuItem.Selected += (s, e) =>
            {
                ((Screen)menu.Owner).SetFooterText("Selected: " + ((MenuItem)s).Text);
            };

            menuItem.Enter += (s, e) =>
            {
                ((Screen)menu.Owner).SetFooterText("Enter: " + ((MenuItem)s).Text);
            };
        }

        private static void SetupViewMenu(Screen screen, MenuBar menuBar)
        {
            var menu = new Menu(screen);
            menu.Text = "Animals";
            
            AddMenuItem(menu, "Dog");
            AddMenuItem(menu, "Cat");
            menu.AddSeparator();
            AddMenuItem(menu, "Mouse");
            menu.AddSeparator();
            AddMenuItem(menu, "Monkey");
            menu.AddSeparator();
            AddMenuItem(menu, "Horse");
            menu.AddSeparator();
            AddMenuItem(menu, "Hummingbird hawk-moth");

            menuBar.Menus.Add(menu);
        }
    }
}