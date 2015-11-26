using ConsoleUI;

namespace ConsoleApplication1
{
    internal static class LoginScreen
    {
        internal static void SetupLoginScreen(ScreenCollection screens)
        {
            ListBoxPopup(screens);
        }

        private static void ListBoxPopup(ScreenCollection screens)
        {
            var screen = new ConsoleUI.LoginScreen();

            screens.Add(screen);
        }
    }
}