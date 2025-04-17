using FengShuiWeb.DTOs;
using FengShuiWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace FengShuiWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompatibilityController : ControllerBase
    {
        private readonly ICompatibilityService _service;

        public CompatibilityController(ICompatibilityService service)
        {
            _service = service;
        }

        [HttpPost("check")]
        public IActionResult Check([FromBody] CompatibilityDTO dto)
        {
            var result = _service.EvaluateCompatibility(dto.PersonA, dto.PersonB);
            return Ok(result);
        }
    }
}
