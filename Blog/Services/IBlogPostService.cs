using Blog.Domain;

namespace Blog.Services;
public interface IBlogPostService
{
    IEnumerable<BlogPost> GetAll();
    BlogPost GetById(int id);
    BlogPost Add(BlogPost post);
    BlogPost Update(int id, BlogPost post);
    void Delete(int id);
    Comment AddComment(int postId, Comment comment);
}

public class BlogPostService : IBlogPostService
{
    private readonly List<BlogPost> _posts;

    public BlogPostService()
    {
        _posts = new List<BlogPost>
        {
            new BlogPost(1, "First Post", "This is the first post."),
            new BlogPost(2, "Second Post", "This is the second post."),
            new BlogPost(3, "Third Post", "This is the third post.")
        };
    }

    public IEnumerable<BlogPost> GetAll() => _posts;

    public BlogPost GetById(int id) => _posts.FirstOrDefault(p => p.Id == id);

    public BlogPost Add(BlogPost post)
    {
        post = new BlogPost(_posts.Count + 1, post.Title, post.Content);
        _posts.Add(post);
        return post;
    }

    public BlogPost Update(int id, BlogPost post)
    {
        var existingPost = _posts.FirstOrDefault(p => p.Id == id);
        if (existingPost == null)
        {
            throw new KeyNotFoundException("Post not found");
        }
        existingPost.Update(post.Title, post.Content);
        return existingPost;
    }

    public void Delete(int id)
    {
        var post = _posts.FirstOrDefault(p => p.Id == id);
        if (post == null)
        {
            throw new KeyNotFoundException("Post not found");
        }
        _posts.Remove(post);
    }

    public Comment AddComment(int postId, Comment comment)
    {
        var post = _posts.FirstOrDefault(p => p.Id == postId);
        if (post == null)
        {
            throw new KeyNotFoundException("Post not found");
        }
        post.AddComment(comment);
        return comment;
    }
}
