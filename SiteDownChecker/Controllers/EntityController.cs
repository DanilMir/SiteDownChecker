using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SiteDownChecker.Business.DataBase;
using SiteDownChecker.Business.Models;
using SiteDownChecker.DataAccess;

namespace SiteDownChecker.Controllers
{
    public class EntityController<TApi, TBusiness> : ControllerBase
        where TBusiness : new()
        where TApi : IConvertibleFromBusinessModel<TApi, TBusiness>, IConvertibleToBusinessModel<TBusiness>, new()
    {
        private static TApi uselessInstance = new();

        [HttpGet("{id:Guid}")]
        public virtual async Task<TApi> GetByIdAsync(Guid id) =>
            uselessInstance.FromBusinessModel(await Serializer<TBusiness>.DeserializeFromIdAsync(id));

        public virtual TApi GetById(Guid id) =>
            uselessInstance.FromBusinessModel(Serializer<TBusiness>.DeserializeFromId(id));

        [HttpPost]
        public virtual async Task<bool> TryCreateAsync([FromBody] TApi item) =>
            await Serializer<TBusiness>.TryInsertAsync(item.ToBusiness());

        public virtual bool TryCreate([FromBody] TApi item) =>
            Serializer<TBusiness>.TryInsert(item.ToBusiness());

        //[HttpPut("{id:Guid}")]
        public virtual async Task<bool> TryUpdateAsync([FromBody] TApi item) =>
            await Serializer<TBusiness>.TryUpdateAsync(item.ToBusiness());

        [HttpPut("{id:Guid}")]
        public virtual bool TryUpdate([FromBody] TApi item) =>
            Serializer<TBusiness>.TryUpdate(item.ToBusiness());

        [HttpDelete("{id:Guid}")]
        public virtual async Task<bool> TryDeleteAsync(Guid id) =>
            await DbHelper.TryDeleteByIdAsync(typeof(TBusiness).ToSqlTableName(), id);

        public virtual bool TryDelete(Guid id) =>
            DbHelper.TryDeleteById(typeof(TBusiness).ToSqlTableName(), id);

        [HttpGet]
        public virtual async Task<IEnumerable<TApi>> GetWithFilterAsync([FromQuery] TApi item) =>
            (await Serializer<TBusiness>.DeserializeWithFilterAsync(item.ToBusiness()))
            .Select(x => uselessInstance.FromBusinessModel(x));

        public virtual IEnumerable<TApi> GetWithFilter([FromQuery] TApi item) =>
            (Serializer<TBusiness>.DeserializeWithFilter(item.ToBusiness()))
            .Select(x => uselessInstance.FromBusinessModel(x));
    }
}
