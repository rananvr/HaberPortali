using HaberPortali.API.Models;
using HaberPortali.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HaberPortali.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Sadece giriş yapmış yetkililer buraya girebilir
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

        // 2. Yorumu Onayla veya Onayını Kaldır (Toggle)
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> ToggleApproval(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null) return NotFound(new { message = "Yorum bulunamadı!" });

            // True ise False, False ise True yap
            comment.IsApproved = !comment.IsApproved;

            _commentRepository.Update(comment);
            await _commentRepository.SaveAsync();

            string status = comment.IsApproved ? "onaylandı" : "gizlendi";
            return Ok(new { message = $"Yorum başarıyla {status}." });
        }

        // 3. Yorumu Tamamen Sil
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
}