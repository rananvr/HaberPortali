using HaberPortali.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaberPortali.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users
                .Select(u => new {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.CreatedAt,
                    u.IsActive
                })
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            return Ok(users);
        }

        // Kullanıcıyı engelle
        [HttpPut("toggle-status/{id}")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound(new { message = "Kullanıcı bulunamadı!" });

            user.IsActive = !user.IsActive;
            await _userManager.UpdateAsync(user);

            string statusMessage = user.IsActive ? "Aktif edildi" : "Pasif duruma getirildi (Banlandı)";
            return Ok(new { message = $"Kullanıcı başarıyla {statusMessage}." });
        }
    }
}