using ConsoleUI;
using System;

namespace ConsoleApplication1
{
    internal static class Menus
    {
        internal static void SetupMenu(ScreenCollection screens)
        {
            var screen = new Screen("Menus");

            var menuBar = new MenuBar(screen);
            menuBar.Left = 0;
            menuBar.Top = 0;
            menuBar.Width = screen.Width;

            SetupFileMenu(screen, menuBar);
            SetupEditMenu(screen, menuBar);
            SetupViewMenu(screen, menuBar);

            screen.Controls.Add(menuBar);

            screens.Add(screen);

            screen.Shown += (s, e) =>
            {
                Console.ReadKey(true);

                (s as Screen).Exit();
            };
        }

        private static void SetupEditMenu(Screen screen, MenuBar menuBar)
        {
            var menu = new Menu(screen);
            menu.Text = "Edit";

            menuBar.Menus.Add(menu);
        }

        private static void SetupFileMenu(Screen screen, MenuBar menuBar)
        {
            var menu = new Menu(screen);
            menu.Text = "File";

            menuBar.Menus.Add(menu);
        }

        private static void SetupViewMenu(Screen screen, MenuBar menuBar)
        {
            var menu = new Menu(screen);
            menu.Text = "View";

            menuBar.Menus.Add(menu);
        }
    }
}