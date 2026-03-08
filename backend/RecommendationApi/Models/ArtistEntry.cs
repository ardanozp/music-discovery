using System.Text.Json.Serialization;

namespace RecommendationApi.Models;

/// <summary>
/// Represents a single artist entry from albums.json.
/// </summary>
public class ArtistEntry
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("mbid")]
    public string? Mbid { get; set; }

    [JsonPropertyName("supergenre")]
    public string Supergenre { get; set; } = string.Empty;

    [JsonPropertyName("energy_level")]
    public string EnergyLevel { get; set; } = string.Empty;

    [JsonPropertyName("mood")]
    public string Mood { get; set; } = string.Empty;

    [JsonPropertyName("era")]
    public string Era { get; set; } = string.Empty;

    [JsonPropertyName("popularity_tier")]
    public string PopularityTier { get; set; } = string.Empty;

    [JsonPropertyName("listeners")]
    public long Listeners { get; set; }

    [JsonPropertyName("albums")]
    public List<AlbumEntry> Albums { get; set; } = new();
}

/// <summary>
/// Represents a single album nested under an artist in albums.json.
/// </summary>
public class AlbumEntry
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("mbid")]
    public string? Mbid { get; set; }

    [JsonPropertyName("year")]
    public int? Year { get; set; }

    [JsonPropertyName("cover_url")]
    public string CoverUrl { get; set; } = string.Empty;
}
