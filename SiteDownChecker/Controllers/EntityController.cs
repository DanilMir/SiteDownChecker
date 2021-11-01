using System;
using System.Collections.Generic;
using System.Linq;
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

        /*[HttpGet]
        public virtual List<TApi> GetALl() =>
            Serializer<TBusiness>.DeserializeAll().Select(x => uselessInstance.FromBusinessModel(x)).ToList();*/

        [HttpGet("{id:Guid}")]
        public virtual TApi GetById(Guid id) =>
            uselessInstance.FromBusinessModel(Serializer<TBusiness>.DeserializeFromId(id));

        [HttpPost]
        public virtual bool Create([FromBody] TApi item) => Serializer<TBusiness>.TryInsert(item.ToBusiness());

        [HttpPut("{id:Guid}")]
        public virtual bool Update([FromBody] TApi item) => Serializer<TBusiness>.TryUpdate(item.ToBusiness());

        [HttpDelete("{id:Guid}")]
        public virtual bool Delete(Guid id) => DbHelper.TryDeleteById(typeof(TBusiness).ToSqlTableName(), id);

        [HttpGet]
        public virtual IEnumerable<TApi> GetWithFilter([FromQuery] TApi item) =>
            Serializer<TBusiness>
                .DeserializeWithFilter(item.ToBusiness())
                .Select(x => uselessInstance.FromBusinessModel(x));
    }
}
