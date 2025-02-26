namespace Blog.Domain
{
    public record Comment
    {
        public int BlogId { get; private set; }
        public string Author { get; private set; }
        public string Content { get; private set; }

        public Comment(int blogId, string author, string content)
        {
            if (string.IsNullOrWhiteSpace(author))
            {
                throw new ArgumentException("Author cannot be empty or whitespace.", nameof(author));
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Content cannot be empty or whitespace.", nameof(content));
            }

            BlogId = blogId;
            Author = author;
            Content = content;
        }
    }
}
