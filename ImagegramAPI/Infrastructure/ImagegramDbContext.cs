using ImagegramAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace ImagegramAPI.Infrastructure
{
    public class ImagegramDbContext : DbContext
    {
        public ImagegramDbContext(DbContextOptions<ImagegramDbContext> options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Account>()
                .HasMany(p => p.Comments)
                .WithOne(q => q.Creator)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Account>()
               .HasMany(p => p.Posts)
               .WithOne(q => q.Creator)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Account>()
            .HasData(new { Id = (long)1 , Name = "Admin"});

        }
    }
}
