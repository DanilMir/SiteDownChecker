using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DBSerializer
{
    public struct SelectResult
    {
        public string[] Header { get; set; }
        public List<List<object>> Table { get; set; }

        public int ColumnsCount => Header.Length;
        public int RowsCount => Table.Count;

        public SelectResult(string[] header, List<List<object>> table)
        {
            Header = header;
            Table = table;
        }
    }

    public static class DbHelper
    {
        //TODO ref to online server
        private const string ConnectionString =
            @"Data Source=localhost;Initial Catalog=usersdb;Integrated Security=True";
        
        private static SqlConnection connection;
        
        public static SelectResult SelectRequest(string request)
        {
            using (connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(request, connection);

                var reader = command.ExecuteReader();
                var table = new List<List<object>>();
                var count = reader.FieldCount;
                while (reader.Read())
                {
                    var newList = new List<object>();
                    table.Add(newList);
                    for (var i = 0; i < count; ++i)
                        newList.Add(reader.GetValue(i));
                }

                var names = new string[count];
                for (var i = 0; i < count; ++i)
                    names[i] = reader.GetName(i);

                return new SelectResult(names, table);
            }
        }

        public static int NonQueryRequest(string request)
        {
            using (connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(request, connection);
                var count = command.ExecuteNonQuery();
                Console.WriteLine($"Changed {count} values");
                return count;
            }
        }

        public static int UpdateByIdRequest(string tableName, int id, string[] columnNames, object[] values)
        {
            if (columnNames.Length != values.Length || columnNames.Length == 0)
                throw new Exception("wrong names and values length");
            var requestStringBuilder = new StringBuilder();
            requestStringBuilder.Append($"UPDATE {tableName} SET ");
            requestStringBuilder.Append(
                $"{columnNames[0]} = {(values[0] is int ? values[0].ToString() : $"'{values[0]}'")}");
            for (var i = 1; i < columnNames.Length; ++i)
                requestStringBuilder.Append(
                    $", {columnNames[i]} = {(values[i] is int ? values[i].ToString() : $"'{values[i]}'")}");
            requestStringBuilder.Append($" Where Id = {id}");
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
            requestStringBuilder.Append(values[0] is int ? values[0].ToString() : $"'{values[0]}'");
            for (var i = 1; i < values.Length; ++i)
                requestStringBuilder.Append(values[i] is int ? values[i].ToString() : $", '{values[i]}'");
            requestStringBuilder.Append(')');
            return NonQueryRequest(requestStringBuilder.ToString());
        }
    }
}
