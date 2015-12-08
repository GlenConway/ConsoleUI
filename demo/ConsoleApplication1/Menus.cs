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

            menu.AddMenuItem("Cut");
            menu.AddMenuItem("Copy");
            menu.AddSeparator();
            menu.AddMenuItem("Paste");

            menuBar.Menus.Add(menu);
        }

        private static void SetupFileMenu(Screen screen, MenuBar menuBar)
        {
            var menu = new Menu(screen);
            menu.Text = "File";

            menu.AddMenuItem("New");
            menu.AddMenuItem("Open");
            menu.AddSeparator();
            menu.AddMenuItem("Save");
            menu.AddSeparator();
            menu.AddMenuItem("Close");

            menuBar.Menus.Add(menu);
        }

        private static void SetupViewMenu(Screen screen, MenuBar menuBar)
        {
            var menu = new Menu(screen);
            menu.Text = "View";

            menu.AddMenuItem("Dog");
            menu.AddMenuItem("Cat");
            menu.AddSeparator();
            menu.AddMenuItem("Mouse");
            menu.AddSeparator();
            menu.AddMenuItem("Monkey");
            menu.AddSeparator();
            menu.AddMenuItem("Horse");

            menuBar.Menus.Add(menu);
        }
    }
}