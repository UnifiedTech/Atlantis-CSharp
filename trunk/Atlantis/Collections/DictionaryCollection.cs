namespace Atlantis.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DictionaryCollection<T> : ICollection<T> where T : INamedObject
    {
        #region Constructor(s)

        public DictionaryCollection()
            : this(100)
        {
        }

        public DictionaryCollection(int initialSize)
        {
            m_Collection = new Dictionary<string, T>(initialSize);
        }

        #endregion

        #region Fields

        private Dictionary<string, T> m_Collection;

        #endregion

        #region Properties

        /// <summary>
        ///     <para>Retrieves an element from the collection at the specified index.</para>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get { return m_Collection[m_Collection.Keys.ElementAt(index)]; }
        }

        /// <summary>
        ///     <para>Retrieves an element from the collection that is of the specified name.</para>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public T this[string name]
        {
            get { return (m_Collection.ContainsKey(name) && String.IsNullOrEmpty(name) ? m_Collection[name] : default(T)); }
        }

        /// <summary>
        ///     <para>Gets the number of items contained in the DictionaryCollection&lt;T&gt;</para>
        /// </summary>
        public int Count
        {
            get { return m_Collection.Count; }
        }

        /// <summary>
        ///     <para>Gets a value indicating whether the collection is read-only.</para>
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     <para>Adds the specified item to the collection.</para>
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            if (!Contains(item))
            {
                m_Collection.Add(item.Name, item);
            }
        }

        /// <summary>
        ///     <para>Clears the collection of all elements.</para>
        /// </summary>
        public void Clear()
        {
            m_Collection.Clear();
        }

        /// <summary>
        ///     <para>Returns a value whether the specified item is in the collection.</para>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return m_Collection.ContainsKey(item.Name);
        }

        /// <summary>
        ///     <para>Returns a value whether an element exists with the specified name.</para>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contiains(string item)
        {
            return m_Collection.ContainsKey(item);
        }

        /// <summary>
        ///     <para>Copies the items to the specified array.</para>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            m_Collection.Values.CopyTo(array, arrayIndex);
        }

        /// <summary>
        ///     <para>Removes the specified item from the collection.</para>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            return (Contains(item) && m_Collection.Remove(item.Name));
        }

        /// <summary>
        ///     <para>Returns an enumerator that iterates through the DictionaryCollection.</para>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return m_Collection.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_Collection.Values.GetEnumerator();
        }

        #endregion
    }
}
