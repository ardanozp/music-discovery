namespace RecommendationApi.Models;

/// <summary>
/// DTO for album data in API responses.
/// </summary>
public class AlbumDto
{
    /// <summary>Album MBID (MusicBrainz identifier).</summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>Album title.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Primary artist or band name.</summary>
    public string Artist { get; set; } = string.Empty;

    /// <summary>Release year.</summary>
    public int Year { get; set; }

    /// <summary>URL to album cover image (Cover Art Archive).</summary>
    public string CoverUrl { get; set; } = string.Empty;

    /// <summary>Link to search for the album on Wikipedia.</summary>
    public string WikipediaUrl { get; set; } = string.Empty;

    /// <summary>Energy score (0.0 low – 1.0 high).</summary>
    public float EnergyScore { get; set; }

    /// <summary>Familiarity score (0.0 underground – 1.0 mainstream).</summary>
    public float FamiliarityScore { get; set; }

    /// <summary>Time score (0.0 vintage – 1.0 modern).</summary>
    public float TimeScore { get; set; }
}
