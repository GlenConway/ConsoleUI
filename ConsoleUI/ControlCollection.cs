using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUI
{
    public class ControlCollection : ICollection<Control>
    {
        private readonly IControlContainer owner;
        private bool exit;
        private IList<Control> list = new List<Control>();
        private int tabOrder = 0;

        public ControlCollection(IControlContainer owner)
        {
            this.owner = owner;
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return list.IsReadOnly;
            }
        }

        private IEnumerable<InputControl> InputControls
        {
            get
            {
                var result = list.OfType<InputControl>();

                return result;
            }
        }

        public void Add(params Control[] items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void Add(Control item)
        {
            item.Owner = owner;

            list.Add(item);

            var inputControl = item as InputControl;

            if (inputControl != null)
            {
                var lastControl = LastControl();

                if (lastControl != null)
                {
                    inputControl.TabOrder = lastControl.TabOrder + 1;
                }

                inputControl.TabPressed += (s, e) =>
                {
                    TabToNextControl(e.Shift);
                };

                inputControl.Enter += (s, e) =>
                {
                    foreach (var control in InputControls)
                    {
                        control.HasFocus = control == s;

                        if (control == s)
                            tabOrder = control.TabOrder;
                    }
                };
            }
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(Control item)
        {
            return list.Contains(item);
        }

        public void CopyTo(Control[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Control> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public Control HasFocus()
        {
            return InputControls.Where(p => p.HasFocus).LastOrDefault();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public bool Remove(Control item)
        {
            return list.Remove(item);
        }

        internal void Exit()
        {
            exit = true;

            foreach (var item in InputControls.Where(p => p.HasFocus))
            {
                item.HasFocus = false;
            }

            tabOrder = 0;
        }

        internal void SetFocus()
        {
            if (exit)
                return;

            var control = InputControls.OrderBy(p => p.TabOrder).Where(p => p.TabStop).Where(p => p.Visible).Where(p => p.TabOrder >= tabOrder).FirstOrDefault();

            if (control == null)
                return;

            control.Focus();
        }

        internal void SetFocus(InputControl control)
        {
            if (exit)
                return;

            control.Focus();
        }

        private InputControl LastControl()
        {
            return InputControls.OrderBy(p => p.TabOrder).Where(p => p.Visible).LastOrDefault();
        }

        private void TabToNextControl(bool shift)
        {
            if (exit)
                return;

            if (InputControls.Where(p => p.TabStop).Where(p => p.Visible).Count() == 1)
                return;

            var last = LastControl();

            if (last == null)
                return;

            var lastTabOrder = last.TabOrder;

            if (shift && tabOrder > 0)
                tabOrder--;
            else
            if (tabOrder < lastTabOrder)
                tabOrder++;
            else
                tabOrder = 0;

            SetFocus();
        }
    }
}