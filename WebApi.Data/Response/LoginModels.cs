using System.ComponentModel.DataAnnotations;

namespace WebApi.Response
{
    public class LoginModels
    {
        [Required(ErrorMessage = "UserName is required")]
        [MaxLength(50, ErrorMessage = "UserName can't be longer than 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(50, ErrorMessage = "Password can't be longer than 50 characters")]
        public string Password { get; set; }
    }
}
