using Avancerad.NET_SlutProjekt.Interfaces;
using ClassLibrary;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Avancerad.NET_SlutProjekt.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {

        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthenticationRepository(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }
        public string GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var securityKey = new SymmetricSecurityKey(key);

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };


            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(120),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateLogin(string username, string password)
        {
            var user = _userRepository.FindByUsername(username);
            if (user != null && _userRepository.CheckPassword(user, password))
            {
                return true;
            }
            return false;
        }
    }
}