using System;
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace MedicalVideo.Business.Models
{
    public class Video : IConvertibleToBusinessModel<Video>, IConvertibleFromBusinessModel<Video, Video>
    {
        public Guid Id { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }

        public override string ToString() => $"{Id} {URL} {Title}";
    }
}
