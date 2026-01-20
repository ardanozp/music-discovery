namespace RecommendationApi.Models;

public class RecommendationRequest
{
    // Primary recommendation parameters
    public EnergyLevel Energy { get; set; }
    public EmotionLevel Emotion { get; set; }
    
    // Future parameters - not yet used
    public FamiliarityLevel? Familiarity { get; set; }
    public TimeFeeling? Time { get; set; }
}
