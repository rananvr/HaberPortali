using HaberPortali.API.DTOs;
using HaberPortali.API.Models;
using HaberPortali.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<IActionResult> GetAllNews()
        {
            var news = await _newsRepository.GetAllNewsWithDetailsAsync();
            return Ok(news);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNewsById(int id)
        {
            var news = await _newsRepository.GetNewsByIdWithDetailsAsync(id);
            if (news == null) return NotFound(new { message = "Haber bulunamadı!" });
            return Ok(news);
        }

        //admin user
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNews([FromBody] NewsCreateDto dto)
        {
            //oto token
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

            var newArticle = new News
            {
                Title = dto.Title,
                Content = dto.Content,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId,
                AuthorId = int.Parse(userIdStr),
                IsPublished = true
            };

            await _newsRepository.AddAsync(newArticle);
            await _newsRepository.SaveAsync();
            return Ok(new { message = "Haber başarıyla eklendi." });
        }

        //admin 
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var news = await _newsRepository.GetByIdAsync(id);
            if (news == null) return NotFound();

            _newsRepository.Delete(news);
            await _newsRepository.SaveAsync();
            return Ok(new { message = "Haber silindi." });
        }
    }
}