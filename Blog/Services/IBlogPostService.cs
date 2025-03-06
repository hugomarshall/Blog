using Blog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services;
public interface IBlogPostService
{
    IEnumerable<BlogPost> GetAll();
    BlogPost? GetById(int id);
    BlogPost Add(BlogPost post);
    BlogPost Update(int id, BlogPost post);
    void Delete(int id);
    Comment AddComment(int postId, Comment comment);
}

public class BlogPostService : IBlogPostService
{
    private readonly BlogDbContext _context;

    public BlogPostService(BlogDbContext context)
    {
        _context = context;
    }

    public BlogPostService(List<BlogPost> posts)
    {
        if (_context is not null)
        {
            _context.Posts.AddRange(posts);
            _context.SaveChanges();
        }
    }

    public IEnumerable<BlogPost> GetAll()
    {
        return [.. _context.Posts.Include(x => x.Comments).AsNoTracking()];
    }

    public BlogPost? GetById(int id) => _context.Posts.Include(x => x.Comments).FirstOrDefault(p => p.Id == id);

    public BlogPost Add(BlogPost post)
    {
        post = new BlogPost(0, post.Title, post.Content);
        _context.Posts.Add(post);
        _context.SaveChanges();
        return post;
    }

    public BlogPost Update(int id, BlogPost post)
    {
        var existingPost = _context.Posts.FirstOrDefault(p => p.Id == id);
        if (existingPost == null)
        {
            throw new KeyNotFoundException("Post not found");
        }
        existingPost.Update(post.Title, post.Content);
        _context.Update(existingPost);
        _context.SaveChanges();
        return existingPost;
    }

    public void Delete(int id)
    {
        var post = _context.Posts.FirstOrDefault(p => p.Id == id);
        if (post == null)
        {
            throw new KeyNotFoundException("Post not found");
        }
        _context.Posts.Remove(post);
        _context.SaveChanges();
    }

    public Comment AddComment(int postId, Comment comment)
    {
        var post = _context.Posts.FirstOrDefault(p => p.Id == postId);
        if (post == null)
        {
            throw new KeyNotFoundException("Post not found");
        }
        post.AddComment(comment);
        _context.Posts.Update(post);
        _context.SaveChanges();
        return comment;
    }
}
