using Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Security.Claims;
using System.Text;
using WebApi.Models;


namespace Front_End.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly Uri BaseAddress = new Uri("https://localhost:7137/api");

        public UserController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = BaseAddress;
        }

        [HttpGet]
        public async Task<IActionResult> Customerface()
        {
            return View();
        }

        public async Task<IActionResult> Index()
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
        public async Task<IActionResult> LoginFirm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginFirm(User user)
        {
            try
            {
                // Serialize the user object to JSON
                string data = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                // Send to API
                HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Login/Login", content);

                // Check if response is successful
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(jsonResponse);

                    if (loginResponse.Successfull)
                    {
                        TempData["UserEmail"] = user.Email;

                        TempData["Success"] = user.UserName + " Login Success";
                        switch (loginResponse.Role)
                        {
                            case "Admin":
                                return RedirectToAction("ControlPanel", "Admin");
                            case "Customer":
                                return RedirectToAction("IndexAll", "Customer");
                            default:
                                return RedirectToAction("IndexAll", "Customer");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, loginResponse.Error);
                        return View("Login", user);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid credentials");
                    return View("Login", user);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Đăng nhập không thành công: {ex.Message}";
                return RedirectToAction("Login");
            }
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
                    return RedirectToAction("LoginFirm","User");
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
        public async Task<IActionResult> Edit(int id)
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
        public async Task<IActionResult> Edit(User user)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + "/User/Edit", user);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Edit Success";
                return RedirectToAction("IndexUser","Admin");
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // get request delete to APi
                HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + $"/User/Delete/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "User deleted successfully";
                    return RedirectToAction("IndexUser","Admin");
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
    }
}
