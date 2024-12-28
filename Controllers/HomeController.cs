using Hairdresser_Website.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Hairdresser_Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly HairstyleApiService _hairstyleApiService;
        private readonly IWebHostEnvironment _environment;

        // API anahtarını buraya doğrudan geçiyoruz
        public HomeController(IWebHostEnvironment environment)
        {
            _environment = environment;
            var apiKey = "lxTWfs8FzMAwzViOeUphJQAYkrIo4YfESJp50vSjWdc4htvmqRK6LkynGCuUEHND";  // API anahtarınızı burada yazın
            _hairstyleApiService = new HairstyleApiService(apiKey);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Appointment()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AIHair()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AIHair(IFormFile image, int hairType)
        {
            try
            {
                if (image != null && image.Length > 0)
                {
                    var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploads); // Klasör yoksa oluştur
                    var filePath = Path.Combine(uploads, image.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    // API çağrısı
                    var base64Image = await _hairstyleApiService.ChangeHairstyleAsync(filePath, hairType);

                    // Geçici dosyayı sil
                    System.IO.File.Delete(filePath);

                    return Json(new { success = true, image = base64Image });
                }
                else
                {
                    return Json(new { success = false, message = "Resim yüklenemedi veya geçersiz." });
                }
            }
            catch (Exception ex)
            {
                // Hata günlüğe yazılıyor
                Console.WriteLine($"Hata: {ex.Message}");
                return Json(new { success = false, message = "Sunucu tarafında bir hata oluştu: " + ex.Message });
            }
        }
    }
}
