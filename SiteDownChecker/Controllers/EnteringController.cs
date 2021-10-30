using MedicalVideo.Business.LoginDealing;
using MedicalVideo.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedicalVideo.Controllers
{
    [ApiController, Route("[controller]")]
    public class EnteringController : Controller
    {
        [HttpPost, Route(nameof(Login))]
        public ActionResult Login([FromBody] User user) =>
            LoginDealer.IsRegistered(LoginDealer.SetId(user))
                ? LoginDealer.IsPasswordCorrect(user)
                    ? Ok("вы успешно вошли")
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
