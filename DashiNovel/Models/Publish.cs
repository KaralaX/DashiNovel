namespace DashiNovel.Models
{
    public partial class Publish
    {
        public int PublishId { get; set; }
        public int? UserId { get; set; }
        public int? NovelId { get; set; }

        public virtual Novel? Novel { get; set; }
        public virtual User? User { get; set; }
    }
}
