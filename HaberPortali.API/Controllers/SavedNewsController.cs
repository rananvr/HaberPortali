using HaberPortali.API.Data;
using HaberPortali.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HaberPortali.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class SavedNewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SavedNewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("status/{newsId}")]
        public async Task<IActionResult> GetStatus(int newsId)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return Unauthorized();

            var isSaved = await _context.SavedNews.AnyAsync(s => s.NewsId == newsId && s.UserId == userId);
            return Ok(new { isSaved });
        }


        [HttpPost("toggle/{newsId}")]
        public async Task<IActionResult> ToggleSavedNews(int newsId)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return Unauthorized();

            var existing = await _context.SavedNews
                .FirstOrDefaultAsync(s => s.NewsId == newsId && s.UserId == userId);

            if (existing != null)
            {
                _context.SavedNews.Remove(existing);
                await _context.SaveChangesAsync();
                return Ok(new { isSaved = false, message = "Kayıt kaldırıldı." });
            }
            else
            {
                var newSaved = new SavedNews { NewsId = newsId, UserId = userId, SavedAt = DateTime.Now };
                _context.SavedNews.Add(newSaved);
                await _context.SaveChangesAsync();
                return Ok(new { isSaved = true, message = "Haber kaydedildi." });
            }

        }
        [HttpGet("my-list")]
        public async Task<IActionResult> GetMySavedNews()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return Unauthorized();

            var savedNews = await _context.SavedNews
                .Include(s => s.News)
                .ThenInclude(n => n.Category) 
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.SavedAt)
                .Select(s => new {
                    id = s.News.Id,
                    title = s.News.Title,
                    imageUrl = s.News.ImageUrl,
                    categoryName = s.News.Category.Name,
                    savedAt = s.SavedAt
                })
                .ToListAsync();

            return Ok(savedNews);
        }
    }
}