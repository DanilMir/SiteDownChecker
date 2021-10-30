using System;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace MedicalVideo.Business.Models
{
    public class BodyPart : IConvertibleToBusinessModel<BodyPart>, IConvertibleFromBusinessModel<BodyPart, BodyPart>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid Area { get; set; }

        public override string ToString() => $"{Id} {Name} {Area}";
    }
}