namespace HaberPortali.API.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<NewsTag>? NewsTags { get; set; }
    }
}