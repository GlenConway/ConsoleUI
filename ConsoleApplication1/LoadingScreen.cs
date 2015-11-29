using ConsoleUI;

namespace ConsoleApplication1
{
    internal static class LoadingScreen
    {
        internal static void SetupLoadingScreen(ScreenCollection screens)
        {
            ListBoxPopup(screens);
        }

        private static void ListBoxPopup(ScreenCollection screens)
        {
            var screen = new ConsoleUI.LoadingScreen("Loading Screen");
            screen.Message = "Doing some work, please wait";

            screen.Footer.Text = "Working...";

            screens.Add(screen);

            screen.Shown += Screen_Shown;
        }

        private static void Screen_Shown(object sender, System.EventArgs e)
        {
            System.Threading.Thread.Sleep(6000);
        }
        
    }
}