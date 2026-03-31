using AuthAPI.DTOs;
using AuthAPI.Model;
using AuthAPI.Repository;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthAPI.Service
{
    public class AuthServices : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthServices(IAuthRepository authRepository, IMapper mapper, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        // xử lý tạo token
        public Task<string> GenerateJwtTokenAsync(Account user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("FullName", user.FullName)
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<string> LoginAsync(LoginDTOs dto)
        {
            var user = await _authRepository.GetByUsernameAsync(dto.UsernameOrEmail)
                         ?? await _authRepository.GetByEmailAsync(dto.UsernameOrEmail);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.PasswordHash, user.PasswordHash))
                throw new UnauthorizedAccessException("Sai tên đăng nhập hoặc mật khẩu chưa đúng.");

            var token = await GenerateJwtTokenAsync(user);
            return token;
        }

        public async Task<string> RegisterAsync(RegisterDTOs dto)
        {
            var existingUser = await _authRepository.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
                throw new InvalidOperationException("Tên đăng nhập đã tồn tại.");

            var existingEmail = await _authRepository.GetByEmailAsync(dto.Email);
            if (existingEmail != null)
                throw new InvalidOperationException("Email đã được sử dụng.");

            var user = _mapper.Map<Account>(dto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);
            user.Role = "User";

            await _authRepository.AddAsync(user);

            return "User registered successfully";
        }
    }
}
