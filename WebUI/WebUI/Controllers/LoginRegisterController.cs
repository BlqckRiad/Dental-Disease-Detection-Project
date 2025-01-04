using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using WebUI.Models.ViewModels;

namespace WebUI.Controllers
{
    public class LoginRegisterController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public LoginRegisterController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var loginData = new
                {
                    emailOrTelNo = model.EmailOrPhone,
                    password = model.Password
                };

                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:7254/api/Login/UserLogin", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = true, data = responseContent });
                }
                else
                {
                    return Json(new { success = false, message = "Giriş başarısız. Lütfen bilgilerinizi kontrol edin." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:7254/api/Register/UserRegister", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                return Json(new { success = response.IsSuccessStatusCode, data = responseContent });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Kayıt işlemi sırasında bir hata oluştu: " + ex.Message });
            }
        }
    }
} 