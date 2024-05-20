using ClassLibrary;

namespace Avancerad.NET_SlutProjekt.Interfaces
{
    public interface IAuthenticationRepository
    {
        string GenerateJwtToken(User user, string newSigningKey);
        bool ValidateLogin(string username, string password);
    }
}