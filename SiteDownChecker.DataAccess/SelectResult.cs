using System.Collections.Generic;

namespace SiteDownChecker.DataAccess
{
    public readonly struct SelectResult
    {
        public string[] Header { get; init; }
        public List<List<object>> Table { get; init; }

        public int ColumnsCount => Header.Length;
        public int RowsCount => Table.Count;
    }
}
