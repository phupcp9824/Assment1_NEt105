using Data.Models;

namespace WebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        // 1user - 1 role
        public int? RoleId { get; set; }
        public virtual Role? Role { get; set; }

        public virtual List<Invoice>? Invoice { get; set; }

        // 1user - n odder
        public virtual List<Order>? Orders { get; set; }
    }
}
