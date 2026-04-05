using Microsoft.AspNetCore.Identity;

namespace HaberPortali.API.Models
{
    public class AppUser : IdentityUser<int>
    {
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        
        public ICollection<News>? News { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<SavedNews>? SavedNews { get; set; }
    }
}