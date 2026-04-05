namespace HaberPortali.API.Models
{
    public class SavedNews
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public AppUser? User { get; set; }

        public int NewsId { get; set; }
        public News? News { get; set; }

        public DateTime SavedAt { get; set; } = DateTime.Now;
    }
}