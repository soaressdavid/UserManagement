using Microsoft.EntityFrameworkCore;
using UserManagement.API.Data;
using UserManagement.API.DTOs.User;
using UserManagement.API.Entities;
using UserManagement.API.Exceptions;
using UserManagement.API.Utils;

namespace UserManagement.API.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserService> _logger;
        public UserService(AppDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Create(CreateUserRequest request)
        {
            if (await _context.Users.AnyAsync(x => x.Email == request.Email))
                throw new BadRequestException("Email já existe.");

            var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User(
                    request.Name,
                    request.Email,
                    hash
                );

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Usuário criado: {Email}", user.Email);
        }

        public async Task<PagedResult<UserResponse>> GetAll(int page, int pageSize, string search)
        {
            var query = _context.Users.Where(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.Contains(search) || x.Email.Contains(search));

            var total = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new UserResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Role = x.Role,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

            return new PagedResult<UserResponse>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalPages = total
            };
        }

        public async Task<UserResponse> GetById(Guid id)
        {
            var user = await _context.Users
                .Where(x => x.Id == id && !x.IsDeleted)
                .Select(x => new UserResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Role = x.Role,
                    CreatedAt = x.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (user is null)
                throw new NotFoundException("Usuário não encontrado.");

            return user;
        }

        public async Task Update(Guid id, UpdateUserRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (user is null)
                throw new NotFoundException("Usuário não encontrado.");

            if (!string.IsNullOrWhiteSpace(request.Name))
                user.SetName(request.Name);

            if (!string.IsNullOrWhiteSpace(request.Email))
                user.SetEmail(request.Email);

            if (!string.IsNullOrWhiteSpace(request.Password))
                user.SetPassword(BCrypt.Net.BCrypt.HashPassword(request.Password));

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Usuário atualido: {Id}", user.Id);

        }

        public async Task Delete(Guid id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (user is null)
                throw new NotFoundException("Usuário não encontrado.");

            user.SoftDelete();
            await _context.SaveChangesAsync();

            _logger.LogInformation("Usuário deletado: {Id}", user.Id);
        }
    }
}
