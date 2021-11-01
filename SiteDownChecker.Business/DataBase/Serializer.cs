using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SiteDownChecker.DataAccess;

// ReSharper disable StaticMemberInGenericType
// ReSharper disable CommentTypo

namespace SiteDownChecker.Business.DataBase
{
    /// <summary>
    /// класс умеет сериализировать все публичные свойства любых типов в базу данных, и десериализировать их из базы,
    /// при условиях:
    /// 1) существует таблица имя которой = имя типа + 's',
    /// причем множества публичных свойств типа и названий столбцов в таблице базы данных совпадают
    /// 2) у типа существует публичное свойство Id типа Guid
    ///
    /// использует рефлексию в каждом методе
    /// </summary>
    /// <typeparam name="TBusiness"></typeparam>
    public static class Serializer<TBusiness>
        where TBusiness : new()
    {
        public static TBusiness DeserializeFromId(Guid id) =>
            new SelectResultAdapter(DbHelper.SelectWithFilter(tableName, new SqlValuePair("Id", id)))
                .Deserialize<TBusiness>(0);

        public static List<TBusiness> DeserializeAll() =>
            new SelectResultAdapter(DbHelper.SelectWithFilter(tableName)).DeserializeAll<TBusiness>();

        private static IReadOnlyCollection<string> generalPropertyNames;
        private static readonly Type type = typeof(TBusiness);
        private static readonly string tableName = type.ToSqlTableName();

        /// <summary>
        /// метод создает или обновляет соответствующий обьект в базе данных
        /// 
        /// дополнительно меняет свойство Id экземпляра, если оно равно Guid.Empty
        /// 
        /// при вызове с одним параметром, сам выберет обновлять или содавать
        /// иначе выкинет исключение, если выбранный тип операции невозможен
        /// </summary>
        /// <param name="item">обьект сериализации</param>
        /// <param name="onlyUpdate">если нужно только обновить</param>
        /// <param name="onlyInsert">если нужно только вставить</param>
        public static bool TrySerialize(TBusiness item, bool onlyUpdate = default, bool onlyInsert = default)
        {
            generalPropertyNames ??= CreateGeneralPropertyNames();
            var id = ProcessId(item);
            return DbHelper.SelectWithFilter(tableName, new SqlValuePair("Id", id)).Count is 0
                ? onlyUpdate
                    ? throw new Exception($"cant find object with id = '{id}'")
                    : DbHelper.TryInsert(tableName, CreatePairs(item))
                : onlyInsert
                    ? throw new Exception($"object with id = '{id}' already exists")
                    : DbHelper.TryUpdateById(tableName, id, CreatePairs(item));
        }

        public static async Task<bool> TrySerializeAsync(TBusiness item, bool onlyUpdate = default,
            bool onlyInsert = default)
        {
            var gpnCreatingTask = generalPropertyNames is null ? CreateGeneralPropertyNamesAsync() : null;
            var id = ProcessId(item);
            return (await DbHelper.SelectWithFilterAsync(tableName, new SqlValuePair("Id", id))).Count is 0
                ? onlyUpdate
                    ? throw new Exception($"cant find object with id = '{id}'")
                    : await DbHelper.TryInsertAsync(tableName, await CreatePairsAsync(item, gpnCreatingTask))
                : onlyInsert
                    ? throw new Exception($"object with id = '{id}' already exists")
                    : await DbHelper.TryUpdateByIdAsync(tableName, id, await CreatePairsAsync(item, gpnCreatingTask));
        }

        private static Guid ProcessId(TBusiness item)
        {
            var id = item.GetValue<Guid>("Id", type);
            if (id == Guid.Empty)
                item.SetValue("Id", id = Guid.NewGuid(), type);
            return id;
        }

        //TODO ToArray
        private static SqlValuePair[] CreatePairs(TBusiness item) => generalPropertyNames.Select(name =>
            new SqlValuePair(name, type.GetProperty(name)?.GetValue(item))).ToArray();

        private static async Task<SqlValuePair[]> CreatePairsAsync(
            TBusiness item,
            Task<IReadOnlyCollection<string>> gpnCreatingTask)
            =>
                (generalPropertyNames ??= await gpnCreatingTask).Select(name =>
                    new SqlValuePair(name, type.GetProperty(name)?.GetValue(item))).ToArray();

        #region generalPropertyNames

        private static readonly string generalPropertyNamesSelectRequest =
            $"SELECT COLUMN_NAME FROM {DbRequestDealer.Catalog}.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'{tableName}'";

        private static IReadOnlyCollection<string> CreateGeneralPropertyNames() =>
            ProcessGeneralPropertyNames(DbRequestDealer.SelectRequest(generalPropertyNamesSelectRequest));

        private static async Task<IReadOnlyCollection<string>> CreateGeneralPropertyNamesAsync() =>
            ProcessGeneralPropertyNames(await DbRequestDealer.SelectRequestAsync(generalPropertyNamesSelectRequest));

        private static IReadOnlyCollection<string> ProcessGeneralPropertyNames(SelectResult selectResult) =>
            selectResult
                .Select(resultObject => resultObject["COLUMN_NAME"] as string)
                .Where(name => type.GetProperties().Select(property => property.Name).Contains(name)).ToArray();

        #endregion

        /// <summary>
        /// вызывает TrySerialize с указанием, что нужно только обновить
        /// </summary>
        /// <param name="item"></param>
        public static bool TryUpdate(TBusiness item) => TrySerialize(item, true);

        public static async Task<bool> TryUpdateAsync(TBusiness item) => await TrySerializeAsync(item, true);

        /// <summary>
        /// вызывает TrySerialize с указанием, что нужно только вставить
        /// </summary>
        /// <param name="item"></param>
        public static bool TryInsert(TBusiness item) => TrySerialize(item, false, true);

        public static async Task<bool> TryInsertAsync(TBusiness item) => await TrySerializeAsync(item, false, true);


        public static List<TBusiness> DeserializeWithFilter(TBusiness filter) =>
            new SelectResultAdapter(
                    DbHelper.SelectWithFilter(type.ToSqlTableName(),
                        (IReadOnlyCollection<SqlValuePair>) CreateDeserializePairs(filter)))
                .DeserializeAll<TBusiness>();
        
        public static async Task<List<TBusiness>> DeserializeWithFilterAsync(TBusiness filter) =>
            new SelectResultAdapter(
                    await DbHelper.SelectWithFilterAsync(type.ToSqlTableName(),
                        (IReadOnlyCollection<SqlValuePair>) CreateDeserializePairs(filter)))
                .DeserializeAll<TBusiness>();

        private static PropertyInfo[] properties;

        private static IEnumerable<SqlValuePair> CreateDeserializePairs(TBusiness filter) =>
            (properties ??= type.GetProperties())
            .Select(property => new SqlValuePair(property.Name, property.GetValue(filter)))
            .Where(pair => pair.Value is not null);
    }
}
