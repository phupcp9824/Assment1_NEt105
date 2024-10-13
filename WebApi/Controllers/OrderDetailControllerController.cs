using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderDetailControllerController : ControllerBase
    {
        private readonly OrderDbContext _db;
        public OrderDetailControllerController(OrderDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var User = HttpContext.Session.GetString("User");
            if (User == null)
            {
                return Content("You not Login");
            }
            else
            {
                // truy xuất dl người dùng
                var GetUser = _db.Users.FirstOrDefault(x => x.UserName == User);
                // get đơn hàng user
                var GetOrder = _db.Orders.FirstOrDefault(x => x.Id == GetUser.Id);
                // get infor detail đơnhàng
                var GHCTdata = _db.OrderDetails.Where(x => x.Id == GetOrder.Id).ToList();
                return Ok(GHCTdata);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int? FoodId, int amount)
        {
            if (FoodId == null || amount <= 0)
            {
                return Content("Số lượng không hợp lệ hoặc Món ăn không hợp lệ");
            }

            // Get user stored in session
            var user = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(user))
            {
                return Content("Chưa đăng nhập hoặc phiên bản hết hạn");
            }

            // Get user information
            var GetUser = await _db.Users.FirstOrDefaultAsync(x => x.UserName == user);
            if (GetUser == null)
            {
                return Content("User not found.");
            }

            // Check the active cart for the user
            var GetOrder = await _db.Orders.FirstOrDefaultAsync(x => x.UserId == GetUser.Id && x.Status == "Pending");
            if (GetOrder == null)
            {
                // Create a new order if none exists
                GetOrder = new Order
                {
                    UserId = GetUser.Id,
                    DateOrder = DateTime.Now,
                    Status = "Pending",
                    SippingAddress = "To be defined", 
                    TotalPrice = 0
                };
                await _db.Orders.AddAsync(GetOrder);
                await _db.SaveChangesAsync();
            }

            // get the food item only once
            var foodItem = await _db.FastFoodItems.FindAsync(FoodId);
            if (foodItem == null)
            {
                return Content("Food item not found.");
            }

            // Check if the food item is already in the order details
            var existingOrderDetail = await _db.OrderDetails
                .FirstOrDefaultAsync(x => x.IdOrder == GetOrder.Id && x.IdFood == FoodId);

            if (existingOrderDetail != null)
            {
                existingOrderDetail.Quantity += amount;
                existingOrderDetail.TotalPrice = existingOrderDetail.Quantity * foodItem.Price;
                _db.OrderDetails.Update(existingOrderDetail);
            }
            else
            {
                var newOrderDetail = new OrderDetail
                {
                    IdOrder = GetOrder.Id,
                    IdFood = FoodId,
                    Quantity = amount,
                    TotalPrice = amount * foodItem.Price // Calculate total price
                };
                await _db.OrderDetails.AddAsync(newOrderDetail);
            }

            GetOrder.TotalPrice += amount * foodItem.Price; // Use the fetched foodItem
            _db.Orders.Update(GetOrder);
            await _db.SaveChangesAsync();

            return Content("Thêm vào giỏ hàng thành công");
        }

    }
}
