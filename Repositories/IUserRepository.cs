using FengShuiWeb.Models;

namespace FengShuiWeb.Repositories
{
    public interface IUserRepository
    {
        public User? GetUserByUsername(string username);
        void AddUser(User user);
    }
}
