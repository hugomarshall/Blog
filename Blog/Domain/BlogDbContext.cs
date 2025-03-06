using Microsoft.EntityFrameworkCore;

namespace Blog.Domain;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

    public DbSet<BlogPost> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
}
