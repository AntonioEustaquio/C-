namespace BookOnTable.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int LatestRevision { get; set; }
        public string? Key { get; set; }
        public List<Author>? Authors { get; set; }
        public string? Type { get; set; } 
        public string? Synopsis { get; set; }
        public string? PublishDate { get; set; }
    }
}