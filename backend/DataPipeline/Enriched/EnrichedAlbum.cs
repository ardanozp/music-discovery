namespace DataPipeline.Enriched;

/// <summary>
/// Represents an album after enrichment with psychological profile scores.
/// OUTPUT: Strictly numeric scores for decision making.
/// No Enums, no Metadata, no Raw Data.
/// </summary>
public class EnrichedAlbum
{
    /// <summary>
    /// Unique identifier for correlation.
    /// </summary>
    public required string Id { get; init; }

    public required string Title { get; init; }
    public required string Artist { get; init; }
    public required int Year { get; init; }
    public required string CoverUrl { get; init; }
    public required string WikipediaUrl { get; init; }

    /// <summary>
    /// Energy score (0.0 - 1.0).
    /// Represents intensity, activity, and power of the album.
    /// </summary>
    public required double EnergyScore { get; init; }

    /// <summary>
    /// Emotion score (0.0 - 1.0).
    /// Represents emotional depth/positivity.
    /// </summary>
    public required double EmotionScore { get; init; }

    /// <summary>
    /// Familiarity score (0.0 - 1.0).
    /// Represents how well-known/mainstream the album is.
    /// </summary>
    public required double FamiliarityScore { get; init; }

    /// <summary>
    /// Time score (0.0 - 1.0).
    /// Represents temporal feeling.
    /// </summary>
    public required double TimeScore { get; init; }
}
