var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

List<BlogPost> posts = new List<BlogPost>
{
    new BlogPost(1, "First Post", "This is the first post."),
    new BlogPost(2, "Second Post", "This is the second post."),
    new BlogPost(3, "Third Post", "This is the third post.")
};

app.MapGet("/api/posts/", () =>
{
    return posts;
});

app.MapGet("api/posts/{id}", (int id) =>
{
    return posts.FirstOrDefault(p => p.Id == id);
});

app.MapPost("/api/posts/", (BlogPost post) =>
{
    post = post with { Id = posts.Count() + 1 };
    posts.Add(post);
    return post;
});

app.MapPut("api/posts/{id}", (int id, BlogPost post) =>
{
    var existingPost = posts.FirstOrDefault(p => p.Id == id);
    if (existingPost == null)
    {
        return Results.NotFound();
    }
    posts.Remove(existingPost);
    existingPost = existingPost with { Id = id, Title = post.Title, Content = post.Content };
    posts.Add(existingPost);
    return Results.Ok(existingPost);
});

app.MapDelete("api/posts/{id}", (int id) =>
{
    var post = posts.FirstOrDefault(p => p.Id == id);
    if (post == null)
    {
        return Results.NotFound();
    }
    posts.Remove(post);
    return Results.NoContent();
});

app.MapPost("api/posts/{id}/comments", (int id, Comment comment) =>
{
    var post = posts.FirstOrDefault(p => p.Id == id);
    if (post == null)
    {
        return Results.NotFound();
    }
    if (post.Comments.Any(x => x.Content == comment.Content))
    {
        return Results.Conflict("A comment with the same content already exists.");
    }
    post.Comments.Add(comment);
    return Results.Ok(comment);
});

app.Run();

internal record BlogPost(int Id, string Title, string Content)
{
    public List<Comment> Comments { get; set; } = [];
}

internal record Comment(int BlogId, string Author, string Content)
{
}