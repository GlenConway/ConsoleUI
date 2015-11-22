using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleUI
{
    public class ScreenCollection : ICollection<Screen>
    {
        private IList<Screen> list = new List<Screen>();

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

        public void Add(Screen item)
        {
            list.Add(item);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(Screen item)
        {
            return list.Contains(item);
        }

        public void CopyTo(Screen[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Screen> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public bool Remove(Screen item)
        {
            return list.Remove(item);
        }

        public void Show(int index)
        {
            this[index].Show();
        }

        public virtual Screen this[int index]
        {
            get
            {
                return list[index];
            }
        }
    }
}