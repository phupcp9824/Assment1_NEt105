using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net.WebSockets;
using WebApi.Models;

namespace Front_End.Controllers
{
    public class ComboController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly Uri BaseAddress = new Uri("https://localhost:7137/api");

        public ComboController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = BaseAddress;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            List<Combo> combos = new List<Combo>();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Combo ");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                combos = JsonConvert.DeserializeObject<List<Combo>>(data);
            }
            return View(combos);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Combo combo, IFormFile imageURL)
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
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/Combo",combo);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("IndexCombo", "Admin");
            }
            return RedirectToAction("IndexCombo", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            Combo combo = new Combo();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Combo/" + id);
            if (response.IsSuccessStatusCode)
            {
                var apirespon = await response.Content.ReadAsStringAsync();
                combo = JsonConvert.DeserializeObject<Combo>(apirespon);
            }
            return View(combo);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Combo combo, IFormFile imageURL)
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
                var existingCombo = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Combo/" + combo.Id);
                if (existingCombo.IsSuccessStatusCode)
                {
                    var existingComboContent = await existingCombo.Content.ReadAsStringAsync();
                    var existingComboObj = JsonConvert.DeserializeObject<Combo>(existingComboContent);
                    combo.Picture = existingComboObj.Picture;
                }
            }
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + "/Combo", combo);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("IndexCombo", "Admin");
            }
            else
            {
                ModelState.AddModelError("", "Error updating combo. Please try again.");
            }
            return View(combo);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + "/Combo/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("IndexCombo", "Admin");
            }
            return RedirectToAction("IndexCombo", "Admin");
        }

        //[HttpGet]
        //public async Task<IActionResult> FindName(string name)
        //{
        //    HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Combo/GetByName?name={name}");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var combo = await response.Content.ReadAsStringAsync<Combo>(); 
        //        return View("IndexCombo", combo);
        //    }
        //    return RedirectToAction("IndexCombo", "Admin");
        //}
    }
}
