using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using UserLoginRegisterService.BusinessLayer.Abstract;
using UserLoginRegisterService.DtoLayer.Dtos;
using UserLoginRegisterService.EntityLayer.Concrete;

namespace UserLoginRegisterService.API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[AllowAnonymous]
	public class RegisterController : ControllerBase
	{
		private readonly IUserService _userService;
        public RegisterController(IUserService userService)
        {
            _userService = userService;
        }
		[HttpPost]
		public IActionResult UserRegister(UserRegisterDto model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Model Yapıya Uygun Değildir.");
			}

			// Mevcut kullanıcıları getir
			var existingUsers = _userService.TGetList();

			// E-posta ve telefon numarasını kontrol et
			if (existingUsers.Any(u => u.UserEmail == model.UserEmail))
			{
				return BadRequest("E-posta adresi zaten kullanılıyor.");
			}

			if (existingUsers.Any(u => u.UserTelNo == model.UserTelNo))
			{
				return BadRequest("Telefon numarası zaten kullanılıyor.");
			}

			// Yeni kullanıcı oluştur
			var user = new User
			{
				Name = model.Name,
				SurName = model.SurName,
				UserEmail = model.UserEmail,
				UserTelNo = model.UserTelNo,
				UserName = model.UserName,
				CreatedDate = DateTime.Now,
				CreatedUserID = 0,
				UserRoleID = 1 // Rol Entitysinde 1 Değeri Daima Normal User Olarak Atanacaktır.
			};

			// Şifre kontrolü ve hash işlemi
			if (string.IsNullOrEmpty(model.Password))
			{
				return BadRequest("Password is null");
			}

			using (SHA256 sha = SHA256.Create())
			{
				string hashedPassword = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(model.Password)));
				user.Password = hashedPassword;
			}

			// Kullanıcıyı kaydet
			_userService.TAdd(user);

			return Ok();
		}

	}
}
