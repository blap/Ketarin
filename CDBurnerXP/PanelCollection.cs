using System;
using System.Collections.Generic;
using System.Text;

namespace CDBurnerXP.Controls
{
    public class PanelCollection : ICollection<ListBoxPanel>, IList<ListBoxPanel>, System.Collections.IList
    {
        private List<ListBoxPanel> m_Panels = new List<ListBoxPanel>();
        public event EventHandler? PanelAdded;
        public event EventHandler? PanelRemoved;

        #region ICollection<ListBoxPanel> Members

        public void Add(ListBoxPanel item)
        {
            m_Panels.Add(item);

            if (PanelAdded != null)
            {
                PanelAdded(item, EventArgs.Empty);
            }
        }

        public void Clear()
        {
            ListBoxPanel[] copyList = m_Panels.ToArray();
            m_Panels.Clear();
            foreach (ListBoxPanel panel in copyList)
            {
                if (PanelRemoved != null)
                {
                    PanelRemoved(panel, EventArgs.Empty);
                }
            }
        }

        public bool Contains(ListBoxPanel item)
        {
            return m_Panels.Contains(item);
        }

        public void CopyTo(ListBoxPanel[] array, int arrayIndex)
        {
            m_Panels.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get {
                return m_Panels.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(ListBoxPanel item)
        {
            bool res = m_Panels.Remove(item);
            if (PanelRemoved != null)
            {
                PanelRemoved(item, EventArgs.Empty);
            }
            return res;
        }

        #endregion

        #region IEnumerable<ListBoxPanel> Members

        public IEnumerator<ListBoxPanel> GetEnumerator()
        {
            return m_Panels.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_Panels.GetEnumerator();
        }

        #endregion

        #region IList<ListBoxPanel> Members

        public int IndexOf(ListBoxPanel item)
        {
            return m_Panels.IndexOf(item);
        }

        public void Insert(int index, ListBoxPanel item)
        {
            m_Panels.Insert(index, item);
            if (PanelAdded != null)
            {
                PanelAdded(item, EventArgs.Empty);
            }
        }

        public void RemoveAt(int index)
        {
            m_Panels.RemoveAt(index);
        }

        public ListBoxPanel this[int index]
        {
            get
            {
                return m_Panels[index];
            }
            set
            {
                m_Panels[index] = value;
            }
        }

        #endregion

        #region IList Members

        public int Add(object? value)
        {
            if (value is ListBoxPanel panel)
            {
                Add(panel);
                return m_Panels.Count - 1;
            }
            throw new ArgumentException("Value must be of type ListBoxPanel", nameof(value));
        }

        public bool Contains(object? value)
        {
            return value is ListBoxPanel panel && m_Panels.Contains(panel);
        }

        public int IndexOf(object? value)
        {
            return value is ListBoxPanel panel ? m_Panels.IndexOf(panel) : -1;
        }

        public void Insert(int index, object? value)
        {
            if (value is ListBoxPanel panel)
            {
                Insert(index, panel);
            }
        }

        public void Remove(object? value)
        {
            if (value is ListBoxPanel panel)
            {
                Remove(panel);
            }
        }

        object? System.Collections.IList.this[int index]
        {
            get
            {
                return m_Panels[index];
            }
            set
            {
                if (value is ListBoxPanel panel)
                {
                    m_Panels[index] = panel;
                }
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            m_Panels.CopyTo((ListBoxPanel[])array, index);
        }

        public bool IsSynchronized
        {
            get {
                return false;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public object SyncRoot
        {
            get { return m_Panels; }
        }

        #endregion
    }
}
