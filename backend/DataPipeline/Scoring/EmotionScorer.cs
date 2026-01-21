namespace DataPipeline.Scoring;

/// <summary>
/// Computes Emotion score from valence and harmonic depth signals.
/// Emotion represents emotional positivity (0 = sad, 1 = euphoric).
/// </summary>
public class EmotionScorer : IScorer
{
    // Signal names expected in the dictionary
    private const string ValenceDepthSignal = "ValenceDepth";
    private const string HarmonicDepthSignal = "HarmonicDepth";

    public double ComputeScore(Dictionary<string, double> signals)
    {
        // Get signals with fallback to neutral (0.5) if missing
        var valenceDepth = signals.GetValueOrDefault(ValenceDepthSignal, 0.5);
        var harmonicDepth = signals.GetValueOrDefault(HarmonicDepthSignal, 0.5);

        // Weighted average:
        // - Valence depth: 70% (direct emotional positivity measurement)
        // - Harmonic depth: 30% (complex harmonies can add emotional richness)
        var score = (valenceDepth * 0.7) + (harmonicDepth * 0.3);

        return Math.Clamp(score, 0.0, 1.0);
    }
}
