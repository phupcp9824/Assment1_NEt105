using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using WebApi.Models;
using WebApi.SessionExten;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace Front_End.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly Uri BaseAddress = new Uri("https://localhost:7137/api");

        public OrderDetailController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = BaseAddress;
        }

        [HttpGet]
        public async Task<IActionResult> AddtoCart()
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/OrderDetail/AddtoCart");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                orderDetails = JsonConvert.DeserializeObject<List<OrderDetail>>(data);
            }
            return View(orderDetails);

        }

        [HttpPost]  
        public async Task<IActionResult> AddtoCart(int? foodId = null, int? comboId = null, int amount = 1)
        {
            var requestData = new { ComboId = comboId, FoodId = foodId, Amount = amount };
            var jsonContent = System.Text.Json.JsonSerializer.Serialize(requestData);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/OrderDetail/AddToCart", httpContent);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Item added to cart successfully!";
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ViewBag.Message = "Error adding item to cart: " + errorContent;
            }

            //// get the food items and combos after adding to cart
            //var foodItemsResponse = await _httpClient.GetAsync(_httpClient.BaseAddress + "/OrderDetail/AddtoCart");
            //var foodItemsContent = await foodItemsResponse.Content.ReadAsStringAsync();
            //var model = System.Text.Json.JsonSerializer.Deserialize<ComboAndFoodViewModel>(foodItemsContent);

            return RedirectToAction("AddtoCart", "OrderDetail");
        }
    }
}
