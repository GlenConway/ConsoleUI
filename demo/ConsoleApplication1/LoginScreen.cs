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
            screen.Username = "admin";

            screen.Footer.Text = "Try admin admin.";

            screens.Add(screen);

            screen.Login += Screen_Login;
        }

        private static void Screen_Login(object sender, LoginEventArgs e)
        {
            System.Threading.Thread.Sleep(2000);

            e.Success = (e.Username == "admin" & e.Password == "admin");

            if (!e.Success)
                e.FailureMessage = "Take the hint.";
        }
    }
}