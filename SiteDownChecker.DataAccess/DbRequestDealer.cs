using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SiteDownChecker.DataAccess
{
    public static class DbRequestDealer
    {
        public const string Catalog = "sitedownchecker";

        private static readonly string connectionString =
            $"Data Source=localhost;Initial Catalog={Catalog};Integrated Security=True";

        private static readonly SqlConnection baseConnection = new(connectionString);

        public static SelectResult SelectRequest(string request)
        {
            using var connection = baseConnection.Clone();
            connection.Open();
            var reader = new SqlCommand(request, connection).ExecuteReader();

            var res = new SelectResult(CreateNamesList(reader));

            var objects = new object[reader.FieldCount];
            while (reader.Read())
            {
                for (var i = 0; i < reader.FieldCount; ++i)
                    objects[i] = reader.GetValue(i);
                res.Add(objects);
            }

            return res;
        }

        public static async Task<SelectResult> SelectRequestAsync(string request)
        {
            await using var connection = baseConnection.Clone();
            connection.Open();
            var reader = await new SqlCommand(request, connection).ExecuteReaderAsync();

            var res = new SelectResult(CreateNamesList(reader));

            var objects = new object[reader.FieldCount];
            while (await reader.ReadAsync())
            {
                for (var i = 0; i < reader.FieldCount; ++i)
                    objects[i] = reader.GetValue(i);
                res.Add(objects);
            }

            return res;
        }

        private static IList<string> CreateNamesList(IDataRecord reader)
        {
            var names = new List<string>(reader.FieldCount);
            for (var i = 0; i < reader.FieldCount; ++i)
                names.Add(reader.GetName(i));
            return names;
        }

        public static int NonQueryRequest(string request)
        {
            using var connection = baseConnection.Clone();
            connection.Open();
            return new SqlCommand(request, connection).ExecuteNonQuery();
        }

        public static async Task<int> NonQueryRequestAsync(string request)
        {
            await using var connection = baseConnection.Clone();
            connection.Open();
            return await new SqlCommand(request, connection).ExecuteNonQueryAsync();
        }
    }
}
