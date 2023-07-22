namespace DashiNovel.Models
{
    public partial class Reading
    {
        public int NovelId { get; set; }
        public int UserId { get; set; }
        public int ChapterNumber { get; set; }

        public virtual Novel Novel { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
