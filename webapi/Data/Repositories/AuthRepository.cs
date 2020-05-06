using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using webapi.Data.Interfaces;
using webapi.Models;

namespace webapi.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<User> GetUserById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
        public async Task<User> GetUserByUserName(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return user;
        }
        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }
        public async Task<User> RegisterUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> SaveRefreshToken(User user)
        {
            var userToUpdate = await _context.Users.FirstOrDefaultAsync(u=>u.Id==user.Id);
            userToUpdate.RefreshToken = user.RefreshToken;
            await _context.SaveChangesAsync();
            return userToUpdate;
        }
        public async Task<string> GetRefreshToken(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user.RefreshToken;
        }
        

        public async void SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
