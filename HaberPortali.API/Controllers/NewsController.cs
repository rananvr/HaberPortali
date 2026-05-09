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

        public NewsController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetNews()
        {
            var news = await _newsRepository.GetAllNewsWithDetailsAsync();
            return Ok(news);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNews(int id)
        {
            var news = await _newsRepository.GetNewsByIdWithDetailsAsync(id);
            if (news == null) return NotFound(new { message = "Haber bulunamadı!" });
            return Ok(news);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNews([FromForm] string title, [FromForm] string content, [FromForm] int categoryId, IFormFile? image)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int authorId = 1;
            if (int.TryParse(userIdString, out int parsedId))
            {
                authorId = parsedId;
            }

            string? imageUrl = null;
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

                imageUrl = "/uploads/" + uniqueFileName; 
            }

            var news = new News
            {
                Title = title,
                Content = content,
                CategoryId = categoryId,
                AuthorId = authorId, 
                ImageUrl = imageUrl,
                CreatedAt = DateTime.Now,
                IsPublished = true 
            };

            await _newsRepository.AddAsync(news);
            await _newsRepository.SaveAsync();

            return Ok(new { message = "Haber ve fotoğraf başarıyla eklendi." });
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateNews(int id, [FromForm] string title, [FromForm] string content, [FromForm] int categoryId, IFormFile? image)
        {
            var news = await _newsRepository.GetByIdAsync(id);
            if (news == null) return NotFound(new { message = "Güncellenecek haber bulunamadı!" });

            news.Title = title;
            news.Content = content;
            news.CategoryId = categoryId;
            news.UpdatedAt = DateTime.Now;

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

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var news = await _newsRepository.GetByIdAsync(id);
            if (news == null) return NotFound(new { message = "Silinecek haber bulunamadı!" });

            _newsRepository.Delete(news);
            await _newsRepository.SaveAsync();
            return Ok(new { message = "Haber başarıyla silindi." });
        }
    }
}