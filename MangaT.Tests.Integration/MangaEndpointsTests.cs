using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using MangaT.ApplicationCore.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;

namespace MangaT.Tests.Integration;

/// <summary>
/// Pruebas de integración HTTP contra la API completa con base de datos en memoria.
/// </summary>
public class MangaEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public MangaEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetPopular_Should_Return_Created_Manga()
    {
        var token = await LoginAsync("admin", "Admin123!");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        await _client.PostAsJsonAsync("/api/v1/manga", new CreateMangaRequest
        {
            Title = "Spy x Family",
            Author = "Tatsuya Endo",
            Description = "A spy builds a fake family",
            Category = "Comedy",
            VolumeCount = 12,
            Point = 9.4,
            ImageUrl = "https://example.com/sxf.jpg",
            DetailUrl = "https://example.com/sxf"
        });

        var response = await _client.GetAsync("/api/v1/manga/popular?page=1&pageSize=5");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Spy x Family", content);
    }

    [Fact]
    public async Task Create_Should_Return_Unauthorized_Without_Token()
    {
        var request = new CreateMangaRequest
        {
            Title = "Demon Slayer",
            Author = "Koyoharu Gotouge",
            Point = 9.3
        };

        var response = await _client.PostAsJsonAsync("/api/v1/manga", request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Create_Should_Return_Created_With_Admin_Token()
    {
        var token = await LoginAsync("admin", "Admin123!");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var request = new CreateMangaRequest
        {
            Title = "Demon Slayer",
            Author = "Koyoharu Gotouge",
            Description = "A boy fights demons",
            Category = "Action",
            VolumeCount = 23,
            Point = 9.3,
            ImageUrl = "https://example.com/ds.jpg",
            DetailUrl = "https://example.com/ds"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/manga", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    /// <summary>Obtiene un JWT válido del endpoint de login demo.</summary>
    private async Task<string> LoginAsync(string username, string password)
    {
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", new LoginRequest
        {
            Username = username,
            Password = password
        });

        response.EnsureSuccessStatusCode();
        var login = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return login!.Token;
    }
}

/// <summary>Fábrica de la aplicación en entorno Testing (InMemory DB, sin migraciones SQL).</summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        builder.UseSetting(Microsoft.AspNetCore.Hosting.WebHostDefaults.EnvironmentKey, "Testing");
    }
}
