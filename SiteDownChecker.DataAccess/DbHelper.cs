using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteDownChecker.DataAccess
{
    public static class DbHelper
    {
        #region select

        public static SelectResult SelectWithFilter(string tableName, params SqlValuePair[] filters) =>
            SelectWithFilter(tableName, (IReadOnlyCollection<SqlValuePair>) filters);
        
        public static SelectResult SelectWithFilter(string tableName, IReadOnlyCollection<SqlValuePair> filters) =>
            DbRequestDealer.SelectRequest(CreateSelectRequest(tableName, filters));

        public static async Task<SelectResult> SelectWithFilterAsync(
            string tableName,
            params SqlValuePair[] filters)
            =>
                await SelectWithFilterAsync(tableName, (IReadOnlyCollection<SqlValuePair>) filters);

        public static async Task<SelectResult> SelectWithFilterAsync(
            string tableName,
            IReadOnlyCollection<SqlValuePair> filters)
            =>
                await DbRequestDealer.SelectRequestAsync(CreateSelectRequest(tableName, filters));

        private static string CreateSelectRequest(
            string tableName,
            IReadOnlyCollection<SqlValuePair> filters)
            =>
                $"SELECT * FROM {tableName}" +
                (filters.Count is not 0
                    ? $" WHERE {filters.Skip(1).Aggregate($"{filters.First()}", (s, pair) => $"{s} AND {pair}")}"
                    : string.Empty);

        #endregion

        #region delete

        public static bool TryDeleteById(string tableName, Guid id) =>
            DbRequestDealer.NonQueryRequest(CreateDeleteRequest(tableName, id)) > 0;

        public static async Task<bool> TryDeleteByIdAsync(string tableName, Guid id) =>
            await DbRequestDealer.NonQueryRequestAsync(CreateDeleteRequest(tableName, id)) > 0;

        private static string CreateDeleteRequest(string tableName, Guid id) =>
            $"DELETE FROM {tableName} where Id = {id.ToSqlString()}";

        #endregion

        #region update

        public static bool TryUpdateById(string tableName, Guid id, IReadOnlyList<SqlValuePair> values) =>
            DbRequestDealer.NonQueryRequest(CreateUpdateRequest(tableName, id, values)) is not 0;

        public static async Task<bool> TryUpdateByIdAsync(string tableName, Guid id, IReadOnlyList<SqlValuePair> values)
            =>
                await DbRequestDealer.NonQueryRequestAsync(CreateUpdateRequest(tableName, id, values)) is not 0;

        private static string CreateUpdateRequest(string tableName, Guid id, IReadOnlyList<SqlValuePair> values) =>
            $"UPDATE {tableName} SET "
            + values.Skip(1).Aggregate($"{values[0]}", (s, pair) => $"{s}, {pair}") +
            $" WHERE Id = {id.ToSqlString()}";

        #endregion

        #region insert

        public static bool TryInsert(string tableName, IReadOnlyList<SqlValuePair> values) =>
            DbRequestDealer.NonQueryRequest(CreateInsertRequest(tableName, values)) is not 0;

        public static async Task<bool> TryInsertAsync(string tableName, IReadOnlyList<SqlValuePair> values) =>
            await DbRequestDealer.NonQueryRequestAsync(CreateInsertRequest(tableName, values)) is not 0;

        private static string CreateInsertRequest(string tableName, IReadOnlyList<SqlValuePair> values) =>
            $"INSERT INTO {tableName} ("
            + values.Skip(1).Aggregate(values[0].Name, (s, pair) => $"{s}, {pair.Name}")
            + ") VALUES ("
            + values.Skip(1).Aggregate(values[0].ValueString, (s, pair) => $"{s}, {pair.ValueString}")
            + ")";

        #endregion
    }
}
