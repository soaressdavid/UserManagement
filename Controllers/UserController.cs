using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.API.DTOs.User;
using UserManagement.API.Services;
using UserManagement.API.Utils;

namespace UserManagement.API.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserRequest request)
        {
            await _service.Create(request);
            return Ok(ApiResponse<string>.Success("Usuário cadastrado."));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10, string search = "")
        {
            var results = await _service.GetAll(page, pageSize, search);
            return Ok(ApiResponse<PagedResult<UserResponse>>.Success(results));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetById(id);
            return Ok(ApiResponse<UserResponse>.Success(result));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserRequest request)
        {
            await _service.Update(id, request);
            return Ok(ApiResponse<string>.Success("Usuário atualizado"));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.Delete(id);
            return Ok(ApiResponse<string>.Success("Usuário deletado"));
        }
    }
}
