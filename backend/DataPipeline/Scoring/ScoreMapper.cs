namespace DataPipeline.Scoring;

/// <summary>
/// Static helper class for mapping continuous scores to internal enum levels,
/// and normalizing enum levels back to standard numeric scores for output.
/// Implements the "Quantization" logic of Rule v1.
/// </summary>
internal static class ScoreMapper
{
    // ========================================================================
    // Raw Score -> Internal Enum (Decision Phase)
    // ========================================================================

    /// <summary>
    /// Maps raw energy score to internal EnergyLevel.
    /// Low: 0.0-0.33, Mid: 0.34-0.66, High: 0.67-1.0
    /// </summary>
    public static EnergyLevel MapToEnergyLevel(double rawScore)
    {
        return rawScore switch
        {
            < 0.34 => EnergyLevel.Low,
            < 0.67 => EnergyLevel.Mid,
            _ => EnergyLevel.High
        };
    }

    /// <summary>
    /// Maps raw emotion score to internal EmotionLevel.
    /// Light: 0.0-0.5, Deep: 0.5-1.0
    /// </summary>
    public static EmotionLevel MapToEmotionLevel(double rawScore)
    {
        return rawScore switch
        {
            < 0.5 => EmotionLevel.Light,
            _ => EmotionLevel.Deep
        };
    }

    /// <summary>
    /// Maps raw familiarity score to internal FamiliarityLevel.
    /// Exploratory: 0.0-0.5, Familiar: 0.5-1.0
    /// </summary>
    public static FamiliarityLevel MapToFamiliarityLevel(double rawScore)
    {
        return rawScore switch
        {
            < 0.5 => FamiliarityLevel.Exploratory,
            _ => FamiliarityLevel.Familiar
        };
    }

    /// <summary>
    /// Maps raw time score to internal TimeFeeling.
    /// Past: 0.0-0.33, Timeless: 0.34-0.66, Now: 0.67-1.0
    /// </summary>
    public static TimeFeeling MapToTimeFeeling(double rawScore)
    {
        return rawScore switch
        {
            < 0.34 => TimeFeeling.Past,
            < 0.67 => TimeFeeling.Timeless,
            _ => TimeFeeling.Now
        };
    }

    // ========================================================================
    // Internal Enum -> Normalized Score (Output Phase)
    // ========================================================================

    /// <summary>
    /// Converts internal EnergyLevel to normalized output score.
    /// Returns the center value of the level's range.
    /// </summary>
    public static double GetScore(EnergyLevel level)
    {
        return level switch
        {
            EnergyLevel.Low => 0.15,
            EnergyLevel.Mid => 0.5,
            EnergyLevel.High => 0.85,
            _ => 0.5
        };
    }

    /// <summary>
    /// Converts internal EmotionLevel to normalized output score.
    /// </summary>
    public static double GetScore(EmotionLevel level)
    {
        return level switch
        {
            EmotionLevel.Light => 0.25,
            EmotionLevel.Deep => 0.75,
            _ => 0.5
        };
    }

    /// <summary>
    /// Converts internal FamiliarityLevel to normalized output score.
    /// </summary>
    public static double GetScore(FamiliarityLevel level)
    {
        return level switch
        {
            FamiliarityLevel.Exploratory => 0.25,
            FamiliarityLevel.Familiar => 0.75,
            _ => 0.5
        };
    }

    /// <summary>
    /// Converts internal TimeFeeling to normalized output score.
    /// </summary>
    public static double GetScore(TimeFeeling feeling)
    {
        return feeling switch
        {
            TimeFeeling.Past => 0.15,
            TimeFeeling.Timeless => 0.5,
            TimeFeeling.Now => 0.85,
            _ => 0.5
        };
    }
}
