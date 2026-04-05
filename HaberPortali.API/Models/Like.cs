namespace HaberPortali.API.Models
{
    public class Like
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public AppUser? User { get; set; }

        public int NewsId { get; set; }
        public News? News { get; set; }
    }
}