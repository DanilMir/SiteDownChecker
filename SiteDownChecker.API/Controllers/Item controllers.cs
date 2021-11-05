using Microsoft.AspNetCore.Mvc;
using SiteDownChecker.Business.Models;

namespace SiteDownChecker.API.Controllers
{
    [ApiController, Route("[controller]")]
    public class AreaController : EntityController<Area,Area>
    {
    }

    [ApiController, Route("[controller]")]
    public class BodyPartController : EntityController<BodyPart, BodyPart>
    {
    }

    [ApiController, Route("[controller]")]
    public class DisiagesController : EntityController<API.Models.Disease, Disease>
    {
    }

    [ApiController, Route("[controller]")]
    public class LimbController : EntityController<Limb, Limb>
    {
    }

    [ApiController, Route("[controller]")]
    public class VideoController : EntityController<Video, Video>
    {
    }
}
