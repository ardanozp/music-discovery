namespace RecommendationApi.Models;

/// <summary>
/// DTO for album data in API responses.
/// Contains basic album information plus computed scores from the enrichment pipeline.
/// </summary>
public class AlbumDto
{
    /// <summary>
    /// Unique album identifier.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Album title.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Primary artist or band name.
    /// </summary>
    public string Artist { get; set; } = string.Empty;
    
    /// <summary>
    /// Release year.
    /// </summary>
    public int Year { get; set; }
    
    /// <summary>
    /// URL to album cover image.
    /// </summary>
    public string CoverUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// URL to Wikipedia page.
    /// </summary>
    public string WikipediaUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// Computed energy score (0.0 - 1.0).
    /// </summary>
    public float EnergyScore { get; set; }
    
    /// <summary>
    /// Computed emotion score (0.0 - 1.0).
    /// </summary>
    public float EmotionScore { get; set; }
    
    /// <summary>
    /// Computed familiarity score (0.0 - 1.0).
    /// </summary>
    public float FamiliarityScore { get; set; }
    
    /// <summary>
    /// Computed time score (0.0 - 1.0).
    /// </summary>
    public float TimeScore { get; set; }
}
