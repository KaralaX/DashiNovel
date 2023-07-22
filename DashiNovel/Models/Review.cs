namespace DashiNovel.Models
{
    public partial class Review
    {
        public int NovelId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string Review1 { get; set; } = null!;
        public bool Rate { get; set; }

        public virtual Novel Novel { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
