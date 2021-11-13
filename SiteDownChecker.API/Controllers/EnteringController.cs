using Microsoft.AspNetCore.Mvc;
using SiteDownChecker.API.Authentication;
using SiteDownChecker.Business.LoginDealing;
using SiteDownChecker.Business.Models;

// ReSharper disable StringLiteralTypo

namespace SiteDownChecker.API.Controllers
{
    [Route("[controller]")]
    public class EnteringController : Controller
    {
        private readonly IJwtAuthManager _jwtAuthManager;

        public EnteringController(IJwtAuthManager jwtAuthManager) =>
            _jwtAuthManager = jwtAuthManager;

        [HttpPost, Route(nameof(Login))]
        public ActionResult Login([FromBody] User user) =>
            LoginDealer.IsRegistered(LoginDealer.SetId(user))
                ? LoginDealer.IsPasswordCorrect(user)
                    ? Ok($"вы успешно вошли. ваш токен: {_jwtAuthManager.GetToken(user.Id.ToString())}")
                    : Ok("неверный пароль")
                : Ok("вы не зарегистрированы");

        [HttpPost, Route(nameof(Register))]
        public ActionResult Register([FromBody] User user) =>
            LoginDealer.IsRegistered(LoginDealer.SetId(user))
                ? Ok("ты уже зареган")
                : LoginDealer.TryRegisterNewUser(user)
                    ? Ok("вы успешно зарегистрировались")
                    : Ok("чот пошло не так");
    }
}
