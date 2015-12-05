using System;

namespace ConsoleUI
{
    public class LoadingScreen : Screen
    {
        private Label label;

        private ProgressBar progressBar;

        private Rectangle rectangle;

        public LoadingScreen(string name) : base(name)
        {
            SetupControls();
        }

        public string Message
        {
            get
            {
                return label.Text;
            }
            set
            {
                label.Text = value;
            }
        }

        private void SetupControls()
        {
            label = new Label();
            progressBar = new ProgressBar();
            rectangle = new Rectangle();

            rectangle.BorderStyle = BorderStyle.Double;
            rectangle.Height = 8;
            rectangle.Width = Width - 6;
            rectangle.Top = Height / 2 - rectangle.Height / 2;
            rectangle.Left = Width / 2 - rectangle.Width / 2;
            rectangle.HasShadow = true;

            label.Width = rectangle.Width - 4;
            label.Top = rectangle.Top + 2;
            label.Left = rectangle.Left + 2;
            label.TextAlign = TextAlign.Center;

            progressBar.Width = label.Width / 2;
            progressBar.ProgressBarStyle = ProgressBarStyle.Marquee;
            progressBar.Left = Width / 2 - progressBar.Width / 2;
            progressBar.Top = label.Top + 2;
            progressBar.BorderStyle = BorderStyle.Single;
            progressBar.BlockColor = ConsoleColor.Green;

            Controls.Add(rectangle, label, progressBar);
        }
    }
}