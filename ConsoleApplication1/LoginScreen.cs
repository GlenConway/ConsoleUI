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

            screen.Footer.Text = "Try admin admin.";

            screens.Add(screen);

            screen.Login += Screen_Login;
        }

        private static void Screen_Login(object sender, LoginEventArgs e)
        {
            e.Success = (e.Username == "admin" & e.Password == "admin");
        }
    }
}