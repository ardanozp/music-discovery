using System.ComponentModel.DataAnnotations;
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
    [Range(0, 1)]
    public float Energy { get; set; }
    
    /// <summary>
    /// Familiarity preference score (0.0 = exploratory/niche, 1.0 = familiar/mainstream).
    /// </summary>
    [Range(0, 1)]
    public float Familiarity { get; set; }
    
    /// <summary>
    /// Time preference score (0.0 = vintage/past, 1.0 = modern/now).
    /// </summary>
    [Range(0, 1)]
    public float Time { get; set; }
}
