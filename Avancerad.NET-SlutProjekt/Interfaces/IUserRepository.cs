using ClassLibrary;

namespace Avancerad.NET_SlutProjekt.Interfaces
{
    public interface IUserRepository
    {

        Task<int> CreateUser(User user);
        bool CheckPassword(User user, string password);
        User FindByUsername(string username);

    }
}
