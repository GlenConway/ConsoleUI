using ConsoleUI;
using System;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static ScreenCollection screens = new ScreenCollection();

        static void Main(string[] args)
        {
            Console.WindowHeight = 40;
            Console.WindowWidth = 132;
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;

            //Labels.SetupLabelScreens(screens);
            TextBoxes.SetupTextBoxScreens(screens);
            //ListBoxes.SetupListBoxScreens(screens);
            KeyPressedEvents.SetupListBoxScreens(screens);

            ShowScreens();
        }

        private static void ShowScreens()
        {
            for (int i = 0; i < screens.Count; i++)
            {
                screens[i].Show();
            }
        }
    }
}