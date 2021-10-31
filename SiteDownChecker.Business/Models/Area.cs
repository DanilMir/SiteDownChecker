using System;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SiteDownChecker.Business.Models
{
    public class Area : IConvertibleToBusinessModel<Area>, IConvertibleFromBusinessModel<Area, Area>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid VideoId { get; set; }

        public override string ToString() => $"{Id} {Name} {VideoId}";
    }
}