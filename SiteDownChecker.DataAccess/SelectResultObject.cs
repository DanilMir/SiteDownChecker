using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SiteDownChecker.DataAccess
{
    public readonly struct SelectResultObject
    {
        private readonly int _index;
        private readonly Dictionary<string, List<object>> _dictionary;
        public readonly IReadOnlyCollection<string> Names { get; }

        public SelectResultObject(int index, Dictionary<string, List<object>> dictionary, IReadOnlyCollection<string> names)
        {
            _index = index;
            _dictionary = dictionary;
            Names = names;
        }

        public object this[string name] => _dictionary[name][_index];
    }
}
