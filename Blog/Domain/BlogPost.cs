using System.ComponentModel;

namespace Blog.Domain
{
    public record BlogPost
    {
        [Description("The post ID.")]

        public int Id { get; private set; }
        [Description("Post Title.")]
        public string Title { get; private set; }
        [Description("Post Content.")]
        public string Content { get; private set; }
        [Description("Post Comments.")]
        public List<Comment> Comments { get; private set; } = [];

        public BlogPost(int id, string title, string content)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be empty or whitespace.", nameof(title));
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Content cannot be empty or whitespace.", nameof(content));
            }

            Id = id;
            Title = title;
            Content = content;
        }

        public void Update(string title, string content)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be empty or whitespace.", nameof(title));
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Content cannot be empty or whitespace.", nameof(content));
            }

            Title = title;
            Content = content;
        }

        public void AddComment(Comment comment)
        {
            if (Comments.Any(c => c.Content == comment.Content))
            {
                throw new InvalidOperationException("A comment with the same content already exists.");
            }

            Comments.Add(comment);
        }
    }
}
