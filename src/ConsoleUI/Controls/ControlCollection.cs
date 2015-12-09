using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUI
{
    public class ControlCollection<T> : ICollection<T> where T : Control
    {
        private readonly IControlContainer owner;
        private bool exit;
        private IList<T> list = new List<T>();
        private int tabOrder = 0;

        public ControlCollection(IControlContainer owner)
        {
            this.owner = owner;
        }

        public event EventHandler EscPressed;

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

        public void Add(params T[] items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void Add(T item)
        {
            item.Owner = owner;

            list.Add(item);

            var lastControl = LastControl();

            if (lastControl != null)
            {
                item.TabOrder = lastControl.TabOrder + 1;
            }

            item.TabPressed += (s, e) =>
            {
                TabToNextControl(e.Shift);
            };

            item.Enter += (s, e) =>
            {
                foreach (var control in list)
                {
                    control.HasFocus = control == s;

                    if (control == s)
                        tabOrder = control.TabOrder;
                }
            };

            item.EscPressed += (s, e) =>
            {
                OnEscPressed(s, e);
            };
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public T GetHasFocus()
        {
            return list.Where(p => p.HasFocus).LastOrDefault();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public bool Remove(T item)
        {
            return list.Remove(item);
        }

        internal void Exit()
        {
            exit = true;

            RemoveFocus();
        }

        internal void RemoveFocus()
        {
            foreach (var item in list.Where(p => p.HasFocus))
            {
                item.HasFocus = false;
            }

            tabOrder = 0;
        }

        internal void SetFocus()
        {
            if (exit)
                return;

            var control = list.OrderBy(p => p.TabOrder).Where(p => p.TabStop).Where(p => p.Visible).Where(p => p.TabOrder >= tabOrder).FirstOrDefault();

            if (control == null)
                return;

            control.Focus();
        }

        internal void SetFocus(T control)
        {
            if (exit)
                return;

            control.Focus();
        }

        internal void TabToNextControl(bool shift)
        {
            if (exit)
                return;

            if (list.Where(p => p.TabStop).Where(p => p.Visible).Count() == 1)
                return;

            var last = LastControl();

            if (last == null)
                return;

            var lastTabOrder = last.TabOrder;

            if (shift && tabOrder > 0)
            {
                var previous = list.Where(p => p.TabStop).Where(p => p.Visible).Where(p => p.TabOrder < tabOrder).OrderByDescending(p => p.TabOrder).FirstOrDefault();

                if (previous != null)
                    tabOrder = previous.TabOrder;
                else
                    tabOrder = last.TabOrder;
            }
            else
            if (tabOrder < lastTabOrder)
                tabOrder++;
            else
                tabOrder = 0;

            SetFocus();
        }

        protected virtual void OnEscPressed(object sender, EventArgs e)
        {
            tabOrder = 0;

            if (EscPressed != null)
                EscPressed(sender, e);
        }

        private T LastControl()
        {
            return list.OrderBy(p => p.TabOrder).Where(p => p.Visible).LastOrDefault();
        }
    }
}