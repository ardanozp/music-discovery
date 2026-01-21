namespace DataPipeline.Scoring;

/// <summary>
/// Computes Time score from era and timelessness signals.
/// Time represents temporal feeling (0 = vintage, 1 = timeless).
/// </summary>
public class TimeScorer : IScorer
{
    // Signal names expected in the dictionary
    private const string EraScoreSignal = "EraScore";
    private const string TimelessnessSignal = "Timelessness";

    public double ComputeScore(Dictionary<string, double> signals)
    {
        // Get signals with fallback to neutral (0.5) if missing
        var eraScore = signals.GetValueOrDefault(EraScoreSignal, 0.5);
        var timelessness = signals.GetValueOrDefault(TimelessnessSignal, 0.5);

        // Weighted average:
        // - Era score: 60% (when the album was released)
        // - Timelessness: 40% (whether it transcends its era)
        // 
        // High era + high timelessness = Modern & Timeless
        // Low era + high timelessness = Vintage but Timeless (classic)
        // High era + low timelessness = Modern but dated
        var score = (eraScore * 0.6) + (timelessness * 0.4);

        return Math.Clamp(score, 0.0, 1.0);
    }
}
