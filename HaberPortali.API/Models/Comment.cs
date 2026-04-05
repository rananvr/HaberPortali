namespace HaberPortali.API.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;

        public int UserId { get; set; }
        public AppUser? User { get; set; }

        public int NewsId { get; set; }
        public News? News { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsApproved { get; set; } = false; // yöneticinin onayı
    }
}