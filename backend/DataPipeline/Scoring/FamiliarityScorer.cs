namespace DataPipeline.Scoring;

/// <summary>
/// Computes Familiarity score from popularity, label scale, and genre signals.
/// Familiarity represents how well-known/mainstream an album is.
/// </summary>
public class FamiliarityScorer : IScorer
{
    // Signal names expected in the dictionary
    private const string PopularitySignal = "Popularity";
    private const string LabelScaleSignal = "LabelScale";
    private const string GenreWeightSignal = "GenreWeight";

    public double ComputeScore(Dictionary<string, double> signals)
    {
        // Get signals with fallback to neutral (0.5) if missing
        var popularity = signals.GetValueOrDefault(PopularitySignal, 0.5);
        var labelScale = signals.GetValueOrDefault(LabelScaleSignal, 0.5);
        var genreWeight = signals.GetValueOrDefault(GenreWeightSignal, 0.5);

        // Weighted average:
        // - Popularity: 50% (strongest indicator of mainstream appeal)
        // - Label scale: 30% (major labels = more mainstream)
        // - Genre weight: 20% (some genres are inherently more mainstream)
        var score = (popularity * 0.5) + (labelScale * 0.3) + (genreWeight * 0.2);

        return Math.Clamp(score, 0.0, 1.0);
    }
}
