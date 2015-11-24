using System;

namespace ConsoleUI
{
    public class KeyPressedEventArgs : EventArgs
    {
        private ConsoleKeyInfo info;

        public KeyPressedEventArgs(ConsoleKeyInfo info)
        {
            this.info = info;
        }

        public bool Handled
        {
            get;
            set;
        }

        public ConsoleKeyInfo Info
        {
            get
            {
                return info;
            }
        }
    }
}