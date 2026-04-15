using Microsoft.AspNetCore.Mvc;
using UserManagement.API.DTOs.Auth;
using UserManagement.API.Services;
using UserManagement.API.Utils;

namespace UserManagement.API.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;
        public AuthController(AuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _service.Login(request);
            return Ok(ApiResponse<AuthResponse>.Success(result));
        }
    }
}
