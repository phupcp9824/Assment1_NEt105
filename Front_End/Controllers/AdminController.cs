using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using WebApi.Models;

namespace Front_End.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly Uri BaseAddress = new Uri("https://localhost:7137/api");

        public AdminController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = BaseAddress;
        }
        [HttpGet]
        public async Task<IActionResult> ControlPanel()
        {
            return View();
        }


        public async Task<IActionResult> IndexUser()
        {
            List<User> users = new List<User>();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/User");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<User>>(data);
            }
            else
            {
                string errorData = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Unable to load users. Error: {errorData}");
            }

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> IndexCombo()
        {

            List<Combo> combos = new List<Combo>();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Combo");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                combos = JsonConvert.DeserializeObject<List<Combo>>(data);
            }
            return View(combos);
        }

        [HttpGet]
        public async Task<IActionResult> IndexFood()
        {
            List<FastFoodItem> fastFoodItems = new List<FastFoodItem>();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Food");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                fastFoodItems = JsonConvert.DeserializeObject<List<FastFoodItem>>(data);
            }
            return View(fastFoodItems);
        }

    }
}
