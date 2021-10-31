using System;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming

namespace SiteDownChecker.Business.Models
{
    public class Disease
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Grade { get; set; }
        public string ISC { get; set; }

        public override string ToString() => $"{Id} {Name} {Grade} {ISC}";
    }
}
