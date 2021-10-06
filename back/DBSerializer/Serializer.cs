using System;
using System.Linq;

// ReSharper disable StaticMemberInGenericType
// ReSharper disable StringLiteralTypo

namespace DBSerializer
{
    public static class Serializer<T> where T : new()
    {
        public static T DeserializeFromId(int id) =>
            new SelectResultAdapter(DbHelper.SelectRequest($"select * from {type.Name}s where Id = {id}"))
                .Deserialize<T>(0);

        private static string[] generalPropertyNames;
        private static readonly Type type = typeof(T);
        private static readonly string tableName = $"{type.Name}s";

        private static string[] GenerateGeneralPropertyNames()
        {
            var objPropertyNames =
                type.GetProperties().Select(property => property.Name).Where(name => name != "Id").ToArray();
            var selectResult =
                DbHelper.SelectRequest(
                    $"SELECT COLUMN_NAME FROM usersdb.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'{tableName}'");
            var dbColumnNames = selectResult.Table.Select(line => (string) line[0]).ToArray();
            var result = dbColumnNames.Where(name => objPropertyNames.Contains(name)).ToArray();

            return result;
        }

        public static void Serialize(T item)
        {
            generalPropertyNames ??= GenerateGeneralPropertyNames();

            var values = new object[generalPropertyNames.Length];
            for (var i = 0; i < values.Length; ++i)
                values[i] = type.GetProperty(generalPropertyNames[i])?.GetValue(item);
            _ = DbHelper.SelectRequest(
                $"select * from {tableName} where Id = {type.GetProperty("Id")?.GetValue(item)}").RowsCount == 0
                ? DbHelper.InsertRequest(tableName, generalPropertyNames, values)
                : DbHelper.UpdateByIdRequest(tableName, (int) type.GetProperty("Id")?.GetValue(item),
                    generalPropertyNames, values);
        }
    }
}
