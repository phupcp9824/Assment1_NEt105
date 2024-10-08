using Data.Repository.IRepository;
using Data.Repository.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComboController : ControllerBase
    {
        private readonly IRepCombo _repCombo;
        private ILogger<ComboController> _logger; // Error check
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public ComboController(IRepCombo repCombo, ILogger<ComboController> logger, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _repCombo = repCombo;
            _logger = logger;
            _configuration = configuration;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _repCombo.GetAllCombo();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Combo comboImage)
        {
            var add = _repCombo.CreateCombo(comboImage);
            return Ok(add); 
        }

        [HttpPut]
        public async Task<IActionResult> Edit(Combo combo)
        {
            await _repCombo.UpdateCombo(combo);
            return Ok(combo);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ComboId = await _repCombo.GetById(id); // await the async call
            if (ComboId == null)
            {
                _logger.LogWarning($"Student with ID {id} not found.");
                return NotFound($"Student with ID {id} not found.");
            }
            return Ok(ComboId);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ComboId = await _repCombo.DeleteCombo(id); 
            return Ok(ComboId);
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName( string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name cannot be empty");
            }

            var combo = await _repCombo.GetByName(name);

            if (combo == null)
            {
                return NotFound($"Combo with name '{name}' not found");
            }

            return Ok(combo);

        }
    }
}

