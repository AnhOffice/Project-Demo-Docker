using Microsoft.AspNetCore.Mvc;
using WebClient.DTOs;
using WebClient.Service;

namespace WebClient.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly AuthClientService _authClientService;

        public AuthenticationController(AuthClientService authClientService)
        {
            _authClientService = authClientService;
        }
        // GET: AuthController/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: AuthController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTOs dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                var token = await _authClientService.LoginAsync(dto);
                return RedirectToAction("Index", "Home");
            }
            catch (UnauthorizedAccessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi hệ thống: " + ex.Message);
                return View(dto);
            }
        }




        // POST: AuthController/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authClientService.LogoutAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi khi đăng xuất: " + ex.Message);
                return View("Index");
            }
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        // POST: AuthController/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTOs dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                var result = await _authClientService.RegisterAsync(dto);

                if (result == "Đăng ký thành công!")
                {
                    TempData["SuccessMessage"] = result;
                    return RedirectToAction("Login");
                }
                else
                {
                    // result chứa thông báo lỗi từ API
                    ModelState.AddModelError("", result);
                    return View(dto);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi hệ thống: " + ex.Message);
                return View(dto);
            }
        }


    }
}
