using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistroEstudiantes.Server.DTOs;
using RegistroEstudiantes.Server.Services.Interfaces;
using System.Security.Claims;

namespace RegistroEstudiantes.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var (success, message) = await _authService.RegisterAsync(dto);

            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var response = await _authService.LoginAsync(dto);
            if (response == null)
                return Unauthorized(new { message = "Usuario o contraseña incorrectos. Por favor, inténtelo de nuevo." });

            return Ok(response);
        }

        [HttpPut("update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto dto)
        {
            // Extraemos el ID del usuario desde los Claims del JWT
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var (success, message) = await _authService.UpdateProfileAsync(userId, dto);

            if (!success) return BadRequest(new { message });

            return Ok(new { message });
        }

        [HttpDelete("delete-account")]
        [Authorize]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var (success, message) = await _authService.DeleteAccountAsync(userId);

            if (!success) return BadRequest(new { message });

            return Ok(new { message = "Cuenta eliminada exitosamente" });
        }
    }
}
