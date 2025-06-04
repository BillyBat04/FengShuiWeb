using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FengShuiWeb.Application;
using FengShuiWeb.Application.DTOs;
using FengShuiWeb.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;

namespace FengShuiWeb.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);
                var profile = await _userService.GetOwnProfileAsync(userId);
                return Ok(profile);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi lấy thông tin hồ sơ" });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);
                await _userService.UpdateOwnProfileAsync(userId, dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi cập nhật hồ sơ" });
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);
                await _userService.ChangeOwnPasswordAsync(userId, dto);
                return NoContent();
            }
            catch (AuthenticationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi thay đổi mật khẩu" });
            }
        }
    }
}