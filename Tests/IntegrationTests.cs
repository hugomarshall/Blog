using Blog.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace Tests;

public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly BlogDbContext _context;

    public IntegrationTests(WebApplicationFactory<Program> factory)
    {
        var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();
        IServiceScope scope = scopeFactory.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
        _client = factory.CreateClient();
    }

    private async Task<BlogPost?> CreateNewPostAsync(string title, string content)
    {
        var newPost = new { title, content };
        var response = await _client.PostAsJsonAsync("/api/posts/", newPost);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BlogPost>();
    }

    [Fact, TestPriority(1)]
    [Trait("Category", "Integration")]
    public async Task GetAllPosts_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/posts/");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<List<BlogPost>>();
        Assert.NotNull(content);
    }

    [Fact, TestPriority(2)]
    [Trait("Category", "Integration")]
    public async Task GetPostById_ReturnsOk()
    {
        var post = await CreateNewPostAsync("New Post", "This is a new post.");
        Assert.NotNull(post);

        var response = await _client.GetAsync($"/api/posts/{post.Id}");
        response.EnsureSuccessStatusCode();
        var getContent = await response.Content.ReadFromJsonAsync<BlogPost>();
        Assert.NotNull(getContent);
        Assert.Equal(post.Id, getContent.Id);

    }

    [Fact, TestPriority(3)]
    [Trait("Category", "Integration")]
    public async Task CreatePost_ReturnsCreated()
    {
        var post = await CreateNewPostAsync("New Post", "This is a new post.");
        Assert.NotNull(post);
        Assert.True(post.Id > 0);

        var createdPost = await _context.Posts.FindAsync(post.Id);
        Assert.NotNull(createdPost);
    }

    [Fact, TestPriority(4)]
    [Trait("Category", "Integration")]
    public async Task UpdatePost_ReturnsOk()
    {
        var post = await CreateNewPostAsync("New Post", "This is a new post.");
        Assert.NotNull(post);

        var updatedPost = new { title = "Updated Post", content = "This is an updated post." };
        var response = await _client.PutAsJsonAsync($"/api/posts/{post.Id}", updatedPost);
        response.EnsureSuccessStatusCode();

        var updatedContent = await response.Content.ReadFromJsonAsync<BlogPost>();
        Assert.NotNull(updatedContent);
        Assert.Equal("Updated Post", updatedContent.Title);

        var dbPost = await _context.Posts.FindAsync(post.Id);
        Assert.NotNull(dbPost);
        Assert.Equal("Updated Post", dbPost.Title);
    }

    [Fact, TestPriority(5)]
    [Trait("Category", "Integration")]
    public async Task DeletePost_ReturnsNoContent()
    {
        var post = await CreateNewPostAsync("New Post", "This is a new post.");
        Assert.NotNull(post);

        var response = await _client.DeleteAsync($"/api/posts/{post.Id}");
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);

        var deletedPost = await _context.Posts.FindAsync(post.Id);
        Assert.Null(deletedPost);
    }

    [Fact, TestPriority(6)]
    [Trait("Category", "Integration")]
    public async Task AddCommentToPost_ReturnsOk()
    {
        var post = await CreateNewPostAsync("New Post", "This is a new post.");
        Assert.NotNull(post);

        var comment = new Comment(post.Id, "Author", "This is a comment.");
        var response = await _client.PostAsJsonAsync($"/api/posts/{post.Id}/comments", comment);
        response.EnsureSuccessStatusCode();

        var commentResponse = await response.Content.ReadFromJsonAsync<Comment>();
        Assert.NotNull(commentResponse);
        Assert.Contains("This is a comment.", commentResponse.Content);
    }

    [Theory, TestPriority(7)]
    [Trait("Category", "Integration")]
    [InlineData(999)]
    public async Task GetPostById_ReturnsNotFound_ForNonExistentPost(int postId)
    {
        var response = await _client.GetAsync($"/api/posts/{postId}");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory, TestPriority(8)]
    [Trait("Category", "Integration")]
    [InlineData(999)]
    public async Task UpdatePost_ReturnsNotFound_ForNonExistentPost(int postId)
    {
        var updatedPost = new { title = "Updated Post", content = "This is an updated post." };
        var response = await _client.PutAsJsonAsync($"/api/posts/{postId}", updatedPost);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory, TestPriority(9)]
    [Trait("Category", "Integration")]
    [InlineData(999)]
    public async Task DeletePost_ReturnsNotFound_ForNonExistentPost(int postId)
    {
        var response = await _client.DeleteAsync($"/api/posts/{postId}");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}
