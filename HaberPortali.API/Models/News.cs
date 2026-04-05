namespace HaberPortali.API.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }

       // category 
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        
        public int AuthorId { get; set; }
        public AppUser? Author { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public bool IsPublished { get; set; } = false;
        public int ViewCount { get; set; } = 0;

        
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<NewsTag>? NewsTags { get; set; }
        public ICollection<SavedNews>? SavedNews { get; set; }
    }
}