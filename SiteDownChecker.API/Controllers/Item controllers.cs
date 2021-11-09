using Microsoft.AspNetCore.Mvc;
using SiteDownChecker.Business.Models;

namespace SiteDownChecker.API.Controllers
{
    [Route("[controller]")]
    public class AreaController : EntityController<Area, Area> { }

    [Route("[controller]")]
    public class BodyPartController : EntityController<BodyPart, BodyPart> { }

    [Route("[controller]")]
    //TODO поменять обратно на diseases
    public class DisiagesController : EntityController<API.Models.Disease, Disease> { }

    [Route("[controller]")]
    public class LimbController : EntityController<Limb, Limb> { }

    [Route("[controller]")]
    public class VideoController : EntityController<Video, Video> { }
}
