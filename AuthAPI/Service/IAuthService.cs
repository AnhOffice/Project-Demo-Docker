using AuthAPI.DTOs;
using AuthAPI.Model;

namespace AuthAPI.Service
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDTOs dto);
        Task<string> LoginAsync(LoginDTOs dto);
        Task<string> GenerateJwtTokenAsync(Account user);
    }
}
