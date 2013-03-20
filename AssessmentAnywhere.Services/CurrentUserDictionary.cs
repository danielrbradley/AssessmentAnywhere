namespace AssessmentAnywhere.Services
{
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class CurrentUserDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly ConcurrentDictionary<string, Dictionary<TKey, TValue>> userDictionaries = new ConcurrentDictionary<string, Dictionary<TKey, TValue>>();

        private string CurrentUsername
        {
            get
            {
                return System.Threading.Thread.CurrentPrincipal.Identity.Name;
            }
        }

        private Dictionary<TKey, TValue> CurrentUsersDictionary
        {
            get
            {
                return this.userDictionaries.GetOrAdd(this.CurrentUsername, new Dictionary<TKey, TValue>());
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.CurrentUsersDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.CurrentUsersDictionary.GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)this.CurrentUsersDictionary).Add(item);
        }

        public void Clear()
        {
            this.CurrentUsersDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)this.CurrentUsersDictionary).Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)this.CurrentUsersDictionary).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)this.CurrentUsersDictionary).Remove(item);
        }

        public int Count
        {
            get
            {
                return this.CurrentUsersDictionary.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>)this.CurrentUsersDictionary).IsReadOnly;
            }
        }

        public bool ContainsKey(TKey key)
        {
            return this.CurrentUsersDictionary.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            this.CurrentUsersDictionary.Add(key, value);
        }

        public bool Remove(TKey key)
        {
            return this.CurrentUsersDictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.CurrentUsersDictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get
            {
                return this.CurrentUsersDictionary[key];
            }

            set
            {
                this.CurrentUsersDictionary[key] = value;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return this.CurrentUsersDictionary.Keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return this.CurrentUsersDictionary.Values;
            }
        }
    }
}
