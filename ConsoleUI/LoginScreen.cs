using System;

namespace ConsoleUI
{
    public class LoginScreen : Screen
    {
        private Button cancelButton;
        private Button loginButton;
        private Label passwordLabel;
        private TextBox passwordTextBox;
        private Rectangle rectangle;
        private Label usernameLabel;
        private TextBox usernameTextBox;
        
        public LoginScreen() : base("Login Screen")
        {
            usernameLabel = new Label();
            passwordLabel = new Label();
            usernameTextBox = new TextBox();
            passwordTextBox = new TextBox();
            rectangle = new Rectangle();
            loginButton = new Button();
            cancelButton = new Button();

            SetupControls();

            loginButton.Click += LoginButton_Click;
            cancelButton.Click += CancelButton_Click;
            cancelButton.EscPressed += CancelButton_Click;
            loginButton.EscPressed += CancelButton_Click;
            usernameTextBox.EscPressed += CancelButton_Click;
            usernameTextBox.EscPressed += CancelButton_Click;
        }

        public event EventHandler Cancelled;

        protected void OnCancel()
        {
            if (Cancelled != null)
                Cancelled(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            OnCancel();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(usernameTextBox.Text))
            {
                loginButton.Draw();
                usernameTextBox.Focus();
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

            rectangle.BorderStyle = BorderStyle.Double;
            rectangle.Left = usernameLabel.Left - 2;
            rectangle.Top = usernameLabel.Top - 2;
            rectangle.Width = usernameLabel.Width + usernameTextBox.Width + 4;
            rectangle.Height = 8;

            loginButton.Text = "Login";
            loginButton.Width = 8;
            loginButton.Top = passwordTextBox.Top + passwordTextBox.Height + 1;
            loginButton.Left = passwordTextBox.Left;
            loginButton.BackgroundColor = ConsoleColor.DarkBlue;
            loginButton.ForegroundColor = ConsoleColor.White;
            loginButton.TextAlign = TextAlign.Center;

            cancelButton.Text = "Cancel";
            cancelButton.Width = 8;
            cancelButton.Top = loginButton.Top;
            cancelButton.Left = loginButton.Left + loginButton.Width + 1;
            cancelButton.BackgroundColor = ConsoleColor.DarkBlue;
            cancelButton.ForegroundColor = ConsoleColor.Gray;
            cancelButton.TextAlign = TextAlign.Center;

            Controls.Add(rectangle, usernameLabel, usernameTextBox, passwordLabel, passwordTextBox, loginButton, cancelButton);
        }
    }
}