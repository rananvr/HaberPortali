namespace HaberPortali.API.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // bir kategorinin birden fazla haberi olabilir
        public ICollection<News>? News { get; set; }
    }
}