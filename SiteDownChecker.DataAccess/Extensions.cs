using System;
using System.Data.SqlClient;

namespace SiteDownChecker.DataAccess
{
    public static class Extensions
    {
        public static string ToSqlString(this object o) =>
            IsNumber(o) ? $"{o}" : $"'{o}'";

        private static bool IsNumber(object obj) =>
            obj is int or long or short or nint
                or uint or ulong or ushort or nuint
                or double or float or byte or sbyte or decimal;

        public static SqlConnection Clone(this SqlConnection connection) =>
            (SqlConnection) ((ICloneable) connection).Clone();
    }
}
