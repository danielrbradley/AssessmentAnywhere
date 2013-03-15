namespace AssessmentAnywhere.Services.Repos
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
                return this.userDictionaries.GetOrAdd(CurrentUsername, new Dictionary<TKey, TValue>());
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return CurrentUsersDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return CurrentUsersDictionary.GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)CurrentUsersDictionary).Add(item);
        }

        public void Clear()
        {
            CurrentUsersDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)CurrentUsersDictionary).Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)CurrentUsersDictionary).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)CurrentUsersDictionary).Remove(item);
        }

        public int Count
        {
            get
            {
                return CurrentUsersDictionary.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>)CurrentUsersDictionary).IsReadOnly;
            }
        }

        public bool ContainsKey(TKey key)
        {
            return CurrentUsersDictionary.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            CurrentUsersDictionary.Add(key, value);
        }

        public bool Remove(TKey key)
        {
            return CurrentUsersDictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return CurrentUsersDictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get
            {
                return CurrentUsersDictionary[key];
            }

            set
            {
                CurrentUsersDictionary[key] = value;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return CurrentUsersDictionary.Keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return CurrentUsersDictionary.Values;
            }
        }
    }
}
