using Avancerad.NET_SlutProjekt.Data;
using Avancerad.NET_SlutProjekt.Interfaces;
using ClassLibrary;
using Microsoft.AspNetCore.Identity;

namespace Avancerad.NET_SlutProjekt.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool CheckPassword(User user, string password)
        {
            return user.PasswordHash == password;
        }

        public async Task<int> CreateUser(User user)
        {
            var result = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return result.Entity.Id;
        }

        public User FindByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }
    }
}