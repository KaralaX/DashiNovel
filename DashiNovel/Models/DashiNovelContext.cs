using Microsoft.EntityFrameworkCore;

namespace DashiNovel.Models
{
    public partial class DashiNovelContext : DbContext
    {
        public DashiNovelContext()
        {
        }

        public DashiNovelContext(DbContextOptions<DashiNovelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chapter> Chapters { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<Novel> Novels { get; set; } = null!;
        public virtual DbSet<Publish> Publishes { get; set; } = null!;
        public virtual DbSet<Reading> Readings { get; set; } = null!;
        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chapter>(entity =>
            {
                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.ReleaseDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Title).HasMaxLength(100);

                entity.HasOne(d => d.Novel)
                    .WithMany(p => p.Chapters)
                    .HasForeignKey(d => d.NovelId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Chapters__NovelI__37A5467C");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.Comment1)
                    .HasMaxLength(500)
                    .HasColumnName("Comment");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Chapter)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.ChapterId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Comments__Chapte__38996AB5");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Comments__UserId__3B75D760");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Novel>(entity =>
            {
                entity.Property(e => e.Author).HasMaxLength(100);

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.PublishDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Title).HasMaxLength(100);

                entity.HasMany(d => d.Genres)
                    .WithMany(p => p.Novels)
                    .UsingEntity<Dictionary<string, object>>(
                        "HasGenre",
                        l => l.HasOne<Genre>().WithMany().HasForeignKey("GenreId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__HasGenres__Genre__3C69FB99"),
                        r => r.HasOne<Novel>().WithMany().HasForeignKey("NovelId").HasConstraintName("FK__HasGenres__Novel__3B75D760"),
                        j =>
                        {
                            j.HasKey("NovelId", "GenreId").HasName("PK__HasGenre__9B603FC89B2C1E23");

                            j.ToTable("HasGenres");
                        });
            });

            modelBuilder.Entity<Publish>(entity =>
            {
                entity.ToTable("Publish");

                entity.HasOne(d => d.Novel)
                    .WithMany(p => p.Publishes)
                    .HasForeignKey(d => d.NovelId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Publish__NovelId__3C69FB99");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Publishes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Publish__UserId__3F466844");
            });

            modelBuilder.Entity<Reading>(entity =>
            {
                entity.HasKey(e => new { e.NovelId, e.UserId });

                entity.ToTable("Reading");

                entity.HasOne(d => d.Novel)
                    .WithMany(p => p.Readings)
                    .HasForeignKey(d => d.NovelId)
                    .HasConstraintName("FK_Reading_Novels");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Readings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Reading_Users");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => new { e.NovelId, e.UserId })
                    .HasName("PK__Reviews__BA20E35BB8529C41");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Review1)
                    .HasMaxLength(500)
                    .HasColumnName("Review");

                entity.HasOne(d => d.Novel)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.NovelId)
                    .HasConstraintName("FK__Reviews__NovelId__3E52440B");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reviews__UserId__4316F928");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
