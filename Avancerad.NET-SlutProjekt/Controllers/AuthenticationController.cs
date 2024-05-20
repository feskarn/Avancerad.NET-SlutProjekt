using Avancerad.NET_SlutProjekt.DTO.Requests;
using Avancerad.NET_SlutProjekt.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Avancerad.NET_SlutProjekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationRepository _authenticationRepository;

        public AuthenticationController(IUserRepository userRepository, IAuthenticationRepository authenticationRepository)
        {
            _userRepository = userRepository;
            _authenticationRepository = authenticationRepository;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin login)
        {
            var user = _userRepository.FindByUsername(login.Username);

            if (user != null && _userRepository.CheckPassword(user, login.Password))
            {
                var token = _authenticationRepository.GenerateJwtToken(user);
                return Ok(new { token });
            }
            return Unauthorized();
        }

    }
}