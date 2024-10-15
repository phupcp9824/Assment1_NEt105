namespace WebApi.Models
{
    public class LoginResponse
    {
        public bool Successfull { get; set; }
        public string Error {  get; set; }
        public Object Token { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}
