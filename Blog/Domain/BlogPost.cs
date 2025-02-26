namespace Blog.Domain
{
    public record BlogPost
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public List<Comment> Comments { get; private set; } = new List<Comment>();

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
