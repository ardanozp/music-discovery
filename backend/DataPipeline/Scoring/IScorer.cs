namespace DataPipeline.Scoring;

/// <summary>
/// Base interface for all scorers.
/// Each scorer combines multiple signals to produce a final psychological profile score.
/// Scorers apply weighted averaging (rule-based v1).
/// </summary>
public interface IScorer
{
    /// <summary>
    /// Computes a final score from input signals.
    /// </summary>
    /// <param name="signals">Dictionary of signal name to signal value (0.0-1.0)</param>
    /// <returns>Final score between 0.0 and 1.0</returns>
    double ComputeScore(Dictionary<string, double> signals);
}
