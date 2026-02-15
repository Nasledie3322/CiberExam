using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Data;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _authService = authService;
            _userManager = userManager;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var token = await _authService.Register(request.Username, request.Password, request.Role);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _authService.Login(request.Username, request.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return Ok();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var link = $"https://yourapp/reset-password?email={dto.Email}&token={Uri.EscapeDataString(token)}";

            await _emailService.SendAsync(dto.Email, "Reset Password", $"Reset link: {link}");

            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return BadRequest(new { Error = "User not found" });

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);

            if (!result.Succeeded)
                return BadRequest(new { Error = string.Join(", ", result.Errors) });

            return Ok();
        }
    }
  
}
