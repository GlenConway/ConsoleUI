# ConsoleUI
A little library to provide UI components in Windows Console Applications a la DOS legacy apps.

[![Nuget](https://img.shields.io/nuget/dt/consoleUI.svg)](http://nuget.org/packages/consoleUI)
[![Nuget](https://img.shields.io/nuget/v/consoleUI.svg)](http://nuget.org/packages/consoleUI)

## Controls

  * Label
  * TextBox
  * Button
  * ListBox
  * ProgressBar
  * Rectangle

## Features

  * None, Single or Double border
  * Shadow
  * Login Screen
  * Loading Screen

### Loading Screen
![Docs](https://raw.githubusercontent.com/GlenConway/ConsoleUI/master/docs/images/loading-screen.png)

### Login Screen
![Docs](https://raw.githubusercontent.com/GlenConway/ConsoleUI/master/docs/images/login-screen-1.png)
![Docs](https://raw.githubusercontent.com/GlenConway/ConsoleUI/master/docs/images/login-screen-2.png)
![Docs](https://raw.githubusercontent.com/GlenConway/ConsoleUI/master/docs/images/login-screen-3.png)

## Controls (To-Do)

  * Multi-line TextBox
  * Menus
  * CheckBox
  * Radio Button

## Some Code
### Using Built In Screens
``` C#
using System;
using System.Linq;

namespace ConsoleApplication1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // initialise a new instance of the helper loading screen
            var screen = new ConsoleUI.LoadingScreen("Loading Screen");

            // set the message text
            screen.Message = "Doing some work, please wait";

            // each screen has a footer that can display text
            screen.Footer.Text = "Working...";

            // there are no controls in the screen that can have the focus so
            // sleep for a bit before exiting.
            screen.Shown += (s, e) =>
            {
                System.Threading.Thread.Sleep(1500);

                // can access the controls through the Controls collection
                screen.Controls.OfType<ConsoleUI.ProgressBar>().First().BlockColor = ConsoleColor.Yellow;

                System.Threading.Thread.Sleep(1500);

                screen.Controls.OfType<ConsoleUI.ProgressBar>().First().BlockColor = ConsoleColor.Red;

                System.Threading.Thread.Sleep(1500);

                // don't need to call Exit() if there is only one screen
                // but you should if there you displaying more than one screen
                (s as ConsoleUI.Screen).Exit();
            };

            screen.Show();
        }
    }
}
```
#### Using the controls
```C#
using ConsoleUI;

namespace ConsoleApplication1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var screen = new Screen("List Box Pop Up");

            // setup the listbox and add some items
            var listBox = new ListBox();

            listBox.Left = 0;
            listBox.Top = 0;
            listBox.Width = screen.Width; ;
            listBox.Height = screen.Height - 1;
            listBox.BorderStyle = BorderStyle.Double;

            for (int i = 0; i < 50; i++)
            {
                listBox.Items.Add(string.Format("Item {0} - Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua", i + 1));
            }

            // setup the textbox
            var textBox = new TextBox();
            textBox.BorderStyle = BorderStyle.Double;
            textBox.Width = 8;
            textBox.Left = (screen.Width / 2) - (textBox.Width / 2);
            textBox.Top = (screen.Height / 2) - (textBox.Height / 2);
            textBox.MaxLength = 6;
            textBox.BackgroundColor = System.ConsoleColor.DarkGreen;
            textBox.Visible = false;
            textBox.HasShadow = true;

            // turn off enter key firing the tab event
            textBox.TreatEnterKeyAsTab = false;

            // handle the C key to show the textbox
            listBox.KeyPressed += (s, e) =>
            {
                if (e.Info.Key == System.ConsoleKey.C)
                {
                    textBox.Visible = true;
                    textBox.Focus();
                    e.Handled = true;
                }
            };

            // update the screen footer with the select item
            listBox.Selected += (s, e) =>
            {
                screen.Footer.Text = listBox.SelectedItem;
            };

            // update the screen footer
            textBox.TextChanged += (s, e) =>
            {
                screen.Footer.Text = string.Format("{0} => {1}", e.OrignalText ?? string.Empty, e.NewText);
            };

            // hide the textbox when focus switches back to the listbox
            listBox.Enter += (s, e) =>
            {
                textBox.Visible = false;
            };

            screen.Footer.Text = screen.Name + ". Press C to popup a text box, enter or escape.";

            // adding controls sets the tab order
            // unless explicitly set
            screen.Controls.Add(listBox, textBox);

            screen.Show();
        }
    }
}
```
