using FengShuiWeb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FengShuiWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZodiacController : ControllerBase
    {
        private readonly IZodiacRepository _repo;

        public ZodiacController(IZodiacRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("{sign}")]
        public IActionResult Get(string sign)
        {
            var data = _repo.GetRecommendation(sign);
            if (data == null) return NotFound();
            return Ok(data);
        }
    }
}
