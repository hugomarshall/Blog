using System.ComponentModel;

namespace Blog.Domain
{
    public record Comment
    {
        [Description("Post ID.")]
        public int PostId { get; private set; }
        [Description("Comment Author.")]
        public string Author { get; private set; }
        [Description("Comment Content.")]
        public string Content { get; private set; }

        public Comment(int postId, string author, string content)
        {
            if (string.IsNullOrWhiteSpace(author))
            {
                throw new ArgumentException("Author cannot be empty or whitespace.", nameof(author));
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Content cannot be empty or whitespace.", nameof(content));
            }

            PostId = postId;
            Author = author;
            Content = content;
        }
    }
}
