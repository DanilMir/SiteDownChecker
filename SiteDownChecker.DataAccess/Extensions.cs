// ReSharper disable InconsistentNaming
namespace MedicalVideo.DataAccess
{
    public static class Extensions
    {
        public static string ToSQLString(this object o) => 
            IsNumber(o) ? $"{o}" : $"'{o}'";

        private static bool IsNumber(object obj) =>
            obj is int or long or short or nint 
                or uint or ulong or ushort or nuint 
                or double or float or byte or sbyte or decimal;
    }
}