namespace RecommendationApi.Models;

/// <summary>
/// An album with pre-computed numeric scores derived from its artist's metadata.
/// This is the internal working model used by the recommendation engine.
/// </summary>
public class ScoredAlbum
{
    public string Title { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;
    public string AlbumMbid { get; set; } = string.Empty;
    public string ArtistMbid { get; set; } = string.Empty;
    public int Year { get; set; }
    public string CoverUrl { get; set; } = string.Empty;

    // Pre-computed scores (0.0 – 1.0) mapped from artist string attributes
    public float EnergyScore      { get; set; }
    public float FamiliarityScore { get; set; }
    public float TimeScore        { get; set; }
}
