using Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
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

        public async Task<List<User>> FindByUser(string fullname, string phone, string email)
        {
            //var FoodByName = await _db.Users.Include(x => x.Role)
            //                                       .Where(x => x.Fullname.ToLower().Contains(fullname.ToLower())
            //                                       && x.Phone.ToLower().Contains(phone.ToLower())
            //                                       && x.Email.ToLower().Contains(email.ToLower()))
            //                                       .ToListAsync();
            //return FoodByName;
            //use AsQueryable để truy vấn linh hoạt no need implement right now, help tối ưu hóa hiệu suất
           var query = _db.Users.Include(x => x.Role).AsQueryable();

            if (!string.IsNullOrWhiteSpace(fullname))
            {
                query = query.Where(x => x.Fullname.ToLower().Trim().Contains(fullname.ToLower().Trim()));
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                query = query.Where(x => x.Phone.ToLower().Trim().Contains(phone.ToLower().Trim()));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(x => x.Email.ToLower().Trim().Contains(email.ToLower().Trim()));
            }

            return await query.ToListAsync();
        }
    }
}
