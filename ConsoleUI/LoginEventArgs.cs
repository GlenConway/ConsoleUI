using System;

namespace ConsoleUI
{
    public class LoginEventArgs : EventArgs
    {
        private readonly string password;
        private readonly string username;

        public LoginEventArgs(string username, string password)
        {
            this.password = password;
            this.username = username;
        }

        public string FailureMessage { get; set; }
        public string Password { get { return password; } }
        public bool Success { get; set; }
        public string Username { get { return username; } }
    }
}