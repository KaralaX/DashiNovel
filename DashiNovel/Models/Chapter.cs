namespace DashiNovel.Models
{
    public partial class Chapter
    {
        public Chapter()
        {
            Comments = new HashSet<Comment>();
        }

        public int ChapterId { get; set; }
        public int? NovelId { get; set; }
        public int ChapterNumber { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }

        public virtual Novel? Novel { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
