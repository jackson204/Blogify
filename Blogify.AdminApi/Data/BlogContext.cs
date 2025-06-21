using Microsoft.EntityFrameworkCore;
using Blogify.AdminApi.Models;

namespace Blogify.AdminApi.Data
{
    public class BlogContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseInMemoryDatabase("BlogDb");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>()
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(a => a.CategoryId);
        }
    }
}
