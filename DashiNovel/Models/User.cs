namespace DashiNovel.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            Publishes = new HashSet<Publish>();
            Readings = new HashSet<Reading>();
            Reviews = new HashSet<Review>();
        }

        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? UserName { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Publish> Publishes { get; set; }
        public virtual ICollection<Reading> Readings { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
