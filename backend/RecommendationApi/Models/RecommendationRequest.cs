namespace RecommendationApi.Models;

/// <summary>
/// Request model for album recommendations.
/// Contains user preference scores (0.0 - 1.0) for each dimension.
/// </summary>
public class RecommendationRequest
{
    /// <summary>
    /// Energy preference score (0.0 = low energy, 1.0 = high energy).
    /// </summary>
    public float Energy { get; set; }
    
    /// <summary>
    /// Emotion preference score (0.0 = light/happy, 1.0 = deep/complex).
    /// </summary>
    public float Emotion { get; set; }
    
    /// <summary>
    /// Familiarity preference score (0.0 = exploratory/niche, 1.0 = familiar/mainstream).
    /// </summary>
    public float Familiarity { get; set; }
    
    /// <summary>
    /// Time preference score (0.0 = vintage/past, 1.0 = modern/now).
    /// </summary>
    public float Time { get; set; }
}
