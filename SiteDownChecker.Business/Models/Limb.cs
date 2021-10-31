using System;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SiteDownChecker.Business.Models
{
    public class Limb : IConvertibleToBusinessModel<Limb>, IConvertibleFromBusinessModel<Limb, Limb>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid BodyPartId { get; set; }

        public override string ToString() => $"{Id} {Name} {BodyPartId}";
    }
}