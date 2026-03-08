using System.Text.Json;
using RecommendationApi.Models;

namespace RecommendationApi.Services;

/// <summary>
/// Loads albums.json at startup and provides an in-memory cache of scored albums.
/// Registered as a singleton + IHostedService so it's ready before the first request.
/// </summary>
public class AlbumDataService : IHostedService
{
    private readonly IConfiguration _config;
    private readonly ILogger<AlbumDataService> _logger;
    private readonly IWebHostEnvironment _env;

    private List<ScoredAlbum> _scoredAlbums = new();

    public AlbumDataService(
        IConfiguration config,
        ILogger<AlbumDataService> logger,
        IWebHostEnvironment env)
    {
        _config = config;
        _logger = logger;
        _env    = env;
    }

    public IReadOnlyList<ScoredAlbum> ScoredAlbums => _scoredAlbums;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var rawPath = _config["AlbumsFilePath"]
            ?? throw new InvalidOperationException("AlbumsFilePath is not configured in appsettings.json.");

        // Resolve relative paths from ContentRootPath (project root), not the bin folder
        var path = Path.IsPathRooted(rawPath)
            ? rawPath
            : Path.GetFullPath(Path.Combine(_env.ContentRootPath, rawPath));

        _logger.LogInformation("Loading albums from {Path}", path);

        if (!File.Exists(path))
            throw new FileNotFoundException($"albums.json not found at: {path}");

        var json = await File.ReadAllTextAsync(path, cancellationToken);
        var artists = JsonSerializer.Deserialize<List<ArtistEntry>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (artists == null || artists.Count == 0)
            throw new InvalidDataException("albums.json was empty or could not be parsed.");

        _scoredAlbums = FlattenAndScore(artists);
        _logger.LogInformation("Loaded {Count} scored albums from {ArtistCount} artists.",
            _scoredAlbums.Count, artists.Count);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    // ── Private ───────────────────────────────────────────────────────────────

    private static List<ScoredAlbum> FlattenAndScore(List<ArtistEntry> artists)
    {
        var result = new List<ScoredAlbum>();

        foreach (var artist in artists)
        {
            // Pre-compute scores once per artist (energy, familiarity are artist-level)
            var energyScore      = ScoringService.EnergyScore(artist.EnergyLevel);
            var familiarityScore = ScoringService.FamiliarityScore(artist.Listeners);

            foreach (var album in artist.Albums)
            {
                // Skip albums with no year or no MBID (can't uniquely identify them)
                if (album.Year == null || string.IsNullOrWhiteSpace(album.Mbid))
                    continue;

                // TimeScore is album-level: based on actual release year
                var timeScore = ScoringService.TimeScore(album.Year.Value);

                result.Add(new ScoredAlbum
                {
                    Title            = album.Title,
                    ArtistName       = artist.Name,
                    AlbumMbid        = album.Mbid!,
                    ArtistMbid       = artist.Mbid ?? string.Empty,
                    Year             = album.Year.Value,
                    CoverUrl         = album.CoverUrl,
                    EnergyScore      = energyScore,
                    FamiliarityScore = familiarityScore,
                    TimeScore        = timeScore
                });
            }
        }

        return result;
    }
}
