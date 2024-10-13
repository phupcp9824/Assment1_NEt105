using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Models;

namespace Front_End.Controllers
{
    public class FoodController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly Uri BaseAddress = new Uri("https://localhost:7137/api");

        public FoodController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = BaseAddress;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<FastFoodItem> fastFoodItems = new List<FastFoodItem>();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Food/GetAll");
            if(response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                fastFoodItems = JsonConvert.DeserializeObject<List<FastFoodItem>>(data);
            }
            return View(fastFoodItems);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FastFoodItem fastFoodItem, IFormFile imageURL)
        {
            if (imageURL != null && imageURL.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                var unique = Guid.NewGuid().ToString() + "_" + imageURL.FileName;
                var filepath = Path.Combine(path, unique);
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    await imageURL.CopyToAsync(stream);
                }
                fastFoodItem.Picture = "/images/" + unique;
            }
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/Food/Create", fastFoodItem);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("IndexFood","Admin");
            }
            return RedirectToAction("IndexFood", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            FastFoodItem fastFoodItem = new FastFoodItem();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Food/GetbyId/{id}");
            if (response.IsSuccessStatusCode)
            {
                var apirespon = await response.Content.ReadAsStringAsync();
                fastFoodItem = JsonConvert.DeserializeObject<FastFoodItem>(apirespon);
            }
            return View(fastFoodItem);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FastFoodItem fastFoodItem, IFormFile imageURL)
        {
            if (imageURL != null && imageURL.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                var unique = Guid.NewGuid().ToString() + "_" + imageURL.FileName;
                var filepath = Path.Combine(path, unique);
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    await imageURL.CopyToAsync(stream);
                }
                fastFoodItem.Picture = "/images/" + unique; 
            }
            else
            {
                var existingCombo = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Food/GetbyId/{fastFoodItem.Id}");
                if (existingCombo.IsSuccessStatusCode)
                {
                    var existingComboContent = await existingCombo.Content.ReadAsStringAsync();
                    var existingComboObj = JsonConvert.DeserializeObject<Combo>(existingComboContent);
                    fastFoodItem.Picture = existingComboObj.Picture;
                }
            }
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + "/Food/Edit", fastFoodItem);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("IndexFood", "Admin");
            }
            else
            {
                ModelState.AddModelError("", "Error updating combo. Please try again.");
            }
            return View(fastFoodItem);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + "/Food/Delete/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("IndexFood", "Admin");
            }
            return RedirectToAction("IndexFood", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> FindName(string name)
        {
            List<FastFoodItem> fastFoodItems = new List<FastFoodItem>();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Food/GetByName/{name}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                fastFoodItems = JsonConvert.DeserializeObject<List<FastFoodItem>>(data);
            }
            return View("Index", fastFoodItems);
        }

    }
}
