using ConsoleUI;

namespace ConsoleApplication1
{
    internal static class LoadingScreen
    {
        internal static void SetupLoadingScreen(ScreenCollection screens)
        {
            // initialise a new instance of the helper loading screen
            var screen = new ConsoleUI.LoadingScreen("Loading Screen");

            // set the message text
            screen.Message = "Doing some work, please wait";

            // each screen has a footer that can display text
            screen.Footer.Text = "Working...";

            // add the screen to the screens collection
            screens.Add(screen);

            // there are no controls in the screen that can have the focus so
            // sleep for a bit before exiting.
            screen.Shown += (s, e) =>
            {
                System.Threading.Thread.Sleep(3000);

                (s as Screen).Exit();
            };
        }
    }
}