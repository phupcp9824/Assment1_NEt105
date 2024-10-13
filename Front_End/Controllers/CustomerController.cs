using Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Xml.Linq;
using WebApi.Models;

namespace Front_End.Controllers
{
    public class CustomerController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly Uri BaseAddress = new Uri("https://localhost:7137/api");

        public CustomerController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = BaseAddress;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
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
        public async Task<IActionResult> InterfaceCombo()
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

        [HttpGet]
        public async Task<IActionResult> InterfaceFood()
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
        public async Task<IActionResult> IndexAll()
        {

            var viewModel = new Data.Models.ComboAndFoodViewModel
            {
                Combos = new List<Combo>(),
                Foods = new List<FastFoodItem>()
            };

            HttpResponseMessage responseCombos = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Combo/GetAll");
            if (responseCombos.IsSuccessStatusCode)
            {
                string dataCombos = await responseCombos.Content.ReadAsStringAsync();
                viewModel.Combos = JsonConvert.DeserializeObject<List<Combo>>(dataCombos);
            }

            HttpResponseMessage responseFoods = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Food/GetAll");
            if (responseFoods.IsSuccessStatusCode)
            {
                string dataFoods = await responseFoods.Content.ReadAsStringAsync();
                viewModel.Foods = JsonConvert.DeserializeObject<List<FastFoodItem>>(dataFoods);
            }
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Find(string name)
        {
            var viewModel = new Data.Models.ComboAndFoodViewModel
            {
                Combos = new List<Combo>(),
                Foods = new List<FastFoodItem>()
            };

            HttpResponseMessage responseCombo = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Combo/{name}");
            if (responseCombo.IsSuccessStatusCode)
            {
                var data = await responseCombo.Content.ReadAsStringAsync();
                viewModel.Combos = JsonConvert.DeserializeObject<List<Combo>>(data);
            }

            HttpResponseMessage responseFood = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Food/{name}");
            if (responseFood.IsSuccessStatusCode)
            {
                var data = await responseFood.Content.ReadAsStringAsync();
                viewModel.Foods = JsonConvert.DeserializeObject<List<FastFoodItem>>(data);
            }
            return View("Find", viewModel); 
        }

    }
}
  
