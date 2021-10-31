using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SiteDownChecker.DataAccess;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CommentTypo
// ReSharper disable StaticMemberInGenericType
// ReSharper disable StringLiteralTypo
// ReSharper disable PossibleNullReferenceException

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
            new SelectResultAdapter(OldDbHelper.SelectRequest($"select * from {type.Name}s where Id = '{id}'"))
                .Deserialize<TBusiness>(0);

        public static List<TBusiness> DeserializeAll() =>
            new SelectResultAdapter(OldDbHelper.SelectRequest($"select * from {type.Name}s"))
                .DeserializeAll<TBusiness>();

        private static string[] generalPropertyNames;
        private static readonly Type type = typeof(TBusiness);
        private static readonly PropertyInfo idProperty = type.GetProperty("Id");
        private static readonly string tableName = $"{type.Name}s";

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
        public static void Serialize(TBusiness item, bool onlyUpdate = default, bool onlyInsert = default)
        {
            //пока мой код никто не проверяет, мне дозволено так делать

            if (idProperty.GetValue(item) as Guid? == Guid.Empty)
                idProperty.SetValue(item, Guid.NewGuid());

            generalPropertyNames ??= OldDbHelper.SelectRequest(
                    $"SELECT COLUMN_NAME FROM {OldDbHelper.Catalog}.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'{tableName}'")
                .Table.Select(line => line[0] as string)
                .Where(name => type.GetProperties().Select(property => property.Name).Contains(name)).ToArray();

            var values = generalPropertyNames.Select(x => type.GetProperty(x).GetValue(item)).ToArray();
            var id = type.GetProperty("Id").GetValue(item);

            _ = OldDbHelper.SelectRequest($"select * from {tableName} where Id = '{id}'").RowsCount == 0
                ? onlyUpdate
                    ? throw new Exception($"cant find object with id = '{id}'")
                    : OldDbHelper.InsertRequest(tableName, generalPropertyNames, values)
                : onlyInsert
                    ? throw new Exception($"object with id = '{id}' already exists")
                    : OldDbHelper.UpdateByIdRequest(tableName, (Guid) type.GetProperty("Id").GetValue(item),
                        generalPropertyNames, values);
        }

        /// <summary>
        /// вызывает Serialize с указанием, что нужно только обновить
        /// </summary>
        /// <param name="item"></param>
        public static void Update(TBusiness item) => Serialize(item, true);

        /// <summary>
        /// вызывает Serialize с указанием, что нужно только вставить
        /// </summary>
        /// <param name="item"></param>
        public static void Insert(TBusiness item) => Serialize(item, false, true);

        public static void DeleteById(Guid id) =>
            OldDbHelper.NonQueryRequest($"delete from {tableName} where Id = '{id}'");

        private static PropertyInfo[] properties;

        public static List<TBusiness> DeserializeWithFilter(TBusiness filter)
        {
            properties ??= type.GetProperties();

            var whereBuilder = new StringBuilder();
            var thereIsAFilter = false;
            foreach (var property in properties)
            {
                var value = property.GetValue(filter);
                switch (property.Name, value)
                {
                    case ("Id", _):
                        if (value as Guid? != Guid.Empty)
                        {
                            thereIsAFilter = true;
                            whereBuilder.Append($"Id = {value.ToSqlString()} AND ");
                        }

                        break;
                    case (_, not null):
                        thereIsAFilter = true;
                        whereBuilder.Append($"{property.Name} = {value.ToSqlString()} AND ");
                        break;
                }
            }
            
            return thereIsAFilter
                ? new SelectResultAdapter(
                        OldDbHelper.SelectRequest(
                            $"SELECT * FROM {tableName} WHERE {whereBuilder.Remove(whereBuilder.Length - 5, 5)}"))
                    .DeserializeAll<TBusiness>()
                : DeserializeAll();
        }
    }
}
