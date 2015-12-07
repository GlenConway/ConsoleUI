using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class MenuItem : Control
    {
        public MenuItem(IControlContainer owner)
        {
            Owner = owner;
            Height = 1;
        }

        public int Index { get; set; }
    }
}
