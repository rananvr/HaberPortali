using HaberPortali.API.Data;
using HaberPortali.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaberPortali.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = new
            {
                totalNews = await _context.News.CountAsync(),
                totalCategories = await _context.Categories.CountAsync(),
                totalUsers = await _context.Users.CountAsync(),
                pendingComments = await _context.Comments.CountAsync(c => !c.IsApproved) 
            };

            return Ok(stats);
        }
    }
}