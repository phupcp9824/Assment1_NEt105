using Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Response;

namespace Data.Repository.Repository
{
    public class RepUser : IRepUser
    {
        private readonly OrderDbContext _db;
        public RepUser(OrderDbContext db)
        {
            _db = db;
        }

        public async Task<User> Login(LoginModels login)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.UserName == login.UserName && x.PassWord == login.Password);
        }
        public async Task<List<User>> GetAllUser()
        {
            try
            {
                return await _db.Users.Include(x => x.Role).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                throw; // Rethrow the exception to propagate it
            }
        }

        public async Task<Role?> GetRoleById(int id)
        {
            return await _db.Roles.FindAsync(id); 
        }

        public async Task<bool> CreateUser(User user)
        {
            try
            {
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); 
                return false;
            }
        }

        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                var User = await _db.Users.FindAsync(id);
                if (User == null)
                {
                    return false;
                }

                _db.Users.Remove(User);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log error message
                return false;
            }
        }
     
        public async Task<User> GetById(int id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task<bool> UpdateUser(User user)
        {
            try
            {
                _db.Users.Update(user);
                await _db.SaveChangesAsync();   
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log error message
                return false;
            }
        }

    }
}
