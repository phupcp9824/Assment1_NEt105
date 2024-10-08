using WebApi.Models;
using WebApi.Response;

namespace Data.Repository.IRepository
{
    public interface IRepUser
    {
        Task<List<User>> GetAllUser();
        Task<bool> CreateUser(User user);
        Task<bool> UpdateUser(User user);
        Task<User> GetById (int id);
        Task<bool> DeleteUser (int id);
        Task<Role?> GetRoleById(int id);
        Task<User> Login(LoginModels login);
    }
}
