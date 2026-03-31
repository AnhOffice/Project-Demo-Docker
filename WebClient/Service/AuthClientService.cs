using WebClient.DTOs;

namespace WebClient.Service
{
    public class AuthClientService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthClientService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7062/api/auth/");
            _httpContextAccessor = httpContextAccessor;
        }

        ////Xử lý Login
        //public async Task<bool> LoginAsync(LoginDTOs dto)
        //{
        //    var response = await _httpClient.PostAsJsonAsync("login", dto);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        // Đọc kết quả JSON và parse trực tiếp sang model AuthResponse
        //        var authResult = await response.Content.ReadFromJsonAsync<AuthResponse>();

        //        if (authResult != null && !string.IsNullOrEmpty(authResult.Token))
        //        {
        //            var httpContext = _httpContextAccessor.HttpContext;

        //            if (httpContext != null && httpContext.Session != null)
        //            {
        //                // Lưu token vào Session
        //                httpContext.Session.SetString("authToken", authResult.Token);

        //                // Lưu Username vào Cookie
        //                httpContext.Response.Cookies.Append("Username", dto.UsernameOrEmail, new CookieOptions
        //                {
        //                    HttpOnly = false, // Có thể set true nếu không cần client JS đọc cookie
        //                    Secure = true, // Nên bật true trong production với HTTPS
        //                    SameSite = SameSiteMode.Strict
        //                });

        //                return true;
        //            }
        //        }
        //    }
        //    // Trường hợp login thất bại
        //    return false;
        //}

        public async Task<string> LoginAsync(LoginDTOs dto)
        {
            var response = await _httpClient.PostAsJsonAsync("login", dto);

            if (response.IsSuccessStatusCode)
            {
                var authResult = await response.Content.ReadFromJsonAsync<AuthResponse>();

                if (authResult != null && !string.IsNullOrEmpty(authResult.Token))
                {
                    var httpContext = _httpContextAccessor.HttpContext;

                    if (httpContext != null)
                    {
                        httpContext.Session?.SetString("authToken", authResult.Token);

                        httpContext.Response.Cookies.Append("Username", dto.UsernameOrEmail, new CookieOptions
                        {
                            HttpOnly = false,
                            Secure = true,
                            SameSite = SameSiteMode.Strict
                        });

                        return authResult.Token;
                    }
                }

                throw new Exception("Không thể xử lý phiên đăng nhập.");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorMessage>()
                            ?? new ErrorMessage { Message = "Tài khoản hoặc mật khẩu không chính xác." };
                throw new UnauthorizedAccessException(error.Message);
            }
            else
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorMessage>()
                            ?? new ErrorMessage { Message = "Lỗi hệ thống không xác định." };
                throw new Exception(error.Message);
            }
        }

        //Xử lý Logout
        public async Task<bool> LogoutAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                httpContext.Session.Remove("authToken");
                httpContext.Response.Cookies.Delete("Username");
            }
            return true;
        }

        //Xử lý Register
        public async Task<string> RegisterAsync(RegisterDTOs dto)
        {
            var response = await _httpClient.PostAsJsonAsync("register", dto);

            if (response.IsSuccessStatusCode)
            {
                return "Đăng ký thành công!";
            }
            else
            {
                // Đọc lỗi từ body (nếu có)
                var errorObj = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                return errorObj?.Error ?? "Đăng ký thất bại.";
            }
        }

    }

    // Model map với JSON lỗi backend
    public class ErrorResponse
    {
        public string Error { get; set; } = string.Empty;
    }

    // Kết quả trả về sau khi đăng nhập thành công
    public class AuthResponse
    {
        public string Token { get; set; }
    }

    public class ErrorMessage
    {
        public string Message { get; set; }
    }
}
