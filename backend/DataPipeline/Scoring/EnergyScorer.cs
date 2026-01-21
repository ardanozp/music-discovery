namespace DataPipeline.Scoring;

/// <summary>
/// Computes Energy score from genre and audio energy signals.
/// Energy represents intensity, activity, and power.
/// </summary>
public class EnergyScorer : IScorer
{
    // Signal names expected in the dictionary
    private const string GenreWeightSignal = "GenreWeight";
    private const string AudioEnergySignal = "AudioEnergy";

    public double ComputeScore(Dictionary<string, double> signals)
    {
        // Get signals with fallback to neutral (0.5) if missing
        var genreWeight = signals.GetValueOrDefault(GenreWeightSignal, 0.5);
        var audioEnergy = signals.GetValueOrDefault(AudioEnergySignal, 0.5);

        // Weighted average:
        // - Genre weight: 60% (genre is a strong indicator of energy)
        // - Audio energy: 40% (direct measurement when available)
        var score = (genreWeight * 0.6) + (audioEnergy * 0.4);

        return Math.Clamp(score, 0.0, 1.0);
    }
}
