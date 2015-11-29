using System;

namespace ConsoleUI
{
    public class LoginScreen : Screen
    {
        private Button cancelButton;
        private Label failureLabel;
        private Button loginButton;
        private Label passwordLabel;
        private TextBox passwordTextBox;
        private ProgressBar progressBar;
        private Rectangle rectangle;
        private Label usernameLabel;
        private TextBox usernameTextBox;

        public LoginScreen() : this("Login Screen")
        {
        }

        public LoginScreen(string name) : base(name)
        {
            usernameLabel = new Label();
            passwordLabel = new Label();
            usernameTextBox = new TextBox();
            passwordTextBox = new TextBox();
            rectangle = new Rectangle();
            loginButton = new Button();
            cancelButton = new Button();
            failureLabel = new Label();
            progressBar = new ProgressBar();

            SetupControls();

            loginButton.Click += LoginButton_Click;
            cancelButton.Click += CancelButton_Click;
            cancelButton.EscPressed += CancelButton_Click;
            loginButton.EscPressed += CancelButton_Click;
            usernameTextBox.EscPressed += CancelButton_Click;
            passwordTextBox.EscPressed += CancelButton_Click;
            passwordTextBox.KeyPressed += PasswordTextBox_KeyPressed;
        }

        public event EventHandler Cancelled;

        public event EventHandler<LoginEventArgs> Login;

        public string Password
        {
            get
            {
                return passwordTextBox.Text;
            }
            set
            {
                passwordTextBox.Text = value;
            }
        }

        public string Username
        {
            get
            {
                return usernameTextBox.Text;
            }
            set
            {
                usernameTextBox.Text = value;
            }
        }

        public override void Show()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                base.Show();

                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                Show(passwordTextBox);

                return;
            }

            Show(loginButton);
        }

        protected void OnCancel()
        {
            if (Cancelled != null)
                Cancelled(this, new EventArgs());
        }

        protected virtual void OnLogin(LoginEventArgs args)
        {
            if (Login != null)
                Login(this, args);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            OnCancel();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(usernameTextBox.Text))
            {
                Console.CursorVisible = false;

                progressBar.Visible = true;

                var args = new LoginEventArgs(Username, Password);

                OnLogin(args);

                progressBar.Visible = false;

                if (args.Success)
                    return;

                failureLabel.Visible = true;
                failureLabel.Text = args.FailureMessage;
            }

            Password = string.Empty;

            Draw();
            Paint();

            passwordTextBox.Focus();
        }

        private void PasswordTextBox_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (e.Info.Key == ConsoleKey.Enter)
            {
                LoginButton_Click(sender, new EventArgs());
                e.Handled = true;
            }
        }

        private void SetupControls()
        {
            var y = Height / 2;
            y--;
            y--;

            var labelWidth = 10;
            var textBoxWidth = 30;

            var x = Width / 2;
            x -= (labelWidth + textBoxWidth) / 2;

            usernameLabel.Text = "Username:";
            usernameLabel.Width = labelWidth;
            usernameLabel.Top = y;
            usernameLabel.Left = x;
            usernameLabel.ForegroundColor = ConsoleColor.Yellow;

            passwordLabel.Text = "Password:";
            passwordLabel.Width = labelWidth;
            passwordLabel.Top = y + 1;
            passwordLabel.Left = x;
            passwordLabel.ForegroundColor = ConsoleColor.Yellow;

            usernameTextBox.Width = textBoxWidth;
            usernameTextBox.Top = y;
            usernameTextBox.Left = usernameLabel.Left + usernameLabel.Width;

            passwordTextBox.TextBoxType = TextBoxType.Password;
            passwordTextBox.Width = textBoxWidth;
            passwordTextBox.Top = y + 1;
            passwordTextBox.Left = passwordLabel.Left + passwordLabel.Width;
            passwordTextBox.TreatEnterKeyAsTab = false;

            rectangle.BorderStyle = BorderStyle.Double;
            rectangle.Left = usernameLabel.Left - 2;
            rectangle.Top = usernameLabel.Top - 2;
            rectangle.Width = usernameLabel.Width + usernameTextBox.Width + 4;
            rectangle.Height = 8;
            rectangle.HasShadow = true;

            loginButton.Text = "Login";
            loginButton.Width = 8;
            loginButton.Top = passwordTextBox.Top + passwordTextBox.Height + 1;
            loginButton.Left = passwordTextBox.Left;
            loginButton.BackgroundColor = ConsoleColor.Gray;
            loginButton.ForegroundColor = ConsoleColor.Black;
            loginButton.TextAlign = TextAlign.Center;
            loginButton.HasShadow = true;

            cancelButton.Text = "Cancel";
            cancelButton.Width = 8;
            cancelButton.Top = loginButton.Top;
            cancelButton.Left = loginButton.Left + loginButton.Width + 1;
            cancelButton.BackgroundColor = ConsoleColor.Gray;
            cancelButton.ForegroundColor = ConsoleColor.Black;
            cancelButton.TextAlign = TextAlign.Center;
            cancelButton.HasShadow = true;

            failureLabel.Width = rectangle.Width * 2;
            failureLabel.BorderStyle = BorderStyle.Double;
            failureLabel.HasShadow = true;
            failureLabel.Left = (Width / 2) - (failureLabel.Width / 2);
            failureLabel.Top = rectangle.Bottom + 3;
            failureLabel.BackgroundColor = ConsoleColor.DarkRed;
            failureLabel.ForegroundColor = ConsoleColor.White;
            failureLabel.Visible = false;

            progressBar.Top = (Height / 2) - (progressBar.Height / 2);
            progressBar.Width = 30;
            progressBar.Left = (Width / 2) - (progressBar.Width / 2);
            progressBar.BorderStyle = BorderStyle.Double;
            progressBar.BlockColor = ConsoleColor.Green;
            progressBar.HasShadow = true;
            progressBar.ProgressBarStyle = ProgressBarStyle.Marquee;
            progressBar.Visible = false;

            Controls.Add(rectangle, usernameLabel, usernameTextBox, passwordLabel, passwordTextBox, loginButton, cancelButton, failureLabel, progressBar);
        }
    }
}