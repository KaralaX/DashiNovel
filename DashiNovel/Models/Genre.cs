namespace DashiNovel.Models
{
    public partial class Genre
    {
        public Genre()
        {
            Novels = new HashSet<Novel>();
        }

        public int GenreId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Novel> Novels { get; set; }
    }
}
