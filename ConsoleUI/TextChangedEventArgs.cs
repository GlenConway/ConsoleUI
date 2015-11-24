using System;

namespace ConsoleUI
{
    public class TextChangedEventArgs : EventArgs
    {
        private string newText;
        private string originalText;

        public TextChangedEventArgs(string originalText, string newText)
        {
            this.originalText = originalText;
            this.newText = newText;
        }

        public string NewText
        {
            get
            {
                return newText;
            }
        }

        public string OrignalText
        {
            get
            {
                return originalText;
            }
        }
    }
}