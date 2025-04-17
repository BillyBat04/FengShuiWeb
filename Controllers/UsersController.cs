using FengShuiWeb.Data;
using FengShuiWeb.DTOs;
using FengShuiWeb.Models;
using FengShuiWeb.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] UserDTO dto)
    {
        _service.RegisterUser(dto);
        return Ok("Đăng ký thành công");
    }

    [HttpGet("lucky-direction")]
    public IActionResult GetLuckyDirection([FromQuery] string gender, [FromQuery] DateTime birthDate)
    {
        var result = _service.GetLuckyDirection(gender, birthDate);
        return Ok(result);
    }
}