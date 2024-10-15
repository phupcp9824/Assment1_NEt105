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

        /// <summary>
        /// Part User
        /// </summary>
        /// <returns></returns>

        public async Task<IActionResult> IndexUser()
        {
            List<User> users = new List<User>();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/User/GetAll");

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
        public async Task<IActionResult> SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(User user)
        {
            try
            {
                string data = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                // Send to API  
                HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/User/Create", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Register Success";
                    return RedirectToAction("LoginFirm", "Admin");
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Error from server: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }
            return RedirectToAction("LoginFirm", "User");
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            User User = new User();
            // Send GET request to the API to get user by id
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/User/GetById/{id}");
            if (response.IsSuccessStatusCode)
            {
                // đọc content  từ api
                string data = await response.Content.ReadAsStringAsync();
                User = JsonConvert.DeserializeObject<User>(data); // chuyển chuỗi json về 1 đối tượng
            }
            return View(User);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(User user)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + "/User/Edit", user);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Edit Success";
                return RedirectToAction("IndexUser", "Admin");
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                // get request delete to APi
                HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + $"/User/Delete/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "User deleted successfully";
                    return RedirectToAction("IndexUser", "Admin");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete user.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> FindByUser(string fullname)
        {
            List<User> users = new List<User>();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/User/FindByUser/{fullname}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                // Deserialize into a single User object if that's what the API returns
                 users = JsonConvert.DeserializeObject<List<User>>(data);
            }
            return View("IndexUser", users);
        }

        /// <summary>
        /// Part Combo
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> IndexCombo()
        {

            List<Combo> combos = new List<Combo>();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Combo/GetAll");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                combos = JsonConvert.DeserializeObject<List<Combo>>(data);
            }
            return View(combos);
        }

        [HttpGet]
        public async Task<IActionResult> FindNameCombo(string name)
        {
            List<Combo> combos = new List<Combo>();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Combo/GetByName/{name}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                combos = JsonConvert.DeserializeObject<List<Combo>>(data);
            }
            return View("IndexCombo", combos);
        }

        [HttpGet]
        public IActionResult CreateCombo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCombo(Combo combo, IFormFile imageURL)
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
                combo.Picture = "/images/" + unique;
            }

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/Combo/Create", combo);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("IndexCombo", "Admin");
            }
            return RedirectToAction("IndexCombo", "Admin");
        }


        [HttpGet]
        public async Task<IActionResult> EditCombo(int? id)
        {
            Combo combo = new Combo();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Combo/GetById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                combo = JsonConvert.DeserializeObject<Combo>(apiResponse);
            }
            return View(combo);
        }



        [HttpPost]
        public async Task<IActionResult> EditCombo(Combo combo, IFormFile imageURL)
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
                combo.Picture = "/images/" + unique;
            }
            else
            {
                var existingComboResponse = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Combo/GetById/{combo.Id}");
                if (existingComboResponse.IsSuccessStatusCode)
                {
                    var existingComboContent = await existingComboResponse.Content.ReadAsStringAsync();
                    var existingComboObj = JsonConvert.DeserializeObject<Combo>(existingComboContent);
                    combo.Picture = existingComboObj.Picture;
                }
            }

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + "/Combo/Edit", combo);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("IndexCombo", "Admin");
            }
            else
            {
                ModelState.AddModelError("", "Lỗi khi cập nhật combo. Vui lòng thử lại.");
            }
            return View(combo);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCombo(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + $"/Combo/Delete/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("IndexCombo", "Admin");
            }
            return RedirectToAction("IndexCombo", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> DetailCombo(int? id)
        {
            Combo combo = new Combo();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Combo/GetById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                combo = JsonConvert.DeserializeObject<Combo>(apiResponse);
            }
            return View(combo);
        }

        /// <summary>
        /// Part Food
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> IndexFood()
        {
            List<FastFoodItem> fastFoodItems = new List<FastFoodItem>();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Food/GetAll");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                fastFoodItems = JsonConvert.DeserializeObject<List<FastFoodItem>>(data);
            }
            return View(fastFoodItems);
        }

        [HttpGet]
        public async Task<IActionResult> CreateFood()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFood(FastFoodItem fastFoodItem, IFormFile imageURL)
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
                return RedirectToAction("IndexFood", "Admin");
            }
            return RedirectToAction("IndexFood", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> EditFood(int? id)
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
        public async Task<IActionResult> EditFood(FastFoodItem fastFoodItem, IFormFile imageURL)
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
        public async Task<IActionResult> DeleteFood(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + "/Food/Delete/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("IndexFood", "Admin");
            }
            return RedirectToAction("IndexFood", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> DetailFood(int? id)
        {
            FastFoodItem fastFoodItem = new FastFoodItem();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Food/GetByIdCate/{id}");
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                fastFoodItem = JsonConvert.DeserializeObject<FastFoodItem>(apiResponse);
            }
            return View(fastFoodItem);
        }

        [HttpGet]
        public async Task<IActionResult> FindNameFood(string name)
        {
            List<FastFoodItem> fastFoodItems = new List<FastFoodItem>();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Food/GetByName/{name}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                fastFoodItems = JsonConvert.DeserializeObject<List<FastFoodItem>>(data);
            }
            return View("IndexFood", fastFoodItems);
        }
    }
}
