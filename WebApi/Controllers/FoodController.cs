using Data.Repository.IRepository;
using Data.Repository.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IRepFood _repFood;
        private ILogger<FoodController> _logger; // Error check
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public FoodController(IRepFood repFood, ILogger<FoodController> logger, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _repFood = repFood;
            _logger = logger;
            _configuration = configuration;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _repFood.GetAllFood();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FastFoodItem fastFoodItem)
        {
            var add = _repFood.CreateFood(fastFoodItem);
            return Ok(add);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var ComboId = await _repFood.GetById(id); // await the async call
            if (ComboId == null)
            {
                _logger.LogWarning($"Student with ID {id} not found.");
                return NotFound($"Student with ID {id} not found.");
            }
            return Ok(ComboId);
        }


        [HttpPut]
        public async Task<IActionResult> Edit( FastFoodItem fastFoodItem)
        {
            await _repFood.UpdateFood(fastFoodItem);
            return Ok(fastFoodItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var FoodId = await _repFood.DeleteFood(id); // await the async call
            if (FoodId == null)
            {
                _logger.LogWarning($"Student with ID {id} not found.");
                return NotFound($"Student with ID {id} not found.");
            }
            return Ok(FoodId);
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name cannot be empty");
            }

            var Food = await _repFood.GetByName(name);

            if (Food == null)
            {
                return NotFound($"Combo with name '{name}' not found");
            }

            return Ok(Food);

        }

    }
}
