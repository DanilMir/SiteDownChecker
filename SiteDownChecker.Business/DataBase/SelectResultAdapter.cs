using System.Collections.Generic;
using System.Linq;
using SiteDownChecker.DataAccess;

namespace SiteDownChecker.Business.DataBase
{
    internal readonly struct SelectResultAdapter
    {
        private readonly SelectResult _result;

        public SelectResultAdapter(SelectResult result) => _result = result;

        public override string ToString() =>
            _result.Names.Aggregate((s, t) => $"{s}\t{t}")
            + '\n'
            + _result.Aggregate(string.Empty, (current, nextObject) =>
                $"{current}{nextObject.Names.Aggregate((curr, next) => $"{curr}\t{nextObject[next]}")}\n");

        public T Deserialize<T>(int index) where T : new()
        {
            var item = new T();
            foreach (var name in _result.Names)
                typeof(T).GetProperty(name)?.SetValue(item, _result[index, name]);
            return item;
        }

        public List<T> DeserializeAll<T>() where T : new()
        {
            var list = new List<T>(_result.Count);
            for (var i = 0; i < _result.Count; ++i)
                list.Add(Deserialize<T>(i));
            return list;
        }
    }
}
