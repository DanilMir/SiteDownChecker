using System;

namespace SiteDownChecker.Business;

internal static class Extensions
{
    /// <summary>
    /// работает чуть быстрее, если указать тип
    /// </summary>
    /// <param name="item"></param>
    /// <param name="propertyName"></param>
    /// <param name="type"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetValue<T>(this object item, string propertyName, Type type = null) =>
        (T) (type ?? item.GetType()).GetProperty(propertyName)?.GetValue(item);

    /// <summary>
    /// работает чуть быстрее, если указать тип
    /// </summary>
    /// <param name="item"></param>
    /// <param name="propertyName"></param>
    /// <param name="value"></param>
    /// <param name="type"></param>
    public static void SetValue(this object item, string propertyName, object value, Type type = null) =>
        (type ?? item.GetType()).GetProperty(propertyName)?.SetValue(item, value);
}
