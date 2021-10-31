using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SiteDownChecker.DataAccess
{
    public static class DbHelper
    {
        public const string Catalog = "sitedownchecker";
        private static readonly string connectionString =
            $"Data Source=localhost;Initial Catalog={Catalog};Integrated Security=True";

        private static readonly SqlConnection baseConnection = new(connectionString);
        private static SqlConnection connection;
        
        public static SelectResult SelectRequest(string request)
        {
            using (connection = baseConnection.Clone())
            {
                connection.Open();
                var command = new SqlCommand(request, connection);

                var reader = command.ExecuteReader();
                var table = new List<List<object>>();
                while (reader.Read())
                {
                    var newList = new List<object>();
                    table.Add(newList);
                    for (var i = 0; i < reader.FieldCount; ++i)
                        newList.Add(reader.GetValue(i));
                }

                var names = new string[reader.FieldCount];
                for (var i = 0; i < reader.FieldCount; ++i)
                    names[i] = reader.GetName(i);

                return new SelectResult
                {
                    Header = names,
                    Table = table
                };
            }
        }

        public static int NonQueryRequest(string request)
        {
            using (connection = baseConnection.Clone())
            {
                connection.Open();
                var command = new SqlCommand(request, connection);
                var count = command.ExecuteNonQuery();
                //Console.WriteLine($"Changed {count} values");
                return count;
            }
        }

        public static int UpdateByIdRequest(string tableName, Guid id, string[] columnNames, object[] values)
        {
            if (columnNames.Length != values.Length || columnNames.Length == 0)
                throw new Exception("wrong names and values length");
            var requestStringBuilder = new StringBuilder();
            requestStringBuilder.Append($"UPDATE {tableName} SET ");
            requestStringBuilder.Append(
                $"{columnNames[0]} = {values[0].ToSqlString()}");
            for (var i = 1; i < columnNames.Length; ++i)
                requestStringBuilder.Append(
                    $", {columnNames[i]} = {values[i].ToSqlString()}");
            requestStringBuilder.Append($" Where Id = '{id}'");
            return NonQueryRequest(requestStringBuilder.ToString());
        }

        public static int InsertRequest(string tableName, string[] columnNames, object[] values)
        {
            if (columnNames.Length != values.Length || columnNames.Length == 0)
                throw new Exception("wrong names and values length");
            var requestStringBuilder = new StringBuilder();
            requestStringBuilder.Append($"INSERT INTO {tableName} (");
            requestStringBuilder.Append($"{columnNames[0]}");
            for (var i = 1; i < columnNames.Length; ++i) 
                requestStringBuilder.Append($", {columnNames[i]}");
            requestStringBuilder.Append(") VALUES (");
            requestStringBuilder.Append(values[0].ToSqlString());
            for (var i = 1; i < values.Length; ++i)
                requestStringBuilder.Append($", {values[i].ToSqlString()}");
            requestStringBuilder.Append(')');
            return NonQueryRequest(requestStringBuilder.ToString());
        }
    }
}
