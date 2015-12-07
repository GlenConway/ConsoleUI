namespace ConsoleUI
{
    public class Menu : InputControl
    {
        private ControlCollection menuItems;

        public Menu(IControlContainer owner)
        {
            Owner = owner;
            Height = 1;
            TabStop = true;
        }

        public int Index { get; set; }

        public ControlCollection MenuItems
        {
            get
            {
                if (menuItems == null)
                    menuItems = new ControlCollection(Owner);

                return menuItems;
            }
        }
    }
}