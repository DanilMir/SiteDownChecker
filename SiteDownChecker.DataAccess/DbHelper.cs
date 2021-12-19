using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteDownChecker.DataAccess;

public static class DbHelper
{
    #region select

    public static SelectResult SelectWithFilter(string tableName, params SqlValuePair[] filters) =>
        SelectWithFilter(tableName, (IReadOnlyCollection<SqlValuePair>) filters);

    public static SelectResult SelectWithFilter(string tableName, IEnumerable<SqlValuePair> filters) =>
        DbRequestDealer.SelectRequest(CreateSelectRequest(tableName, filters));

    public static async Task<SelectResult> SelectWithFilterAsync(
        string tableName,
        params SqlValuePair[] filters)
        =>
            await SelectWithFilterAsync(tableName, (IEnumerable<SqlValuePair>) filters);

    public static async Task<SelectResult> SelectWithFilterAsync(
        string tableName,
        IEnumerable<SqlValuePair> filters)
        =>
            await DbRequestDealer.SelectRequestAsync(CreateSelectRequest(tableName, filters));

    private static string CreateSelectRequest(
        string tableName,
        IEnumerable<SqlValuePair> filters)
        =>
            $"SELECT * FROM {tableName}" +
            (filters.Any()
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

    public static bool TryUpdateById(string tableName, Guid id, IEnumerable<SqlValuePair> values) =>
        DbRequestDealer.NonQueryRequest(CreateUpdateRequest(tableName, id, values)) is not 0;

    public static async Task<bool> TryUpdateByIdAsync(string tableName, Guid id, IEnumerable<SqlValuePair> values)
        =>
            await DbRequestDealer.NonQueryRequestAsync(CreateUpdateRequest(tableName, id, values)) is not 0;

    private static string CreateUpdateRequest(string tableName, Guid id, IEnumerable<SqlValuePair> values) =>
        $"UPDATE {tableName} SET "
        + values.Skip(1).Aggregate($"{values.First()}", (s, pair) => $"{s}, {pair}") +
        $" WHERE Id = {id.ToSqlString()}";

    #endregion

    #region insert

    public static bool TryInsert(string tableName, IEnumerable<SqlValuePair> values) =>
        DbRequestDealer.NonQueryRequest(CreateInsertRequest(tableName, values)) is not 0;

    public static async Task<bool> TryInsertAsync(string tableName, IEnumerable<SqlValuePair> values) =>
        await DbRequestDealer.NonQueryRequestAsync(CreateInsertRequest(tableName, values)) is not 0;

    private static string CreateInsertRequest(string tableName, IEnumerable<SqlValuePair> values) =>
        $"INSERT INTO {tableName} ("
        + values.Skip(1).Aggregate(values.First().Name, (s, pair) => $"{s}, {pair.Name}")
        + ") VALUES ("
        + values.Skip(1).Aggregate(values.First().ValueString, (s, pair) => $"{s}, {pair.ValueString}")
        + ")";

    #endregion
}
