namespace DashiNovel.Models
{
    public partial class Novel
    {
        public Novel()
        {
            Chapters = new HashSet<Chapter>();
            Publishes = new HashSet<Publish>();
            Readings = new HashSet<Reading>();
            Reviews = new HashSet<Review>();
            Genres = new HashSet<Genre>();
        }

        public int NovelId { get; set; }
        public string Title { get; set; } = null!;
        public byte[]? Image { get; set; }
        public string Author { get; set; } = null!;
        public bool Type { get; set; }
        public string Description { get; set; } = null!;
        public bool State { get; set; }
        public DateTime PublishDate { get; set; }

        public virtual ICollection<Chapter> Chapters { get; set; }
        public virtual ICollection<Publish> Publishes { get; set; }
        public virtual ICollection<Reading> Readings { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<Genre> Genres { get; set; }
    }
}
