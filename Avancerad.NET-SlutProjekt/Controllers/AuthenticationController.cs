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
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationRepository _authenticationRepository;

        public AuthenticationController(IConfiguration configuration, IUserRepository userRepository, IAuthenticationRepository authenticationRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _authenticationRepository = authenticationRepository;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin login)
        {
            // Retrieve the user from the repository
            var user = _userRepository.FindByUsername(login.Username);
            if (user != null && _userRepository.CheckPassword(user, login.Password))
            {
                // Hämta den nya signing key från en källa, t.ex. konfigurationen
                var newSigningKey = _configuration["Jwt:NewSigningKey"];

                // Generate JWT token for the authenticated user
                var token = _authenticationRepository.GenerateJwtToken(user, newSigningKey);

                // Return the token as a response
                return Ok(new { token = token });
            }
            else
            {
                // Invalid login credentials
                return Unauthorized("Invalid username or password");
            }
        }
    }
}
