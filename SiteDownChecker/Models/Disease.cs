using System;
using SiteDownChecker.Business.Models;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming

namespace SiteDownChecker.Models
{
    public class Disease :
        IConvertibleToBusinessModel<SiteDownChecker.Business.Models.Disease>,
        IConvertibleFromBusinessModel<Disease, SiteDownChecker.Business.Models.Disease>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? Grade { get; set; }
        public string ISC { get; set; }

        public override string ToString() => $"[Id = {Id}, Name = {Name}, Grade = {Grade}, ISC = {ISC}]";

        public SiteDownChecker.Business.Models.Disease ToBusiness() => new()
        {
            Id = Id,
            Name = Name,
            Grade = Grade?.ToString(),
            ISC = ISC
        };

        public Disease FromBusinessModel(SiteDownChecker.Business.Models.Disease businessItem) => new()
        {
            Id = businessItem.Id,
            Name = businessItem.Name,
            Grade = int.Parse(businessItem.Grade),
            ISC = businessItem.ISC,
        };
    }
}