using Blog.Domain;
using Blog.Services;

namespace Blog.Routes;

public static class BlogPostRoutes
{
    public static void MapBlogPostRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/posts/", (IBlogPostService blogPostService) =>
        {
            return blogPostService.GetAll();
        });

        app.MapGet("api/posts/{id}", (int id, IBlogPostService blogPostService) =>
        {
            var post = blogPostService.GetById(id);
            return post is not null ? Results.Ok(post) : Results.NotFound();
        });

        app.MapPost("/api/posts/", (BlogPost post, IBlogPostService blogPostService) =>
        {
            var createdPost = blogPostService.Add(post);
            return Results.Created($"/api/posts/{createdPost.Id}", createdPost);
        });

        app.MapPut("api/posts/{id}", (int id, BlogPost post, IBlogPostService blogPostService) =>
        {
            try
            {
                var updatedPost = blogPostService.Update(id, post);
                return Results.Ok(updatedPost);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        });

        app.MapDelete("api/posts/{id}", (int id, IBlogPostService blogPostService) =>
        {
            try
            {
                blogPostService.Delete(id);
                return Results.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        });

        app.MapPost("api/posts/{id}/comments", (int id, Comment comment, IBlogPostService blogPostService) =>
        {
            try
            {
                var addedComment = blogPostService.AddComment(id, comment);
                return Results.Ok(addedComment);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(ex.Message);
            }
        });
    }
}

