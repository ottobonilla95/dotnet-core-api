using System.Threading.Tasks;
using webapi.Models;

namespace webapi.Data.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> GetUserById(int id);
        Task<User> GetUserByUserName(string username);
        Task<User> GetUserByEmail(string email);
        Task<User> RegisterUser(User user);
        Task<User> SaveRefreshToken(User user);
        Task<string> GetRefreshToken(int userId);
        void SaveChanges();
    }
}
