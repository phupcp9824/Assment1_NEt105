using Data.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Models;
using WebApi.Response;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepUser _IRepUser;
        private ILogger<UserController> _logger; // Error check
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public UserController(IRepUser repUser, ILogger<UserController> logger, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _IRepUser = repUser;
            _logger = logger;
            _configuration = configuration;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _IRepUser.GetAllUser();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            // Check if the Role exists
            if (user.RoleId.HasValue)
            {
                var role = await _IRepUser.GetRoleById(user.RoleId.Value); // Get infor role 
                if (role == null)
                {
                    _logger.LogWarning($"Role with ID {user.RoleId.Value} not found.");
                    return BadRequest($"Role with ID {user.RoleId.Value} not found.");
                }
            }
            var success  = await _IRepUser.CreateUser(user);
            if (success)
            {
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            else
            {
                _logger.LogError("Failed to create user.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) 
        {
            var user = await _IRepUser.GetById(id); 
            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(User user)
        {
             await _IRepUser.UpdateUser(user); 
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _IRepUser.DeleteUser(id); // await the async call
            if (result)
            {
                return NoContent(); // Return 204 No Content for successful deletion
            }
            else
            {
                _logger.LogWarning($"Student with ID {id} not found for deletion.");
                return NotFound($"Student with ID {id} not found for deletion.");
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModels Login)
        {
            var user = await _IRepUser.Login(Login);
            if (user == null)
            {
                return Ok(
                       new LoginResponse
                       {
                           Successfull = false,
                           Error = "Invalid Username and PassWord"
                       }
                   );
            }
            // switch check value cụ thể
            string role = user.RoleId switch
            {
                1 => "Admin",
                2 => "Customer",
                _ => "Guest"
            };

            // tạo mảng byte JwtSecurityKey / khóa bí mật từ configuration
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
            //thuật toán HmacSha256 để mã hóa / ký vào mã thông báo
            var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            // expire time 1day
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));
            // các thông tin đính kèm trong token đc mã hóa
            var claims = new[]
            {
                    new Claim(ClaimTypes.Name, Login.UserName),
                    new Claim(ClaimTypes.Role, role) // Add role claim
            };
            // tạo ra bằng những thông tin đã config
            var Token = new JwtSecurityToken(
                   _configuration["JwtIssuer"],
                   _configuration["JwtAudience"],
                   claims,
                   expires: expiry,
                   signingCredentials: creds
              );
            return Ok(
              new LoginResponse
              {
                  Successfull = true,
                  Error = "Authenticate Success",
                  // chuyển đổi thành chuỗi
                  Token = new JwtSecurityTokenHandler().WriteToken(Token),
                  Role = role
              }
           );
        }
    }
}
