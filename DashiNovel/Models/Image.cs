namespace DashiNovel.Models
{
    public partial class Image
    {
        public int NovelId { get; set; }
        public byte[]? Data { get; set; }

        public virtual Novel Novel { get; set; } = null!;
    }
}
