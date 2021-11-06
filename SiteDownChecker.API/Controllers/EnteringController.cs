using Microsoft.AspNetCore.Mvc;
using SiteDownChecker.Business.LoginDealing;
using SiteDownChecker.Business.Models;

// ReSharper disable StringLiteralTypo

namespace SiteDownChecker.API.Controllers
{
    [ApiController, Route("[controller]")]
    public class EnteringController : Controller
    {
        [HttpPost, Route(nameof(Login))]
        public ActionResult Login([FromQuery] string login, [FromQuery] string password)
        {
            var user = new User
            {
                Login = login,
                Password = password
            };
            return LoginDealer.IsRegistered(LoginDealer.SetId(user))
                ? LoginDealer.IsPasswordCorrect(user)
                    ? Ok("вы успешно вошли")
                    : Ok("неверный пароль")
                : Ok("вы не зарегистрированы");
        }

        [HttpPost, Route(nameof(Register))]
        public ActionResult Register([FromQuery] string login, [FromQuery] string password)
        {
            var user = new User
            {
                Login = login,
                Password = password
            };
            return LoginDealer.IsRegistered(LoginDealer.SetId(user))
                ? Ok("ты уже зареган")
                : LoginDealer.TryRegisterNewUser(user)
                    ? Ok("вы успешно зарегистрировались")
                    : Ok("чот пошло не так");
        }
    }
}
