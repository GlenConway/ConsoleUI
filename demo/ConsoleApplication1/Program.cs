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

            Utils.SetWindowPosition(0, 0);

            //Labels.SetupLabelScreens(screens);
            //TextBoxes.SetupTextBoxScreens(screens);
            //ListBoxes.SetupListBoxScreens(screens);
            //KeyPressedEvents.SetupKeyPressedEventScreens(screens);
            //ProgressBars.SetupProgressBars(screens);
            LoginScreen.SetupLoginScreen(screens);
            //LoadingScreen.SetupLoadingScreen(screens);

            ShowScreens();
        }

        private static void ShowScreens()
        {
            for (int i = 0; i < screens.Count; i++)
            {
                screens.Show(i);
            }
        }
    }
}