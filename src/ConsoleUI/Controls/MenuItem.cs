namespace ConsoleUI
{
    public class MenuItem : Control
    {
        private bool isSeparator;

        public MenuItem(IControlContainer owner)
        {
            Owner = owner;
            Height = 1;
            TabStop = true;

            FocusBackgroundColor = System.ConsoleColor.Red;
            FocusForegroundColor = System.ConsoleColor.White;
        }

        public bool IsSeparator
        {
            get
            {
                return isSeparator;
            }
            set
            {
                SetProperty(ref isSeparator, value);
            }
        }

        public override bool TabStop
        {
            get
            {
                if (IsSeparator)
                    return false;

                return base.TabStop;
            }

            set
            {
                if (!IsSeparator)
                    base.TabStop = value;
            }
        }
    }
}