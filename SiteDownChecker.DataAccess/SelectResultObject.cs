using System.Collections.Generic;

namespace SiteDownChecker.DataAccess
{
    public readonly struct SelectResultObject
    {
        private readonly int _index;
        private readonly Dictionary<string, List<object>> _dictionary;

        public SelectResultObject(int index, Dictionary<string, List<object>> dictionary)
        {
            _index = index;
            _dictionary = dictionary;
        }

        public object this[string name] => _dictionary[name][_index];
    }
}
