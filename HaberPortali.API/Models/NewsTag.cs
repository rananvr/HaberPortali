namespace HaberPortali.API.Models
{
    public class NewsTag
    {
        public int Id { get; set; }

        public int NewsId { get; set; }
        public News? News { get; set; }

        public int TagId { get; set; }
        public Tag? Tag { get; set; }
    }
}