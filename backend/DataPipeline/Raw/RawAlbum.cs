namespace DataPipeline.Raw;

/// <summary>
/// Represents raw album data before enrichment.
/// This is the INPUT to the enrichment pipeline.
/// </summary>
public class RawAlbum
{
    /// <summary>
    /// Unique identifier for the album.
    /// Preserved in the output for correlation.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Album title.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Primary artist or band name.
    /// </summary>
    public required string Artist { get; init; }

    /// <summary>
    /// Release year (e.g., 1977, 2023).
    /// </summary>
    public required int Year { get; init; }

    /// <summary>
    /// List of genre tags (e.g., ["Rock", "Progressive Rock", "Art Rock"]).
    /// Used for genre-based signal extraction.
    /// </summary>
    public required List<string> Genres { get; init; }

    /// <summary>
    /// Popularity score from 0 to 100.
    /// Higher values indicate more mainstream/popular albums.
    /// </summary>
    public required int Popularity { get; init; }

    /// <summary>
    /// Type of record label (Major, Indie, Unknown).
    /// Provided as string to ensure loose coupling.
    /// </summary>
    public required string LabelType { get; init; }

    /// <summary>
    /// URL to the album cover image.
    /// </summary>
    public required string CoverUrl { get; init; }

    /// <summary>
    /// URL to the album's Wikipedia page.
    /// </summary>
    public required string WikipediaUrl { get; init; }

    /// <summary>
    /// Audio feature: Energy level (0.0 - 1.0).
    /// Represents intensity and activity. Optional - may be null if unavailable.
    /// </summary>
    public double? Energy { get; init; }

    /// <summary>
    /// Audio feature: Valence (0.0 - 1.0).
    /// Represents musical positiveness/happiness. Optional - may be null if unavailable.
    /// </summary>
    public double? Valence { get; init; }

    /// <summary>
    /// Audio feature: Danceability (0.0 - 1.0).
    /// Represents how suitable for dancing. Optional - may be null if unavailable.
    /// </summary>
    public double? Danceability { get; init; }

    /// <summary>
    /// Audio feature: Acousticness (0.0 - 1.0).
    /// Represents confidence that the track is acoustic. Optional - may be null if unavailable.
    /// </summary>
    public double? Acousticness { get; init; }
}
