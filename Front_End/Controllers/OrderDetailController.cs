using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddtoCart(int foodId, int amount)
        {   
            var requestData = new
            {
                FoodId = foodId,
                Amount = amount
            }; 
            // Serialize the data to JSON
            var jsonContent = JsonSerializer.Serialize(requestData);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Send the POST request to the API
            var response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/OrderDetail/AddToCart", httpContent);

            // Check the response
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                ViewBag.Message = responseContent;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ViewBag.Message = errorContent;
            }

            return View(); 
        }
    }
}
