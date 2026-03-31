using AuthAPI.DTOs;
using AuthAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authservice;

        public AuthController(IAuthService authservice)
        {
            _authservice = authservice;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTOs dto)
        {
            try
            {
                var result = await _authservice.RegisterAsync(dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });

            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTOs dto)
        {
            try
            {
                var token = await _authservice.LoginAsync(dto);

                return Ok(new { Token = token }); // Trả về token cho client
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi hệ thống.", Detail = ex.Message });
            }
        }
    }
}
