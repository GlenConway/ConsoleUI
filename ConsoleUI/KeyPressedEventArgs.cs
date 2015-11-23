using System;

namespace ConsoleUI
{
    public class KeyPressedEventArgs : EventArgs
    {
        private bool handled;
        private ConsoleKeyInfo info;

        public KeyPressedEventArgs(ConsoleKeyInfo info)
        {
            this.info = info;
        }

        public bool Handled
        {
            get
            {
                return handled;
            }
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