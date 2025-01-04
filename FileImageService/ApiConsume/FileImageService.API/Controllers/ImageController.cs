using FileImageService.BusinessLayer.Abstract;
using FileImageService.DtoLayer.Dtos;
using FileImageService.DtoLayer.Dtos.Delete;
using FileImageService.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileImageService.API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ImageController : ControllerBase
	{
		public IImageService _imageService;
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }
		[HttpPost]
		public async Task<IActionResult> AddImage(IFormFile file)
		{
			// Dosya geçersizse hata döndür
			if (file == null || file.Length == 0)
			{
				return BadRequest("Geçersiz dosya.");
			}

			// Dosyanın bir resim olup olmadığını kontrol et
			var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
			var extension = Path.GetExtension(file.FileName).ToLower();

			if (!allowedExtensions.Contains(extension))
			{
				return BadRequest("Yalnızca resim dosyaları yüklenebilir.");
			}

			// Resmi kaydetmek için klasör oluştur
			var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images");

			if (!Directory.Exists(imagePath))
			{
				Directory.CreateDirectory(imagePath); // Klasör yoksa oluştur
			}

			// Benzersiz dosya adı oluştur
			var uniqueFileName = Guid.NewGuid().ToString() + extension;
			var fullPath = Path.Combine(imagePath, uniqueFileName);

			// Dosyayı hedef klasöre kaydet
			using (var stream = new FileStream(fullPath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			// Yeni dosya bilgilerini oluştur
			Images newfile = new Images
			{
				ImageName = uniqueFileName, // Orijinal dosya adı
				CreatedDate = DateTime.Now,
				ImageUrl = $"/images/{uniqueFileName}",// Uygulamanın dışarıya açık URL'si
				CreatedUserID = 0,
			};
			
			_imageService.TAdd(newfile);

			int imageId = newfile.ImageID;

			ImageRequestDto request = new ImageRequestDto
			{
				ImageUrl = $"https://localhost:7014/images/{uniqueFileName}",
				ImageName = uniqueFileName,
				ImageID = imageId,
			};

			// Başarılı yanıt
			return Ok(request);
		}
		[HttpPost]
		public IActionResult DeleteImage(DeleteDto model)
		{

			var image = _imageService.TGetByid(model.DeletedItemID);
			if(image == null)
			{
				return BadRequest("Image is not found");
			}
			image.IsDeleted = true;
			image.DeletedDate = DateTime.Now;
			image.DeletedUserID = model.DeletedUserID;
			_imageService.TUpdate(image);
			return Ok();
		}
		[HttpGet]
		public IActionResult GetAllImages ()
		{
			var images = _imageService.TGetList().Where(x=> x.IsDeleted ==false );
			return Ok(images);
		}
	}
}
