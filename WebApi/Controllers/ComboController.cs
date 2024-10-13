using Data.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ComboController : ControllerBase
    {
        private readonly IRepCombo _repCombo;
        private readonly ILogger<ComboController> _logger;
        private readonly IConfiguration _configuration;

        public ComboController(IRepCombo repCombo, ILogger<ComboController> logger, IConfiguration configuration)
        {
            _repCombo = repCombo;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _repCombo.GetAllCombo();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Combo comboImage)
        {
            if (comboImage == null)
            {
                return BadRequest("Invalid data.");
            }
            var add = await _repCombo.CreateCombo(comboImage);
            return Ok(add);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] Combo combo)
        {
            if (combo == null)
            {
                return BadRequest("Invalid data.");
            }
            await _repCombo.UpdateCombo(combo);
            return Ok(combo);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var comboId = await _repCombo.GetById(id);
            if (comboId == null)
            {
                _logger.LogWarning($"Không tìm thấy combo có ID {id}.");
                return NotFound($"Không tìm thấy combo có ID {id}.");
            }
            return Ok(comboId);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var comboId = await _repCombo.DeleteCombo(id);
            return Ok(comboId);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Tên không được để trống");
            }
            var combo = await _repCombo.GetByName(name);
            if (combo == null)
            {
                return NotFound($"Không tìm thấy combo có tên '{name}'");
            }
            return Ok(combo);
        }
    }
}