using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SiteDownChecker.DataAccess
{
    public readonly partial struct SelectResult
    {
        private readonly Dictionary<string, List<object>> _dictionary;
        public object this[int index, string name] => _dictionary[name][index];
        public SelectResultObject this[int index] => new(index, _dictionary, Names);
        public ReadOnlyCollection<string> Names { get; }
        public int Count => _dictionary[Names[0]].Count;

        public SelectResult(ReadOnlyCollection<string> names)
        {
            if (names.Count is 0)
                throw new Exception("names count can't be 0");
            Names = names;
            _dictionary = new Dictionary<string, List<object>>();
            foreach (var name in Names)
                _dictionary[name] = new List<object>();
        }

        public SelectResult(IList<string> names) : this(new ReadOnlyCollection<string>(names)) { }

        public void Add(IEnumerable<object> objects)
        {
            using var enumerator = objects.GetEnumerator();
            foreach (var name in Names)
            {
                enumerator.MoveNext();
                _dictionary[name].Add(enumerator.Current);
            }
        }
    }
}
