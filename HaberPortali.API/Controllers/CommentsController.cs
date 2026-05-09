using HaberPortali.API.Models;
using HaberPortali.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HaberPortali.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentsController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        // 1. Tüm Yorumları Getir
        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            var comments = await _commentRepository.GetAllCommentsWithDetailsAsync();
            return Ok(comments);
        }
        [HttpGet("news/{newsId}")]
        [AllowAnonymous] 
        public async Task<IActionResult> GetCommentsByNewsId(int newsId)
        {
            var allComments = await _commentRepository.GetAllCommentsWithDetailsAsync();

            var filteredComments = allComments
                .Where(c => c.NewsId == newsId && c.IsApproved == true)
                .Select(c => new {
                    id = c.Id,
                    content = c.Content,
                    createdAt = c.CreatedAt,
                    userName = c.User.UserName 
                })
                .ToList();

            return Ok(filteredComments);
        }
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentDto commentDto)
        {
            if (commentDto == null || string.IsNullOrWhiteSpace(commentDto.Text))
            {
                return BadRequest(new { message = "Yorum metni boş olamaz!" });
            }


            // Token id
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // int değeri
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized(new { message = "Kullanıcı kimliği okunamadı. Lütfen çıkış yapıp tekrar giriş yapın." });
            }

            var newComment = new Comment
            {
                NewsId = commentDto.NewsId,
                Content = commentDto.Text,

                UserId = userId, 

                IsApproved = true,
                CreatedAt = DateTime.Now
            };

            await _commentRepository.AddAsync(newComment);
            await _commentRepository.SaveAsync();

            return Ok(new { message = "Yorum başarıyla gönderildi ve onay sırasına alındı!" });
        }

        // TOGGLE: Yorumun Onay Durumunu Değiştir (Admin için)
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> ToggleApproval(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null) return NotFound(new { message = "Yorum bulunamadı!" });

            comment.IsApproved = !comment.IsApproved;

            _commentRepository.Update(comment);
            await _commentRepository.SaveAsync();

            string status = comment.IsApproved ? "onaylandı" : "gizlendi";
            return Ok(new { message = $"Yorum başarıyla {status}." });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateCommentDto updateDto)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null) return NotFound(new { message = "Yorum bulunamadı!" });

            comment.Content = updateDto.Text;

            _commentRepository.Update(comment);
            await _commentRepository.SaveAsync();

            return Ok(new { message = "Yorum başarıyla güncellendi." });
        }

        // Yorum Sil (Admin için)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null) return NotFound(new { message = "Silinecek yorum bulunamadı!" });

            _commentRepository.Delete(comment);
            await _commentRepository.SaveAsync();

            return Ok(new { message = "Yorum başarıyla silindi." });
        }
    }

    public class CreateCommentDto
    {
        public int NewsId { get; set; }
        public string Text { get; set; }
    }
    public class UpdateCommentDto
    {
        public string Text { get; set; }
    }

}