using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DBSerializer
{
    public class SelectResultAdapter
    {
        private SelectResult result;

        public SelectResultAdapter(SelectResult result) =>
            this.result = result;

        public string ToBigString()
        {
            var builder = new StringBuilder();
            foreach (var name in result.Header)
                builder.Append($"{name}\t");
            builder.Append('\n');
            foreach (var item in result.Table)
            {
                foreach (var info in item)
                    builder.Append($"{info} ");
                builder.Append('\n');
            }

            return builder.ToString();
        }

        public T Deserialize<T>(int index) where T : new()
        {
            if (index >= result.RowsCount)
                return default;
            var item = new T();
            for (var i = 0; i < result.ColumnsCount; ++i)
                try
                {
                    typeof(T).GetProperty(result.Header[i])?.SetValue(item, result.Table[index][i]);
                }
                catch (Exception e)
                {
                    // ignored
                }

            return item;
        }

        public List<T> DeserializeAll<T>() where T : new()
        {
            var list = new List<T>(result.RowsCount);
            for (var i = 0; i < result.RowsCount; ++i)
                list.Add(Deserialize<T>(i));
            return list;
        }
    }
}