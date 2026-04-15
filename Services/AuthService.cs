using Microsoft.EntityFrameworkCore;
using UserManagement.API.Data;
using UserManagement.API.DTOs.Auth;
using UserManagement.API.Exceptions;
using UserManagement.API.Utils;

namespace UserManagement.API.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenGenerator _tokenGenerator;

        public AuthService(AppDbContext context, JwtTokenGenerator tokenGenerator)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<AuthResponse> Login(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == request.Email && !x.IsDeleted);

            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new BadRequestException("Credencias inválidas.");

            var token = _tokenGenerator.Generate(user);

            return new AuthResponse
            {
                Token = token,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}
