using Blog.Domain;
using Blog.Services;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    public class BlogPostServiceUnityTests
    {
        private readonly BlogPostService _service;
        private readonly BlogDbContext _context;
        private BlogPost _post { get; set; }

        public BlogPostServiceUnityTests()
        {
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(databaseName: "BlogDb")
                .Options;

            _context = new BlogDbContext(options);
            _service = new BlogPostService(_context);

            // Seed the database with initial data
            _context.Posts.AddRange(new List<BlogPost>
            {
                new BlogPost(0, "Test - First Post", "This is the first test post."),
                new BlogPost(0, "Test - Second Post", "This is the second test post."),
                new BlogPost(0, "Test - Third Post", "This is the third test post.")
            });
            _context.SaveChanges();
            _post = _context.Posts.First();
        }

        [Fact, TestPriority(1)]
        public void GetAll_ReturnsAllPosts()
        {
            var result = _service.GetAll();
            Assert.True(result.Any());
        }

        [Fact, TestPriority(2)]
        public void Add_AddsNewPost()
        {
            var newPost = new BlogPost(0, "New Post", "This is a new post.");
            _post = _service.Add(newPost);
            Assert.True(_post.Id > 0);
        }

        [Fact, TestPriority(3)]
        public void GetById_ReturnsCorrectPost()
        {
            var result = _service.GetById(_post.Id);
            Assert.NotNull(result);
            Assert.Equal(_post.Id, result.Id);
        }

        [Fact]
        public void Update_UpdatesExistingPost()
        {
            BlogPost updatedPost = new(_post.Id, "Updated Post", "This is an updated post.");
            var result = _service.Update(_post.Id, updatedPost);
            Assert.Equal(_post.Title, result.Title);
        }

        [Fact]
        public void Delete_RemovesPost()
        {
            _service.Delete(_post.Id);
            var result = _service.GetById(_post.Id);
            Assert.True(result is null);
        }

        [Fact]
        public void AddComment_AddsCommentToPost()
        {
            var comment = new Comment(1, "Author", "This is a comment.");
            var result = _service.AddComment(1, comment);
            Assert.Single(_context.Posts.First(p => p.Id == 1).Comments);
        }
    }
}
