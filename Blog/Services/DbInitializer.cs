using Blog.Domain;

namespace Blog.Services
{
    public static class DbInitializer
    {
        public static void Initialize(BlogDbContext context)
        {
            // Ensure the database is created
            context.Database.EnsureCreated();

            // Check if there are any BlogPosts already in the database
            if (context.Posts.Any())
            {
                return;   // DB has been seeded
            }

            var posts = new BlogPost[]
            {
                new BlogPost(1, "First Post", "This is the first post."),
                new BlogPost(2, "Second Post", "This is the second post."),
                new BlogPost(3, "Third Post", "This is the third post.")
            };

            foreach (var post in posts)
            {
                context.Posts.Add(post);
            }
            context.SaveChanges();

            var comments = new Comment[]
            {
                new Comment(1, "Author1", "This is a comment on the first post."),
                new Comment(2, "Author2", "This is a comment on the second post."),
                new Comment(3, "Author3", "This is a comment on the third post.")
            };

            foreach (var comment in comments)
            {
                context.Comments.Add(comment);
            }
            context.SaveChanges();
        }
    }
}
