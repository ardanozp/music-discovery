namespace RecommendationApi.Models;

/// <summary>
/// Response model for recommendation endpoint.
/// Contains a list of recommended albums sorted by relevance.
/// </summary>
public class RecommendationResponse
{
    /// <summary>
    /// List of recommended albums, sorted by distance to user preferences (closest first).
    /// </summary>
    public List<AlbumDto> Albums { get; set; } = new();
}
