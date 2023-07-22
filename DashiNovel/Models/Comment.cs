namespace DashiNovel.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int? ChapterId { get; set; }
        public int? UserId { get; set; }
        public DateTime Date { get; set; }
        public string Comment1 { get; set; } = null!;

        public virtual Chapter? Chapter { get; set; }
        public virtual User? User { get; set; }
    }
}
