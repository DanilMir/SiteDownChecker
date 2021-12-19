namespace SiteDownChecker.Business.Models;

public interface IConvertibleToBusinessModel<out T>
{
    /// <summary>
    /// возвращает экземпляр соответствующего типа из Business.Models
    /// </summary>
    /// <returns></returns>
    public T ToBusiness() => (T) this; //от такой реализации воняет
}
