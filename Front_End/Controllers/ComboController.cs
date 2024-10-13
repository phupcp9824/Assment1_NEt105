using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.Models;
using System.Collections.Generic;
using System.IO;

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
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Combo/GetAll");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                combos = JsonConvert.DeserializeObject<List<Combo>>(data);
            }
            return View(combos);
        }

        [HttpGet]
        public IActionResult Create()
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

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/Combo/Create", combo);
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
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Combo/GetById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                combo = JsonConvert.DeserializeObject<Combo>(apiResponse);
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

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
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
        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + $"/Combo/Delete/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("IndexCombo", "Admin");
            }
            return RedirectToAction("IndexCombo", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> FindName(string name)
        {
            List<Combo> combos = new List<Combo>();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Combo/GetByName/{name}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                combos = JsonConvert.DeserializeObject<List<Combo>>(data);
            }
            return View("Index", combos);
        }
    }
}