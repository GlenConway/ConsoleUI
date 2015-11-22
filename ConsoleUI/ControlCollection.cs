using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUI
{
    public class ControlCollection : ICollection<Control>
    {
        private readonly IControlContainer owner;
        private IList<Control> list = new List<Control>();
        private int tabOrder = 0;

        public ControlCollection(IControlContainer owner)
        {
            this.owner = owner;
        }

        public event EventHandler Repaint;

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

        public void Add(Control item)
        {
            item.Owner = owner;

            var lastControl = LastControl();

            if (lastControl != null)
            {
                item.TabOrder = lastControl.TabOrder + 1;
            }

            list.Add(item);

            item.TabPressed += (s, e) =>
            {
                var last = LastControl();

                if (last == null)
                    return;

                var lastTabOrder = last.TabOrder;

                if (e.Shift && tabOrder > 0)
                    tabOrder--;
                else
                if (tabOrder < lastTabOrder)
                    tabOrder++;
                else
                    tabOrder = 0;

                SetFocus();
            };

            item.Repaint += (s, e) =>
            {
                OnRepaint();
            };
        }

        public void Clear()
        {
            throw new NotImplementedException();
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public bool Remove(Control item)
        {
            return list.Remove(item);
        }

        public void SetFocus()
        {
            var control = list.OrderBy(p => p.TabOrder).Where(p => p.TabStop).Where(p => p.TabOrder >= tabOrder).FirstOrDefault();

            if (control == null)
                return;

            control.Focus();
        }

        protected virtual void OnRepaint()
        {
            if (Repaint != null)
                Repaint(this, new EventArgs());
        }

        private Control LastControl()
        {
            return list.OrderBy(p => p.TabOrder).LastOrDefault();
        }
    }
}