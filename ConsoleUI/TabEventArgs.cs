using System;

namespace ConsoleUI
{
    public class TabEventArgs : EventArgs
    {
        public bool Shift { get; set; }

        public TabEventArgs()
        {

        }

        public TabEventArgs(bool shift)
        {
            Shift = shift;
        }
    }
}