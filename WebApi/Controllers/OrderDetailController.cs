using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.SessionExten;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly OrderDbContext _db;
        public OrderDetailController(OrderDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = HttpContext.Session.GetString("User");
            if (user == null)
            {
                return Unauthorized("You must be logged in.");
            }

            var getUser = await _db.Users.FirstOrDefaultAsync(x => x.UserName == user);
            if (getUser == null)
            {
                return NotFound("User not found.");
            }
            // get đơn hàng của user
            var getOrder = await _db.Orders.FirstOrDefaultAsync(x => x.UserId == getUser.Id);
            if (getOrder == null)
            {
                return NotFound("Order not found.");
            }

            // get infor detail about 
            var orderDetails = await _db.OrderDetails
                .Where(x => x.OrderId == getOrder.Id)
                .ToListAsync();

            return Ok(orderDetails);
        }

        [HttpGet]
        public async Task<IActionResult> AddtoCart()
        {
            var Orderdetail = await _db.OrderDetails.ToListAsync();
            return Ok(Orderdetail); 
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int? ComboId, int? FoodId, int amount)
        {
            //var user = HttpContext.Session.GetString("User");
            //if (user == null)
            //{
            //    return Unauthorized("You must be logged in to add items to the cart.");
            //}
            var user = 3;
            // Retrieve user information
            var getUser = await _db.Users.FirstOrDefaultAsync(x => x.Id == user);
            if (getUser == null)
            {
                return NotFound("User not found.");
            }

            // Get or create an order for the user
            var getOrder = await _db.Orders.FirstOrDefaultAsync(x => x.UserId == getUser.Id && x.Status == "Pending");
            if (getOrder == null)
            {
                getOrder = new Order
                {
                    UserId = getUser.Id,
                    DateOrder = DateTime.Now,
                    Status = "Pending",
                    TotalPrice = 0, // Initialize total price
                    SippingAddress = getUser.Address
                };
                await _db.Orders.AddAsync(getOrder);
                await _db.SaveChangesAsync();
            }

            // Add Combo to OrderDetail
            if (ComboId.HasValue)
            {
                var combo = await _db.Combos.FindAsync(ComboId.Value);
                if (combo == null)
                {
                    return NotFound("Combo not found.");
                }

                var existingOrderDetail = await _db.OrderDetails
                    .FirstOrDefaultAsync(x => x.OrderId == getOrder.Id && x.ComboId == ComboId.Value);
                if (existingOrderDetail == null)
                {
                    var newOrderDetail = new OrderDetail
                    {
                        OrderId = getOrder.Id,
                        ComboId = combo.Id,
                        Usename = combo.Name,
                        Price = combo.Price,
                        Quantity = amount,
                        TotalPrice = combo.Price * amount
                    };
                    await _db.OrderDetails.AddAsync(newOrderDetail);
                }
                else
                {
                    existingOrderDetail.Quantity += amount;
                    existingOrderDetail.TotalPrice += amount * combo.Price;
                    _db.OrderDetails.Update(existingOrderDetail);
                }
            }

            // Check if adding a Fast Food item
            if (FoodId.HasValue)
            {
                var food = await _db.FastFoodItems.FindAsync(FoodId.Value);
                if (food == null)
                {
                    return NotFound("Food not found.");
                }

                var existingOrderDetail = await _db.OrderDetails
                    .FirstOrDefaultAsync(x => x.OrderId == getOrder.Id && x.FastFoodItemId == FoodId.Value);

                if (existingOrderDetail == null)
                {
                    var newOrderDetail = new OrderDetail
                    {
                        OrderId = getOrder.Id,
                        FastFoodItemId = food.Id,
                        Usename = food.Name,
                        Price = food.Price,
                        Quantity = amount,
                        TotalPrice = food.Price * amount
                    };
                    await _db.OrderDetails.AddAsync(newOrderDetail);
                }
                else
                {
                    existingOrderDetail.Quantity += amount;
                    existingOrderDetail.TotalPrice += food.Price * amount;
                    _db.OrderDetails.Update(existingOrderDetail);
                }
            }

            // Save changes to OrderDetails and update the total price of the order
            await _db.SaveChangesAsync();
            getOrder.TotalPrice = await _db.OrderDetails.Where(od => od.OrderId == getOrder.Id).SumAsync(od => od.TotalPrice);
            _db.Orders.Update(getOrder);
            await _db.SaveChangesAsync();

            return Ok(getOrder);
        }

    }
}
