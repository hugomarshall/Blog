using Blog.Domain;
using Blog.Services;

namespace Tests
{
    public class BlogPostServiceUnityTests
    {
        private readonly BlogPostService _service;
        private readonly List<BlogPost> _posts;

        public BlogPostServiceUnityTests()
        {
            _posts = new List<BlogPost>
        {
            new BlogPost(1, "Test - First Post", "This is the first test post."),
            new BlogPost(2, "Test - Second Post", "This is the second test post."),
            new BlogPost(3, "Test - Third Post", "This is the third test post.")
        };
            _service = new BlogPostService(_posts);
        }

        [Fact]
        public void GetAll_ReturnsAllPosts()
        {
            var result = _service.GetAll();
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetById_ReturnsCorrectPost()
        {
            var result = _service.GetById(1);
            Assert.NotNull(result);
            Assert.Equal("Test - First Post", result.Title);
        }

        [Fact]
        public void Add_AddsNewPost()
        {
            var newPost = new BlogPost(4, "New Post", "This is a new post.");
            var result = _service.Add(newPost);
            Assert.Equal(4, _posts.Count);
            Assert.Equal("New Post", result.Title);
        }

        [Fact]
        public void Update_UpdatesExistingPost()
        {
            var updatedPost = new BlogPost(1, "Updated Post", "This is an updated post.");
            var result = _service.Update(1, updatedPost);
            Assert.Equal("Updated Post", result.Title);
        }

        [Fact]
        public void Delete_RemovesPost()
        {
            _service.Delete(1);
            Assert.Equal(2, _posts.Count);
        }

        [Fact]
        public void AddComment_AddsCommentToPost()
        {
            var comment = new Comment(1, "Author", "This is a comment.");
            var result = _service.AddComment(1, comment);
            Assert.Single(_posts.First(p => p.Id == 1).Comments);
        }
    }
}
