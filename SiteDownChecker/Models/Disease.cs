using System;
using MedicalVideo.Business.Models;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming

namespace MedicalVideo.Models
{
    public class Disease :
        ApiModel,
        IConvertibleToBusinessModel<Business.Models.Disease>,
        IConvertibleFromBusinessModel<Disease, Business.Models.Disease>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? Grade { get; set; }
        public string ISC { get; set; }

        public override string ToString() => MakeStringFromProperties();

        public Business.Models.Disease ToBusiness() => new()
        {
            Id = Id,
            Name = Name,
            Grade = Grade?.ToString(),
            ISC = ISC
        };

        public Disease FromBusinessModel(Business.Models.Disease businessItem) => new()
        {
            Id = businessItem.Id,
            Name = businessItem.Name,
            Grade = int.Parse(businessItem.Grade),
            ISC = businessItem.ISC,
        };
    }
}