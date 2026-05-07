using HaberPortali.API.Models;
using HaberPortali.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HaberPortali.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsRepository _newsRepository;

        // İşçimizi (Repository) içeriye alıyoruz
        public NewsController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        // 1. Tüm Haberleri Getir (Kategori Detaylarıyla Beraber!)
        [HttpGet]
        public async Task<IActionResult> GetNews()
        {
            // İşte kendi yazdığın o özel metot çalışıyor!
            var news = await _newsRepository.GetAllNewsWithDetailsAsync();
            return Ok(news);
        }

        // 2. Tek Bir Haberi Detayıyla Getir
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNews(int id)
        {
            var news = await _newsRepository.GetNewsByIdWithDetailsAsync(id);
            if (news == null) return NotFound(new { message = "Haber bulunamadı!" });
            return Ok(news);
        }

        // 3. Yeni Haber Ekle (Fotoğraflı ve Güvenlikli)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNews([FromForm] string title, [FromForm] string content, [FromForm] int categoryId, IFormFile? image)
        {
            // 1. YAZAR KRİZİNİ ÇÖZÜYORUZ:
            // Token'dan giriş yapan kişinin ID'sini okumaya çalışıyoruz. 
            // Eğer bulamazsa, veritabanı patlamasın diye şimdilik 1 numaralı yazar (Admin) atıyoruz.
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int authorId = 1;
            if (int.TryParse(userIdString, out int parsedId))
            {
                authorId = parsedId;
            }

            // 2. FOTOĞRAF YÜKLEME İŞLEMİ:
            string? imageUrl = null;
            if (image != null && image.Length > 0)
            {
                // Fotoğrafları kaydedeceğimiz klasörü belirliyoruz (wwwroot/uploads)
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                // Dosya isimleri çakışmasın diye rastgele bir isim (Guid) üretiyoruz
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Fotoğrafı klasöre kopyalıyoruz
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                imageUrl = "/uploads/" + uniqueFileName; // Veritabanına yazılacak yol
            }

            // 3. HABERİ VERİTABANINA KAYDET
            var news = new News
            {
                Title = title,
                Content = content,
                CategoryId = categoryId,
                AuthorId = authorId, // <-- Artık veritabanı isyan etmeyecek!
                ImageUrl = imageUrl,
                CreatedAt = DateTime.Now,
                IsPublished = true // Haber direkt yayına girsin
            };

            await _newsRepository.AddAsync(news);
            await _newsRepository.SaveAsync();

            return Ok(new { message = "Haber ve fotoğraf başarıyla eklendi." });
        }

        // 4. Haberi Güncelle (Fotoğraf Destekli)
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateNews(int id, [FromForm] string title, [FromForm] string content, [FromForm] int categoryId, IFormFile? image)
        {
            // Önce güncellenecek haberi veritabanından buluyoruz
            var news = await _newsRepository.GetByIdAsync(id);
            if (news == null) return NotFound(new { message = "Güncellenecek haber bulunamadı!" });

            // Metin verilerini güncelliyoruz
            news.Title = title;
            news.Content = content;
            news.CategoryId = categoryId;
            news.UpdatedAt = DateTime.Now;

            // Eğer kullanıcı yeni bir fotoğraf seçtiyse eskiyi umursamayıp yenisini yüklüyoruz
            if (image != null && image.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                news.ImageUrl = "/uploads/" + uniqueFileName;
            }

            _newsRepository.Update(news);
            await _newsRepository.SaveAsync();

            return Ok(new { message = "Haber başarıyla güncellendi." });
        }

        // 5. Haberi Sil (Güvenlikli)
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteNews(int id)
        {
            // Önce sileceğimiz haberi buluyoruz
            var news = await _newsRepository.GetByIdAsync(id);
            if (news == null) return NotFound(new { message = "Silinecek haber bulunamadı!" });

            _newsRepository.Delete(news);
            await _newsRepository.SaveAsync();
            return Ok(new { message = "Haber başarıyla silindi." });
        }
    }
}