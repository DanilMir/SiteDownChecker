namespace SiteDownChecker.Business.Models;

public interface IConvertibleFromBusinessModel<out TApi, in TBusiness>
{
    /// <summary>
    /// должен менять текущий экземпляр, и возвращать его же
    /// </summary>
    /// <param name="businessItem"></param>
    /// <returns></returns>
    public TApi FromBusinessModel(TBusiness businessItem) => (TApi) (object) businessItem;
}
