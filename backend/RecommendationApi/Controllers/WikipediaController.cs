using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace RecommendationApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WikipediaController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WikipediaController> _logger;

    public WikipediaController(IHttpClientFactory httpClientFactory, ILogger<WikipediaController> logger)
    {
        _httpClient = httpClientFactory.CreateClient("Wikipedia");
        _logger = logger;
    }

    [HttpGet("redirect")]
    public async Task<IActionResult> RedirectToArticle(
        [FromQuery] string album,
        [FromQuery] string artist,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(album) || string.IsNullOrWhiteSpace(artist))
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Both album and artist parameters are required.",
                Type = "https://httpstatuses.com/400"
            });
        }
        if (album.Length > 200 || artist.Length > 200)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "album and artist parameters are too long.",
                Type = "https://httpstatuses.com/400"
            });
        }

        try
        {
            // Query the Wikipedia Search API
            // srlimit=1 ensures we just get the top hit
            var query = Uri.EscapeDataString($"{album} {artist} album");
            var url = $"https://en.wikipedia.org/w/api.php?action=query&list=search&srlimit=1&srsearch={query}&utf8=&format=json";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var jsonStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var jsonDoc = await JsonDocument.ParseAsync(jsonStream, cancellationToken: cancellationToken);

            // Parse response: { "query": { "search": [ { "title": "Never Gone" } ] } }
            if (jsonDoc.RootElement.TryGetProperty("query", out var queryProp) &&
                queryProp.TryGetProperty("search", out var searchProp) &&
                searchProp.ValueKind == JsonValueKind.Array &&
                searchProp.GetArrayLength() > 0)
            {
                var topHit = searchProp[0];
                if (topHit.TryGetProperty("title", out var titleProp))
                {
                    var exactTitle = titleProp.GetString();
                    if (!string.IsNullOrWhiteSpace(exactTitle))
                    {
                        var encodedTitle = Uri.EscapeDataString(exactTitle.Replace(" ", "_"));
                        var redirectUrl = $"https://en.wikipedia.org/wiki/{encodedTitle}";
                        return Redirect(redirectUrl);
                    }
                }
            }

            // Fallback: If search yields 0 results, fallback to a standard search page
            _logger.LogWarning("No Wikipedia results found for album '{Album}' by '{Artist}'", album, artist);
            return Redirect($"https://en.wikipedia.org/w/index.php?search={query}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching from Wikipedia API.");
            // On error, fallback to standard search
            var query = Uri.EscapeDataString($"{album} {artist} album");
            return Redirect($"https://en.wikipedia.org/w/index.php?search={query}");
        }
    }
}
