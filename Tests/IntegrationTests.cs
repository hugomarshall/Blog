using Blog.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace Tests;

public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public IntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact, TestPriority(1)]
    public async Task GetAllPosts_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/posts/");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Post", content);
    }

    [Fact, TestPriority(2)]
    public async Task GetPostById_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/posts/1");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("First Post", content);
    }

    [Fact, TestPriority(3)]
    public async Task CreatePost_ReturnsCreated()
    {
        var newPost = new { title = "New Post", content = "This is a new post." };
        var response = await _client.PostAsJsonAsync("/api/posts/", newPost);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("New Post", content);
    }

    [Fact, TestPriority(4)]
    public async Task UpdatePost_ReturnsOk()
    {
        var updatedPost = new { title = "Updated Post", content = "This is an updated post." };
        var response = await _client.PutAsJsonAsync("/api/posts/1", updatedPost);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Updated Post", content);
    }

    [Fact, TestPriority(5)]
    public async Task DeletePost_ReturnsNoContent()
    {
        var response = await _client.DeleteAsync("/api/posts/2");
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact, TestPriority(6)]
    public async Task AddCommentToPost_ReturnsOk()
    {
        Comment comment = new(1, "Author", "This is a comment.");
        var response = await _client.PostAsJsonAsync("/api/posts/1/comments", comment);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("This is a comment.", content);
    }
}
