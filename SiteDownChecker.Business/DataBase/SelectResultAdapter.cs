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
            _result.Header.Aggregate((current, next) => current + $"{next}\t") + '\n' +
            _result.Table
                .Aggregate(
                    string.Empty,
                    (current, list) =>
                        current + list.Aggregate(string.Empty, (s, o) => s + $"{o} ") + '\n');

        public T Deserialize<T>(int index) where T : new()
        {
            if (index >= _result.RowsCount)
                return default;
            var item = new T();
            for (var i = 0; i < _result.ColumnsCount; ++i)
                typeof(T).GetProperty(_result.Header[i])?.SetValue(item, _result.Table[index][i]);

            return item;
        }

        public List<T> DeserializeAll<T>() where T : new()
        {
            var list = new List<T>(_result.RowsCount);
            for (var i = 0; i < _result.RowsCount; ++i)
                list.Add(Deserialize<T>(i));
            return list;
        }
    }
}
